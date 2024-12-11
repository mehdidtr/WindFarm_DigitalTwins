using System.Collections;
using UnityEngine;

public class WindmillManager : MonoBehaviour
{
    public string turbineID; // Manually set the ID in the Inspector
    public TurbineDataContainer turbineDataContainer; // Drag and drop the TurbineDataContainer ScriptableObject
    public Transform windmillBlades; // The windmill blades to rotate
    public Vector3 rotationAxis = Vector3.forward; // Axis of rotation (default is Z-axis)

    private TurbineData turbineData; // Holds the data for the specific turbine
    private int currentIntervalIndex = 0; // Tracks the current time interval

    private void Start()
    {
        // Fetch data for the turbineID from the container
        turbineData = turbineDataContainer.GetTurbineDataByID(turbineID);

        if (turbineData != null)
        {
            Debug.Log($"Windmill {turbineData.turbineID} initialized with {turbineData.timeIntervals.Length} data entries.");
            StartCoroutine(RotateWindmill());
        }
        else
        {
            Debug.LogError($"No TurbineData found for ID: {turbineID}");
        }
    }

    private IEnumerator RotateWindmill()
    {
        while (true)
        {
            if (turbineData == null || turbineData.rotorSpeeds.Length == 0)
            {
                Debug.LogError("No rotor speed data available.");
                yield break;
            }

            // Get the current rotor speed and interval duration
            float rotorSpeed = turbineData.rotorSpeeds[currentIntervalIndex]; // In RPM
            string timeInterval = turbineData.timeIntervals[currentIntervalIndex];
            float duration = ParseTimeIntervalToSeconds(timeInterval);

            // Log the current rotor speed (RPM)
            Debug.Log($"Current RPM for Turbine {turbineID}: {rotorSpeed}");

            // Convert rotor speed from RPM to degrees per second
            float rotationSpeed = rotorSpeed * 6f; // 1 RPM = 6 degrees per second

            float elapsedTime = 0f;

            // Rotate the windmill blades for the duration of the current interval
            while (elapsedTime < duration)
            {
                float deltaRotation = rotationSpeed * Time.deltaTime;
                windmillBlades.Rotate(rotationAxis, deltaRotation); // Rotate around the chosen axis
                elapsedTime += Time.deltaTime;

                // Check for time interval change
                CheckForTimeIntervalChange();
                yield return null;
            }

            // Move to the next time interval
            currentIntervalIndex = (currentIntervalIndex + 1) % turbineData.rotorSpeeds.Length;
        }
    }

    // Method to parse a time interval like "00:00-00:10" into its duration in seconds
    private float ParseTimeIntervalToSeconds(string timeInterval)
    {
        string[] parts = timeInterval.Split('-');
        if (parts.Length != 2)
        {
            Debug.LogWarning($"Invalid time interval format: {timeInterval}. Defaulting to 1 second.");
            return 1f; // Default to 1 second if parsing fails
        }

        float startSeconds = TimeToSeconds(parts[0]);
        float endSeconds = TimeToSeconds(parts[1]);

        if (endSeconds < startSeconds)
        {
            Debug.LogWarning("End time is earlier than start time. Using default duration of 1 second.");
            return 1f;
        }

        return endSeconds - startSeconds;
    }

    // Converts a time string in "HH:mm:ss" or "mm:ss" format to seconds
    private float TimeToSeconds(string timeString)
    {
        string[] timeParts = timeString.Split(':');
        int seconds = 0;

        if (timeParts.Length == 3)
        {
            // HH:mm:ss format
            seconds += int.Parse(timeParts[0]) * 3600; // Hours to seconds
            seconds += int.Parse(timeParts[1]) * 60;   // Minutes to seconds
            seconds += int.Parse(timeParts[2]);       // Seconds
        }
        else if (timeParts.Length == 2)
        {
            // mm:ss format
            seconds += int.Parse(timeParts[0]) * 60;   // Minutes to seconds
            seconds += int.Parse(timeParts[1]);       // Seconds
        }

        return seconds;
    }

    // Method to handle user input or time interval changes
    private void CheckForTimeIntervalChange()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Example: Go to the next time interval
        {
            currentIntervalIndex = (currentIntervalIndex + 1) % turbineData.timeIntervals.Length;
            Debug.Log($"Time interval changed to {turbineData.timeIntervals[currentIntervalIndex]}");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // Example: Go to the previous time interval
        {
            currentIntervalIndex = (currentIntervalIndex - 1 + turbineData.timeIntervals.Length) % turbineData.timeIntervals.Length;
            Debug.Log($"Time interval changed to {turbineData.timeIntervals[currentIntervalIndex]}");
        }
    }
}