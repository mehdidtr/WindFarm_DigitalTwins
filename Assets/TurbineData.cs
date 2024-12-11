using UnityEngine;

[System.Serializable]
public class TurbineData
{
    public string turbineID;
    public string[] timeIntervals;
    public int[] eventCodes;
    public string[] eventCodeDescriptions;
    public float[] windSpeeds;
    public float[] ambientTemperatures;
    public float[] rotorSpeeds;
    public float[] powers;
}