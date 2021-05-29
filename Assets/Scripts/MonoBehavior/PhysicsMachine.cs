using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class PhysicsMachine : MonoBehaviour
{
    enum States { Stay, Fall }

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

    [HideInInspector]
    public Dictionary<Rigidbody2D, PointerOffset> offsets;

    private int randSeed;
    private PointerFiller filler;
    private DirectionController direction;
    private IControll controll;
    private States _currentState = States.Stay;

    private Rigidbody2D bodyRb;
    private Rigidbody2D frontArmRb;
    private Rigidbody2D backArmRb;
    private Rigidbody2D frontLegRb;
    private Rigidbody2D backLegRb;

    private void Start()
    {
        controll = GetComponent<IControll>();
        filler = GetComponent<PointerFiller>();
        direction = GetComponent<DirectionController>();

        randSeed = UnityEngine.Random.Range(0, 100);

        bodyRb = filler.GetPointer(KinematicsPointerType.Body);
        frontArmRb = filler.GetPointer(KinematicsPointerType.FrontArm);
        backArmRb = filler.GetPointer(KinematicsPointerType.BackArm);
        frontLegRb = filler.GetPointer(KinematicsPointerType.FrontLeg);
        backLegRb = filler.GetPointer(KinematicsPointerType.BackLeg);

        offsets = new Dictionary<Rigidbody2D, PointerOffset> 
        { 
            { frontArmRb, new PointerOffset(new Vector2(1f, 2f), 2f)},
            { backArmRb, new PointerOffset(new Vector2(1f, 2f), 2f)},
            { frontLegRb, new PointerOffset(new Vector2(0f, -1f), 2f)},
            { backLegRb, new PointerOffset(new Vector2(0f, -1f), 2f)},
        };
    }

    private void FixedUpdate()
    {
        if (_currentState == States.Stay) StayUpdate();
        else if (_currentState == States.Fall) FallUpdate();
    }

    #region State Stay
    private void StayUpdate()
    {
        StayBody();
        StayLegs();
        StayArms();
    }

    private void StayBody()
    {
        if (filler.GetTween(bodyRb) == null)
        {
            bodyRb.gravityScale = 0f;
            bodyRb.drag = 3f;
            bodyRb.freezeRotation = true;

            RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
            if (hit && Vector3.Distance(bodyRb.position, hit.point) <= DataGameMain.Default.personStandHeight)
            {
                Vector3 position = bodyRb.position;
                if (controll.GetMoveDown())
                    position.y = Mathf.Lerp(position.y, hit.point.y + DataGameMain.Default.personStandHeight / 2f, Time.fixedDeltaTime * animationSpeed);
                else
                    position.y = Mathf.Lerp(position.y, hit.point.y + DataGameMain.Default.personStandHeight, Time.fixedDeltaTime * animationSpeed);

                position.y += (-Mathf.PerlinNoise(randSeed, Time.time * frequencySpeed)) * frequencyScale;

                bodyRb.velocity = bodyRb.velocity * Vector2.right;
                if (controll.GetMoveLeft()) bodyRb.velocity = Vector2.left * walkSpeed;
                else if (controll.GetMoveRight()) bodyRb.velocity = Vector2.right * walkSpeed;
                else position.x += (Mathf.PerlinNoise(Time.time * frequencySpeed, randSeed) - 0.5f) * frequencyScale;

                bodyRb.position = position;

                float targetRotation = (bodyRb.velocity.x / walkSpeed) * -45f;
                bodyRb.rotation = Mathf.Lerp(bodyRb.rotation, targetRotation, Time.fixedDeltaTime * animationSpeed);

                if (controll.GetJump())
                {
                    bodyRb.AddForce(Vector2.up * jumpForce * bodyRb.mass, ForceMode2D.Impulse);
                    _currentState = States.Fall;
                    return;
                }
            }
            else
            {
                _currentState = States.Fall;
                return;
            }
        }
    }

    private void StayLegs() 
    {
        if (!frontLegRb.isKinematic || !backLegRb.isKinematic) 
        {
            _currentState = States.Fall;
            return;
        }

        if (filler.GetTween(frontLegRb) == null && filler.GetTween(backLegRb) == null)
        {

            if (frontLegRb.position.x == backLegRb.position.x)
                filler.PushMotion(frontLegRb, PointerMotion.LegStep);

            Rigidbody2D leftLeg = frontLegRb.position.x < backLegRb.position.x ? frontLegRb : backLegRb;
            Rigidbody2D rightLeg = frontLegRb.position.x < backLegRb.position.x ? backLegRb : frontLegRb;

            if (bodyRb.position.x < leftLeg.position.x)
                filler.PushMotion(rightLeg, PointerMotion.LegStep);
            else if (bodyRb.position.x > rightLeg.position.x)
                filler.PushMotion(leftLeg, PointerMotion.LegStep);
            else if (Mathf.Abs(frontLegRb.position.x - bodyRb.position.x) > DataGameMain.Default.personStepLenght)
                filler.PushMotion(frontLegRb, PointerMotion.LegToNormalDistance);
            else if (Mathf.Abs(backLegRb.position.x - bodyRb.position.x) > DataGameMain.Default.personStepLenght)
                filler.PushMotion(backLegRb, PointerMotion.LegToNormalDistance);
        }

        foreach (Rigidbody2D rb in new Rigidbody2D[]{frontLegRb, backLegRb})
        {
            if (filler.GetTween(rb) == null)
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
            if (filler.GetTween(rb) == null && rb.isKinematic)
            {
                Vector2 offset = offsets[rb].bodyOffset;
                offset.x *= direction.Direction;
                Vector2 position = (!controll.GetAttackButtonDown(rbs.IndexOf(rb)) ? bodyRb.position : (Vector2)armRoot.position) + offset;
                position.x += (Mathf.PerlinNoise(Time.time, randSeed + rbs.IndexOf(rb)) - 0.5f) * offsets[rb].noiseScale;
                position.y += (Mathf.PerlinNoise(randSeed + rbs.IndexOf(rb), Time.time) - 0.5f) * offsets[rb].noiseScale;
                rb.position = Vector2.Lerp(rb.position, position, Time.fixedDeltaTime * offsets[rb].transitionSpeed * (bodyRb.velocity.x + 1));
            }
        }
    }
    #endregion

    #region State Fall
    private void FallUpdate() 
    {
        bodyRb.gravityScale = 1f;
        bodyRb.drag = 0f;
        bodyRb.freezeRotation = false;

        RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        if (hit && Vector3.Distance(bodyRb.position, hit.point) <= DataGameMain.Default.personStandHeight / 2f)
        {
            _currentState = States.Stay;
            return;
        }

        List<Rigidbody2D> rbs = new List<Rigidbody2D> { frontArmRb, backArmRb, frontLegRb, backLegRb };
        foreach (Rigidbody2D rb in rbs)
        {
            if (filler.GetTween(rb) == null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                Vector2 offset = offsets[rb].bodyOffset;
                offset.x *= direction.Direction;
                Vector2 position = (!controll.GetAttackButtonDown(rbs.IndexOf(rb)) ? bodyRb.position : (Vector2)armRoot.position) + offset;
                rb.position = Vector2.Lerp(rb.position, position, 0.15f);
            }
        }
    }
    #endregion
}
