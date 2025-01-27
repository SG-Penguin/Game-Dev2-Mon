using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Vector2 moveInputVec = Vector2.zero;

    private CharacterController controller;
    private Vector3 velocity;
    public float maxSpeed = 10;
    public float acceleration = 2f;
    public float friction = 1.5f;
    public float gravity = 1f;
    public float jump_power = 21f;



    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInputVec.x, 0, moveInputVec.y);

        velocity += direction * acceleration;

        Vector2 xz_Vel = new Vector2(velocity.x, velocity.z);

        xz_Vel = Vector2.ClampMagnitude(xz_Vel, maxSpeed);

        if (moveInputVec == Vector2.zero)
        {
            xz_Vel = Vector2.MoveTowards(xz_Vel,Vector2.zero,friction);
        }

        velocity.y -= gravity;
        if(controller.isGrounded && velocity.y < -2f)
        {
            velocity.y = -2f;
        }

        velocity = new Vector3(xz_Vel.x, velocity.y, xz_Vel.y);

        controller.Move(velocity * Time.fixedDeltaTime);

        float target_angle = Mathf.Atan2(direction.x,direction.z) *
                               Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        Vector3 move_direcetion = Quaternion.Euler(0f, target_angle, 0f) *Vector3.forward;

        velocity += move_direcetion * acceleration;

    }


    public void CaptureMoveInput(InputAction.CallbackContext context)
    {
        moveInputVec = context.ReadValue<Vector2>();
    }

    public void CaptureJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            if (controller.isGrounded)
                velocity.y = jump_power;
    }
}
