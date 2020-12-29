using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 1f;
    public float MaxSpeed = 1f;
    public float AimSpeed = 5f;
    public float Slowdown = 1f;
    public float JumpPower = 1f;
    public float GroundedThreshold = 0.25f;
    public float GroundedAngularDrag = 5f;
    public AimControl AimController;

    public float ResetPower = 1f;
    public Transform DownforceObject;

    public int GroundLayerIndex = 8;

    public Rigidbody RBody;
    
    private bool isInAir = false;
    private float resetTimer = 1f;

    void Start()
    {
        RBody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        // Get inputs
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        // Double the influence if a single key, as multiple keys goes really fast
        float singleKeyMult = 1.0f;
        if (hor == 0.0f || vert == 0.0f)
        {
            singleKeyMult = 2.0f;
        }

        // Delay the reset if there is any movement input from the player
        if (hor != 0.0f || vert != 0.0f)
        {
            resetTimer = 1f;
        }

        // Move the character
        RBody.AddForce(new Vector3(hor * singleKeyMult, 0.0f, vert * singleKeyMult) * Speed);

        // Limit the speed on X and Z
        if (RBody.velocity.magnitude > MaxSpeed)
        {
            float oldYVal = RBody.velocity.y;
            RBody.velocity = new Vector3(RBody.velocity.x, 0f, RBody.velocity.z);
            RBody.velocity = Vector3.ClampMagnitude(RBody.velocity, MaxSpeed);
            RBody.velocity = new Vector3(RBody.velocity.x, oldYVal, RBody.velocity.z);            
        }

        // Jump
        if (Input.GetAxis("Jump") > 0.0f && !isInAir)
        {
            //RBody.AddRelativeForce(new Vector3(0.0f, JumpPower, 0.0f), ForceMode.Impulse);
            RBody.velocity = RBody.velocity + new Vector3(0f, JumpPower, 0f);
        }

        // Check for if it is grounded
        if (IsGrounded())
        {
            isInAir = false;
            RBody.angularDrag = GroundedAngularDrag;
        }
        else
        {
            isInAir = true;
            RBody.angularDrag = 0f;
            resetTimer = 1f;
        }

        // Decrement the reset timer - if it reaches 0, force the character to right itself
        resetTimer -= Time.deltaTime;

        if (resetTimer <= 0f)
        {
            // Weeble wobble to stand up
            RBody.AddForceAtPosition(Vector3.down * ResetPower, DownforceObject.position);

            // Rotate to look at the reticle
            Vector3 direction = AimController.Reticle.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            //Quaternion toRotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction);
            Vector3 intermediateRotation = Quaternion.Lerp(transform.rotation, toRotation, AimSpeed * Time.deltaTime).eulerAngles;
            //intermediateRotation.x = transform.rotation.x;
            //intermediateRotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Euler(intermediateRotation);
        }

        /*
        if (!IsGrounded())
        {
            // Rotate to look at the reticle
            Vector3 direction = AimController.Reticle.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            //Quaternion toRotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction);
            Vector3 intermediateRotation = Quaternion.Lerp(transform.rotation, toRotation, AimSpeed * Time.deltaTime).eulerAngles;
            //intermediateRotation.x = transform.rotation.x;
            //intermediateRotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Euler(intermediateRotation);
        }
        */
    }

    public bool IsAtRest()
    {
        bool resting = false;

        if (resetTimer < 0f)
        {
            resting = true;
        }

        return resting;
    }

    public bool IsGrounded()
    {
        bool isGrounded = false;

        int layerMask = 1 << GroundLayerIndex;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.distance < GroundedThreshold)
            {
                isGrounded = true;
            }
        }

        return isGrounded;
    }
}
