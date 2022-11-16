using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimProjection : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLine;
    
    [Range(3,100)]
    [SerializeField] private int maxFrame = 60;

    [SerializeField] private float reachTargetTime = 5;

    public void ShowAimProjection(Vector3 startPoint, Vector3 startVelocity)
    {
        aimLine.enabled = true;
        float timeStep = reachTargetTime / maxFrame;

        Vector3[] lineRendererPoints = CalculateProjectionLine(startPoint, startVelocity, timeStep);

        aimLine.positionCount = maxFrame;
        aimLine.SetPositions(lineRendererPoints);
    }
    public void HideAimProjection()
    {
        aimLine.enabled = false;
    }
    private Vector3[] CalculateProjectionLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[maxFrame];
        lineRendererPoints[0] = startPoint;

        for (int i = 0; i < maxFrame; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startPoint + progressBeforeGravity - gravityOffset;
            lineRendererPoints[i] = newPosition;
        }
        return lineRendererPoints;
    }
}
