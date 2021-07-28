﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;
using MLAPI;
using MLAPI.NetworkVariable;

public class PhysicsMachine : NetworkBehaviour
{
    public enum States { Stay, Fall, WallSlide }

    public class PointerOffset 
    {
        public PointerOffset(){}
        public PointerOffset(Vector2 offset, float noise) 
        {
            bodyOffset = offset;
            noiseScale = noise;
        }

        public Vector2 bodyOffset = Vector2.zero;
        public float transitionSpeed = 1f;
        public float noiseScale = 0f;
    }

    [Header("Animation")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float frequencyScale = 1f;
    [SerializeField] private float frequencySpeed = 1f;

    [Header("Motion")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Transform armRoot;

    [Header("Particles")]
    [SerializeField] private ParticleSystem slideParticles;
    [SerializeField] private ParticleSystem fallDownParticles;

    [HideInInspector] public Dictionary<Rigidbody2D, PointerOffset> offsets;

    private int _randSeed;
    private float _trueTime;
    private PointerFiller _filler;
    private DirectionController _direction;
    private FightController _fight;
    private IControll _controll;
    private States _currentState;

    private Rigidbody2D bodyRb;
    private Rigidbody2D frontArmRb;
    private Rigidbody2D backArmRb;
    private Rigidbody2D frontLegRb;
    private Rigidbody2D backLegRb;

    private NetworkVariableFloat _ownerTime;
    private NetworkVariableInt _ownerSeed;

    public States CurrentState => _currentState;

    private void Awake()
    {
        NetworkVariableSettings sets = new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.Everyone
        };

        _ownerTime = new NetworkVariableFloat(sets);
        _ownerSeed = new NetworkVariableInt(sets);

        _ownerTime.Value = 1f;
        _ownerSeed.Value = 1;
    }

    private void Start()
    {
        _controll = GetComponent<IControll>();
        _filler = GetComponent<PointerFiller>();
        _direction = GetComponent<DirectionController>();
        _fight = GetComponent<FightController>();

        _randSeed = UnityEngine.Random.Range(0, 100);

        bodyRb = _filler.GetPointer(KinematicsPointerType.Body);
        frontArmRb = _filler.GetPointer(KinematicsPointerType.FrontArm);
        backArmRb = _filler.GetPointer(KinematicsPointerType.BackArm);
        frontLegRb = _filler.GetPointer(KinematicsPointerType.FrontLeg);
        backLegRb = _filler.GetPointer(KinematicsPointerType.BackLeg);

        offsets = new Dictionary<Rigidbody2D, PointerOffset> 
        { 
            { frontArmRb, new PointerOffset(new Vector2(1f, 2f), 2f)},
            { backArmRb, new PointerOffset(new Vector2(1f, 2f), 2f)},
            { frontLegRb, new PointerOffset(new Vector2(0f, -1f), 2f)},
            { backLegRb, new PointerOffset(new Vector2(0f, -1f), 2f)},
        };

        SwitchState(States.Stay);
    }

    private void FixedUpdate()
    {
        switch (_currentState) 
        {
            case States.Stay:
                StayUpdate();
                break;
            case States.Fall:
                FallUpdate();
                break;
            case States.WallSlide:
                WallSlideUpdate();
                break;
        }

        if (IsOwner) 
        {
            _trueTime = Time.time;
            if (_ownerSeed.Value != _randSeed) _ownerSeed.Value = _randSeed;
            if (_ownerTime.Value != _trueTime) _ownerTime.Value = _trueTime;
        }
        else 
        {
            _randSeed = _ownerSeed.Value;
            _trueTime = _ownerTime.Value;
        }
    }

    private void SwitchState(States state) 
    {
        _currentState = state;
        switch (state) 
        {
            case States.Stay:
                SetStateStay();
                break;
            case States.Fall:
                SetStateFall();
                break;
            case States.WallSlide:
                SetStateWallSlide();
                break;
        }
    }

    #region State Stay
    private void SetStateStay() 
    {
        bodyRb.gravityScale = 0f;
        bodyRb.drag = 3f;
        bodyRb.freezeRotation = false;
        bodyRb.angularDrag = 10f;

        RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        if (hit) 
        {
            fallDownParticles.transform.position = hit.point;
            fallDownParticles.Play();
        }
    }

    private void StayUpdate()
    {
        StayBody();
        StayLegs();
        StayArms();
    }

    private void StayBody()
    {
        if (_filler.GetTween(bodyRb) == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
            if (hit && Vector3.Distance(bodyRb.position, hit.point) <= DataGameMain.Default.personStandHeight)
            {
                Debug.DrawLine(bodyRb.position, hit.point, Color.red);
                Debug.DrawLine(bodyRb.position, new Vector2(hit.point.x, hit.point.y + DataGameMain.Default.personStandHeight), Color.green);
                Vector3 position = bodyRb.position;
                if (_controll.GetMoveDown())
                    position.y = Mathf.Lerp(position.y, hit.point.y + DataGameMain.Default.personStandHeight / 2f, Time.fixedDeltaTime * animationSpeed);
                else
                    position.y = Mathf.Lerp(position.y, hit.point.y + DataGameMain.Default.personStandHeight, Time.fixedDeltaTime * animationSpeed);
                position.y += (-Mathf.PerlinNoise(_randSeed, _trueTime * frequencySpeed)) * frequencyScale;

                if (_controll.GetMoveLeft()) bodyRb.AddForce(Vector2.left * walkSpeed);
                else if (_controll.GetMoveRight()) bodyRb.AddForce(Vector2.right * walkSpeed);

                RaycastHit2D hitLeft = Physics2D.Raycast(bodyRb.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));
                RaycastHit2D hitRight = Physics2D.Raycast(bodyRb.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
                if (hitLeft && !_controll.GetMoveRight()) bodyRb.velocity = Vector2.zero;
                if (hitRight && !_controll.GetMoveLeft()) bodyRb.velocity = Vector2.zero;

                bodyRb.position = position;
                bodyRb.transform.rotation = Quaternion.Lerp(bodyRb.transform.rotation, Quaternion.Euler(Vector3.zero), Time.fixedDeltaTime * animationSpeed);

                if (_controll.GetJump())
                {
                    bodyRb.AddForce(Vector2.up * jumpForce * bodyRb.mass, ForceMode2D.Impulse);
                    SwitchState(States.Fall);
                    return;
                }
            }
            else
            {
                SwitchState(States.Fall);
                return;
            }
        }
    }

    private void StayLegs() 
    {
        if (!frontLegRb.isKinematic || !backLegRb.isKinematic) 
        {
            SwitchState(States.Fall);
            return;
        }

        if (_filler.GetTween(frontLegRb) == null && _filler.GetTween(backLegRb) == null)
        {

            if (frontLegRb.position.x == backLegRb.position.x)
                _filler.PushMotion(frontLegRb, PointerMotion.LegStep);

            Rigidbody2D leftLeg = frontLegRb.position.x < backLegRb.position.x ? frontLegRb : backLegRb;
            Rigidbody2D rightLeg = frontLegRb.position.x < backLegRb.position.x ? backLegRb : frontLegRb;

            if (bodyRb.position.x < leftLeg.position.x)
                _filler.PushMotion(rightLeg, PointerMotion.LegStep);
            else if (bodyRb.position.x > rightLeg.position.x)
                _filler.PushMotion(leftLeg, PointerMotion.LegStep);
            else if (Mathf.Abs(frontLegRb.position.x - bodyRb.position.x) > DataGameMain.Default.personStepLenght)
                _filler.PushMotion(frontLegRb, PointerMotion.LegToNormalDistance);
            else if (Mathf.Abs(backLegRb.position.x - bodyRb.position.x) > DataGameMain.Default.personStepLenght)
                _filler.PushMotion(backLegRb, PointerMotion.LegToNormalDistance);
        }

        foreach (Rigidbody2D rb in new Rigidbody2D[]{frontLegRb, backLegRb})
        {
            if (_filler.GetTween(rb) == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
                if (hit) 
                {
                    float posY = Mathf.Lerp(rb.position.y, hit.point.y, 0.3f);
                    rb.position = new Vector2(rb.position.x, posY);
                }
            }
        }
    }

    private void StayArms() 
    {
        List<Rigidbody2D> rbs = new List<Rigidbody2D>{ frontArmRb, backArmRb };
        foreach (Rigidbody2D rb in rbs) 
        {
            if (_filler.GetTween(rb) == null && rb.isKinematic)
            {
                if (_fight.AimValue > 0f && _controll.GetAttackButton())
                {
                    Vector2 offset = _fight.AimOffset;
                    offset.x *= _direction.Direction;
                    Vector2 targetPosition = (Vector2)armRoot.position + offset;
                    rb.position = Vector2.Lerp(_fight.StartPosition, targetPosition, _fight.AimValue);
                }
                else 
                {
                    Vector2 offset = offsets[rb].bodyOffset;
                    offset.x *= _direction.Direction;
                    Vector2 position = bodyRb.position + offset;
                    position.x += (Mathf.PerlinNoise(_trueTime, _randSeed + rbs.IndexOf(rb)) - 0.5f) * offsets[rb].noiseScale;
                    position.y += (Mathf.PerlinNoise(_randSeed + rbs.IndexOf(rb), _trueTime) - 0.5f) * offsets[rb].noiseScale;
                    rb.position = Vector2.Lerp(rb.position, position, Time.fixedDeltaTime * offsets[rb].transitionSpeed * (bodyRb.velocity.x + 1));
                }
            }
        }
    }
    #endregion

    #region State Fall
    private void SetStateFall() 
    {
        bodyRb.gravityScale = 1f;
        bodyRb.drag = 0f;
        bodyRb.freezeRotation = false;
        bodyRb.angularDrag = 0f;
    }

    private void FallUpdate() 
    {
        RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        if (hit && Vector3.Distance(bodyRb.position, hit.point) <= DataGameMain.Default.personStandHeight / 2f && bodyRb.velocity.y <= 0f)
        {
            SwitchState(States.Stay);
            return;
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(bodyRb.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(bodyRb.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
        bool rotationOk = bodyRb.transform.rotation.eulerAngles.z > -20f && bodyRb.transform.rotation.eulerAngles.z < 20f;
        if ((hitLeft || hitRight) && rotationOk)
        {
            SwitchState(States.WallSlide);
            return;
        }

        if (_controll.GetAttackButton() || !_controll.GetJump()) 
        {
            bodyRb.angularVelocity = 0f;
            bodyRb.transform.rotation = Quaternion.Lerp(bodyRb.transform.rotation, Quaternion.Euler(Vector3.zero), Time.fixedDeltaTime * animationSpeed * 2f);
        }            
        else
            bodyRb.angularVelocity = bodyRb.velocity.x * -30f;
        bodyRb.angularVelocity = Mathf.Clamp(bodyRb.angularVelocity, -250f, 250f);

        List<Rigidbody2D> rbs = new List<Rigidbody2D> { frontLegRb, backLegRb };
        foreach (Rigidbody2D rb in rbs)
        {
            if (_filler.GetTween(rb) == null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                Vector2 offset = offsets[rb].bodyOffset;
                offset.x *= _direction.Direction;
                Vector2 position = (!_controll.GetAttackButtonDown() ? bodyRb.position : (Vector2)armRoot.position) + offset;
                rb.position = Vector2.Lerp(rb.position, position, 0.15f);
            }
        }

        StayArms();
    }
    #endregion

    #region WallSlide
    private void SetStateWallSlide() 
    {
        bodyRb.velocity = Vector2.zero;
        _currentState = States.WallSlide;
        slideParticles.transform.position = bodyRb.transform.position;
        slideParticles.Play();
    }

    private void WallSlideUpdate() 
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(bodyRb.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(bodyRb.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitDown = Physics2D.Raycast(bodyRb.position, Vector2.down, DataGameMain.Default.personStandHeight, LayerMask.GetMask("Ground"));
        if (!hitLeft && !hitRight)
        {
            SwitchState(States.Fall);
            slideParticles.Stop();
            return;
        }
        if (hitDown)
        {
            SwitchState(States.Stay);
            slideParticles.Stop();
            return;
        }

        RaycastHit2D hit = hitLeft ? hitLeft : hitRight;

        bodyRb.drag = 2f;
        bodyRb.gravityScale = 0.1f;
        bodyRb.velocity = Vector2.up * bodyRb.velocity.y;
        bodyRb.rotation = Mathf.Lerp(bodyRb.rotation, 0f, 0.15f);

        slideParticles.transform.position = new Vector2(hit.point.x, bodyRb.position.y);

        Vector2 position;   
        List<Rigidbody2D> rbs = new List<Rigidbody2D> { frontLegRb, backLegRb, backArmRb};
        foreach (Rigidbody2D rb in rbs)
        {
            _filler.PushMotion(rb, PointerMotion.None);
            position.x = hit.point.x;
            if (rb == frontLegRb || rb == backLegRb)
                position.y = bodyRb.position.y - 1.5f;
            else
                position.y = bodyRb.position.y + 1.5f;
            rb.position = Vector2.Lerp(rb.position, position, 0.15f);
        }
        Vector2 offset = offsets[frontArmRb].bodyOffset;
        offset.x *= _direction.Direction;
        position = bodyRb.position + offset;
        position.x += (Mathf.PerlinNoise(_trueTime, _randSeed + rbs.IndexOf(frontArmRb)) - 0.5f) * offsets[frontArmRb].noiseScale;
        position.y += (Mathf.PerlinNoise(_randSeed + rbs.IndexOf(frontArmRb), _trueTime) - 0.5f) * offsets[frontArmRb].noiseScale;
        frontArmRb.position = Vector2.Lerp(frontArmRb.position, position, Time.fixedDeltaTime * offsets[frontArmRb].transitionSpeed * (bodyRb.velocity.x + 1));

        if (_controll.GetJump())
        {
            bodyRb.AddForce(new Vector2(hitLeft ? 1 : -1, 1) * jumpForce * bodyRb.mass, ForceMode2D.Impulse);
            SwitchState(States.Fall);
            return;
        }
    }

    #endregion
}