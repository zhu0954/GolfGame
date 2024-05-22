using UnityEngine.InputSystem;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 walk;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    private bool isRunning = false;

    private PlayerActions playerActions;
    private InputAction walkInput;
    private InputAction runInput;

    public Rigidbody rb;

    public bool canMove;


    void Awake()
    {
        playerActions = new PlayerActions();
        walkInput = playerActions.Movement.Walk;
        runInput = playerActions.Movement.Run;
    }

    void OnEnable()
    {
        walkInput.Enable();
        runInput.Enable();
    }

    void OnDisable()
    {
        walkInput.Disable();
        runInput.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        walk = new Vector3(walkInput.ReadValue<Vector2>().x, 0, walkInput.ReadValue<Vector2>().y);

        if (runInput.ReadValue<float>() > 0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    void FixedUpdate()
    {
        float speed;
        if (canMove)
        {
            if (isRunning)
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }

            rb.MovePosition(transform.position + walk * speed * Time.fixedDeltaTime);

            if (walk != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(walk);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.fixedDeltaTime);
            }
        }
    }

}