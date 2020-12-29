using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLegsGrounder : MonoBehaviour
{
    public float UpdateInterval = 1f;
    public float StepSpeed = 5f;
    public float StepThreshold = 0.1f;
    private int stepNumber = 0;

    public Transform leftForelegMaster, rightForelegMaster, leftHindlegMaster, rightHindlegMaster;
    public Transform leftForelegTarget, rightForelegTarget, leftHindlegTarget, rightHindlegTarget;
    public Transform leftForelegFolded, rightForelegFolded, leftHindlegFolded, rightHindlegFolded;

    public PlayerMovement PlayerMovementController;

    private Vector3 leftForelegPos, rightForelegPos, leftHindlegPos, rightHindlegPos;
    private bool wasInAirPreviousFrame = false;

    private void Start()
    {
        InvokeRepeating("UpdateFootPositions", UpdateInterval, UpdateInterval);
    }

    private void UpdateFootPositions()
    {
        if (stepNumber == 0) leftForelegPos = leftForelegMaster.position;
        if (stepNumber == 1) rightForelegPos = rightForelegMaster.position;
        if (stepNumber == 2) leftHindlegPos = leftHindlegMaster.position;
        if (stepNumber == 3) rightHindlegPos = rightHindlegMaster.position;

        stepNumber += 1;
        if (stepNumber == 4) stepNumber = 0;
    }

    private void Update()
    {
        if (PlayerMovementController.IsAtRest())
        {
            if (wasInAirPreviousFrame)
            {
                leftForelegPos = leftForelegMaster.position;
                rightForelegPos = rightForelegMaster.position;
                leftHindlegPos = leftHindlegMaster.position;
                rightHindlegPos = rightHindlegMaster.position;
            }

            leftForelegTarget.transform.position = Vector3.Lerp(leftForelegTarget.transform.position, leftForelegPos, Time.deltaTime * StepSpeed);
            rightForelegTarget.transform.position = Vector3.Lerp(rightForelegTarget.transform.position, rightForelegPos, Time.deltaTime * StepSpeed);
            leftHindlegTarget.transform.position = Vector3.Lerp(leftHindlegTarget.transform.position, leftHindlegPos, Time.deltaTime * StepSpeed);
            rightHindlegTarget.transform.position = Vector3.Lerp(rightHindlegTarget.transform.position, rightHindlegPos, Time.deltaTime * StepSpeed);

            wasInAirPreviousFrame = false;
        }
        else
        {
            leftForelegTarget.transform.position = Vector3.Lerp(leftForelegTarget.transform.position, leftForelegFolded.transform.position, Time.deltaTime * StepSpeed * 5f);
            rightForelegTarget.transform.position = Vector3.Lerp(rightForelegTarget.transform.position, rightForelegFolded.transform.position, Time.deltaTime * StepSpeed * 5f);
            leftHindlegTarget.transform.position = Vector3.Lerp(leftHindlegTarget.transform.position, leftHindlegFolded.transform.position, Time.deltaTime * StepSpeed * 5f);
            rightHindlegTarget.transform.position = Vector3.Lerp(rightHindlegTarget.transform.position, rightHindlegFolded.transform.position, Time.deltaTime * StepSpeed * 5f);

            wasInAirPreviousFrame = true;
        }
        
    }
}
