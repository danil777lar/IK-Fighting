using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMachine : MonoBehaviour
{
    enum States { Stay, Fall}

    [SerializeField] private Rigidbody2D bodyRb;


    [Header("Animation")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float frequencyScale = 1f;
    [SerializeField] private float frequencySpeed = 1f;

    [Header("Motion")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float jumpForce = 15f;

    private int randSeed;
    private PointerFiller filler;
    private IControll controll;
    private States _currentState = States.Stay;

    private void Start()
    {
        controll = GetComponent<IControll>();
        filler = GetComponent<PointerFiller>();

        randSeed = UnityEngine.Random.Range(0, 100);
    }

    private void FixedUpdate()
    {
        if (_currentState == States.Stay) StayUpdate();
        else if (_currentState == States.Fall) FallUpdate();
    }

    private void StayUpdate() 
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
}
