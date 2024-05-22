using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerActions playerActions;
    private InputAction walkInput;
    private InputAction runInput;
    private InputAction grabbing;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isGrabbing = false;
    private bool hasThrown = false;

    public Goal goal;
    public Movement movement;

    void Awake()
    {
        playerActions = new PlayerActions();
        walkInput = playerActions.Movement.Walk;
        runInput = playerActions.Movement.Run;
        grabbing = playerActions.Grab.Grabbing;
    }

    void OnEnable()
    {
        walkInput.Enable();
        runInput.Enable();
        grabbing.Enable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        walkInput.started += OnWalkStarted;
        walkInput.canceled += OnWalkCanceled;
        runInput.started += OnRunStarted;
        runInput.canceled += OnRunCanceled;
        grabbing.started += OnSpacebarStarted;
        grabbing.canceled += OnSpacebarCanceled;
    }

    void OnWalkStarted(InputAction.CallbackContext context)
    {
        DoWalk(true, context);
    }

    void OnWalkCanceled(InputAction.CallbackContext context)
    {
        DoWalk(false, context);
    }

    void OnRunStarted(InputAction.CallbackContext context)
    {
        DoRun(true, context);
    }

    void OnRunCanceled(InputAction.CallbackContext context)
    {
        DoRun(false, context);
    }

    void OnSpacebarStarted(InputAction.CallbackContext context)
    {
        if (!hasThrown)
        {
            animator.SetTrigger("Grab");
            isGrabbing = true;
        }
    }

    void OnSpacebarCanceled(InputAction.CallbackContext context)
    {
        isGrabbing = false;
        if (!hasThrown)
        {
            animator.SetTrigger("Throw");
            hasThrown = true;
        }
    }

    void DoWalk(bool value, InputAction.CallbackContext context)
    {
        isWalking = value;
        UpdateAnimation();
    }

    void DoRun(bool value, InputAction.CallbackContext context)
    {
        isRunning = value;
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsRunning", isRunning);

        if (isGrabbing)
        {
            animator.SetTrigger("Grab");
        }
        else
        {
            animator.ResetTrigger("Grab");
        }

        if (hasThrown)
        {
            animator.SetTrigger("Throw");
            hasThrown = false;
        }
    }

    void Update()
    {
        Vector3 currentVelocity = movement.rb.velocity;
        if (Mathf.Abs(currentVelocity.magnitude) < 0.1f)
        {
            animator.SetTrigger("Idle");
        }
        else if (isWalking)
        {
            animator.SetBool("IsWalking", true);
        }
        else if (isRunning)
        {
            animator.SetBool("IsRunning", true);
        }

        if (goal.scored)
        {
            animator.SetTrigger("Cheer");
            
        }
    }
    void OnDisable()
    {
        walkInput.Disable();
        runInput.Disable();
        grabbing.Disable();
    }
}