using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class PhysicsMachine : MonoBehaviour
{
    enum States { Stay, Fall }

    [Header("Animation")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float frequencyScale = 1f;
    [SerializeField] private float frequencySpeed = 1f;

    [Header("Motion")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Arms")]
    [SerializeField] private Vector2 armsOffset;

    private int randSeed;
    private PointerFiller filler;
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

        randSeed = UnityEngine.Random.Range(0, 100);

        bodyRb = filler.GetPointer(KinematicsPointerType.Body);
        frontArmRb = filler.GetPointer(KinematicsPointerType.FrontArm);
        backArmRb = filler.GetPointer(KinematicsPointerType.BackArm);
        frontLegRb = filler.GetPointer(KinematicsPointerType.FrontLeg);
        backLegRb = filler.GetPointer(KinematicsPointerType.BackLeg);
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
    }

    private void StayArms() 
    {
        List<Rigidbody2D> rbs = new List<Rigidbody2D>{ frontArmRb, backArmRb };
        foreach (Rigidbody2D rb in rbs) 
        {
            if (filler.GetTween(rb) == null)
            {
                Vector2 position = bodyRb.position + armsOffset;
                position.x += Mathf.PerlinNoise(Time.time, randSeed + rbs.IndexOf(rb)) - 0.5f;
                position.x += Mathf.PerlinNoise(randSeed + rbs.IndexOf(rb), Time.time) - 0.5f;
                rb.position = Vector2.Lerp(rb.position, position, Time.fixedDeltaTime);
            }
        }
    }
    #endregion

    #region State Fall
    private void FallUpdate() 
    {
        bodyRb.gravityScale = 1f;

        RaycastHit2D hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        if (hit && Vector3.Distance(bodyRb.position, hit.point) <= DataGameMain.Default.personStandHeight / 2f)
        {
            _currentState = States.Stay;
            return;
        }
    }
    #endregion
}
