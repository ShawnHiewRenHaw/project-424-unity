using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    private Vector3 lastPosition;
    private float timeNotMoving = 0f;
    public float crashThreshold = 0.1f;  // Distance threshold
    public float timeThreshold = 2.0f;   // Time threshold in seconds

    // Event to notify when a crash is detected
    public delegate void CrashDetected();
    public event CrashDetected OnCrashDetected;

    private void Start()
    {
        lastPosition = transform.position;
        Debug.Log("CrashDetector initialized.");
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < crashThreshold)
        {
            timeNotMoving += Time.deltaTime;
            Debug.Log($"Car not moving. Time not moving: {timeNotMoving}");

            if (timeNotMoving >= timeThreshold)
            {
                // Trigger crash event
                OnCrashDetected?.Invoke();
                Debug.Log("Crash detected. Triggering event.");

                // Reset time counter
                timeNotMoving = 0f;
            }
        }
        else
        {
            // Reset time counter if the car is moving
            timeNotMoving = 0f;
        }

        // Update last position
        lastPosition = transform.position;
    }
}

