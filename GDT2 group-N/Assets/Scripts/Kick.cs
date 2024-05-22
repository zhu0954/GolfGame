using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kick : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform ball;
    [SerializeField] private LineRenderer line;
    [SerializeField] private float minKickAngle = -45.0f;
    [SerializeField] private float maxKickAngle = 45.0f;
    [SerializeField] private float kickImpulse = 100.0f;
    [SerializeField] private float angleSpeed = 10f;
    private GameManager gameManager;

    private Vector3 direction;
    private float horizontalAngle = 0;
    private float verticalAngle = 0;

 

    private enum KickState
    {
        notGrabbing,
        Grabbing,
    }

    private KickState kickState;
    private PlayerActions playerActions;
    private InputAction grabAction;
    private InputAction up;
    private InputAction down;
    private InputAction left;
    private InputAction right;
    public Movement movement;
    private Rigidbody ballRigidbody;

    private bool isKickPerformed;

    void Awake()
    {
        playerActions = new PlayerActions();
        grabAction = playerActions.Grab.Grabbing;
        up = playerActions.Grab.Up;
        down = playerActions.Grab.Down;
        left = playerActions.Grab.Left;
        right = playerActions.Grab.Right;


        ballRigidbody = ball.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        grabAction.Enable();
        up.Enable();
        down.Enable();
        left.Enable();
        right.Enable();
    }


    void OnDisable()
    {
        grabAction.Disable();
        up.Disable();
        down.Disable();
        left.Disable();
        right.Disable();
    }

    void Start()
    {
        kickState = KickState.notGrabbing;
        gameManager = GameObject.FindObjectOfType<GameManager>();

    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, ball.position);
        switch (kickState)
        {
            case KickState.notGrabbing:
                //movement.canMove = true;
                if (distance <= 1.8f && grabAction.ReadValue<float>() > 0.1f)
                {
                    kickState = KickState.Grabbing;
                    movement.canMove = false;
                    ballRigidbody.isKinematic = true;


                }
                break;
            case KickState.Grabbing:
                movement.canMove = false;
                if (grabAction.ReadValue<float>() < 0.1f)
                {
                    kickState = KickState.notGrabbing;
                    movement.canMove = true;
                    ballRigidbody.isKinematic = false;

                    if (isKickPerformed)
                    {
                        Vector3 kickDirection = HandleKickDirection();
                        ballRigidbody.AddForce(kickDirection * kickImpulse, ForceMode.Impulse);

                        line.positionCount = 0;

                        isKickPerformed = false;
                        gameManager.kicksAdd();

                    }
                    else
                    {
                        isKickPerformed = true;
                        direction = Quaternion.Euler(0, 0, 0) * player.forward;
                        horizontalAngle = 0;
                        verticalAngle = 0;
                    }
                }

                break;
        }
        if (kickState == KickState.Grabbing && isKickPerformed)
        {
            Vector3 kickDirection = HandleKickDirection();
            UpdateTrajectory(kickDirection);
        }


    }


    private Vector3 HandleKickDirection()
    {
        
        float upInput = up.ReadValue<float>();     
        float downInput = down.ReadValue<float>();  
        float leftInput = left.ReadValue<float>();  
        float rightInput = right.ReadValue<float>(); 

        
        horizontalAngle += (leftInput - rightInput) * Time.deltaTime * angleSpeed;
        verticalAngle += (upInput - downInput) * Time.deltaTime * angleSpeed;
        horizontalAngle = Mathf.Clamp(horizontalAngle, minKickAngle, maxKickAngle);
        verticalAngle = Mathf.Clamp(verticalAngle, minKickAngle, maxKickAngle);

        
        direction = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * player.forward;
        direction.Normalize();

        return direction;
    }




    private List<Vector3> trajectoryDots = new List<Vector3>();

    private void UpdateTrajectory(Vector3 kickDirection)
    {
        int numDots = 10; 
        float timeInterval = 0.05f;

        Vector3 currentPosition = ball.position;
        Vector3 currentVelocity = kickDirection * kickImpulse;

        trajectoryDots.Clear(); 

        for (int i = 0; i < numDots; i++)
        {
            currentPosition += currentVelocity * timeInterval;
            currentVelocity += Physics.gravity * timeInterval;

            trajectoryDots.Add(currentPosition);
        }

        line.positionCount = trajectoryDots.Count;
        line.SetPositions(trajectoryDots.ToArray());
    }

}