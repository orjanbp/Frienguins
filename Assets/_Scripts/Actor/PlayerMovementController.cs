using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed;
    public float moveSpeedSlow;
    public float gravity = 9.8f;
    public float wobbleAngle;
    public bool isMovingSlow;

    private bool isGrounded;
    private float vSpeed;
    private bool wobble;
    private Vector3 currentWobVel;
    private Vector3 lastRot;
    private Vector3 currentRot;

    private CharacterController controller;

    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset vertical speed if character is grounded
        if(controller.isGrounded)
            vSpeed = 0f;

        vSpeed -= gravity * Time.deltaTime;

        // Calculate movement direction based on camera position
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.y = vSpeed; 

        // Figure out whether the player is moving fast or slow
        float realMoveSpeed = isMovingSlow ? moveSpeedSlow : moveSpeed;

        // Add above calculations to CharacterController
        controller.Move(moveDirection * realMoveSpeed * Time.deltaTime);

        // Scale the x-axis to -1 if character moves to the right
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveDirection.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Check if character moves at all on the XZ plane
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            float wobSpeed = isMovingSlow ? 1.5f : 3f;
            float seesaw = Mathf.Clamp01(1f - Mathf.Abs(1f - 2f * (Time.time * wobSpeed % 1)));       // Seesaw linear curve going from 0 to 1 to 0
            float rotAngle = Mathf.Lerp(wobbleAngle, -wobbleAngle, seesaw);                 // Lerp between angle and minus angle with seesaw

            // Really hacky solution that combines Vector3.SmoothDamp & Quaternion.LookRotation. Don't judge.
            currentRot = new Vector3(rotAngle, 5, 0);
            currentRot = Vector3.SmoothDamp(lastRot, currentRot, ref currentWobVel, 0.05f);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, currentRot);
            lastRot = currentRot;
        }

        else
        {
            // Lerp back to zero position when movement is zero
            currentRot = Vector3.SmoothDamp(lastRot, new Vector3(0, 5, 0), ref currentWobVel, 0.05f);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, currentRot);
            lastRot = currentRot;
        }
    }
}
