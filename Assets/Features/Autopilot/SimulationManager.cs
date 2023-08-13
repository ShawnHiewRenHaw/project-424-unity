using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perrinn424.AutopilotSystem;
using System.Net.Sockets;
using System.Text;
using System;

public class SimulationManager : MonoBehaviour
{
    public Autopilot autopilot;
    private int lapCount = 0;
    private string[] assetPaths = {"GeneticAssets/asset1", "GeneticAssets/asset2"};
    private int currentAssetIndex = 0;
    private LapTimesWrapper lapData;

    void Start()
    {
        autopilot.enabled = true;
        LoadLap(assetPaths[currentAssetIndex]);
        // Initialize the LapTimesWrapper with test data
        lapData = new LapTimesWrapper();
    }

    void Update()
{
    if (autopilot.HasCompletedLap)
    {
        Debug.Log("Lap times before sendLapTimes method call: " + string.Join(", ", lapData.lapTimes));
        Debug.Log("Update method called");
        sendLapTimes();
        float finalLapTime = autopilot.GetFinalLapTime();
        Debug.Log("Final lap time: " + finalLapTime);

        lapCount++;
        Debug.Log("Lap completed. Total laps: " + lapCount);

        if (lapCount > 1)
        {
                
                // Load the next asset file
                currentAssetIndex = (currentAssetIndex + 1) % assetPaths.Length;
            RecordedLap newLap = Resources.Load<RecordedLap>(assetPaths[currentAssetIndex]);
            autopilot.recordedLap = newLap;

            if (newLap != null)
            {
                Debug.Log("Loaded asset: " + assetPaths[currentAssetIndex]);
            }
            else
            {
                Debug.LogError("Failed to load asset: " + assetPaths[currentAssetIndex]);
            }

            // Reset lap count
            lapCount = 0;
            Debug.Log("Starting a new lap with asset: " + assetPaths[currentAssetIndex]);
        }

        // Start a new lap
        autopilot.StartNewLap();
    }
}


    [Serializable]
    public class LapTimesWrapper
    {
       [SerializeField] public float[] lapTimes = { 3.32342f, 3.3f, 4.4f };
    }
    private void LoadLap(string assetPath)
    {
        RecordedLap newLap = Resources.Load<RecordedLap>(assetPath);
        autopilot.recordedLap = newLap;

        if (newLap != null)
        {
            Debug.Log("Loaded asset: " + assetPath);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + assetPath);
        }
    }

    public void sendLapTimes()
    {
        try
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect("127.0.0.1", 65432);
                using (NetworkStream stream = client.GetStream())
                {
                    string jsonData = JsonUtility.ToJson(lapData);
                    Debug.Log("Serialized lap data: " + jsonData);
                    jsonData += "<EOF>";
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);

                    stream.Write(data, 0, data.Length);

                    // Read server response
                    byte[] responseData = new byte[256];
                    int bytes = stream.Read(responseData, 0, responseData.Length);
                    string response = Encoding.UTF8.GetString(responseData, 0, bytes);
                    Debug.Log("Server response: " + response);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error while sending lap times: " + e.Message);
        }
    }




}

