using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WindFarmController : MonoBehaviour
{
    public string fileName = "data.csv";
    public List<GameObject> windTurbines = new List<GameObject>();
    private Dictionary<string, List<DataPoint>> turbineData = new Dictionary<string, List<DataPoint>>();

    void Start()
    {
        ReadCSV();  
    }

    void ReadCSV()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (File.Exists(filePath))
        {
            string[] dataLines = File.ReadAllLines(filePath);

            for (int i = 1; i < dataLines.Length; i++)
            {
                string[] splitData = dataLines[i].Split(',');

                try
                {
                    if (splitData.Length != 8)
                    {
                        Debug.LogWarning($"Format incorrect à la ligne {i + 1}: {dataLines[i]}");
                        continue;
                    }

                    string turbineID = splitData[0].Trim(); // ID de l'éolienne
                    string timeInterval = splitData[1].Trim(); // Intervalle de temps
                    string eventCode = splitData[2].Trim(); // Code d'événement
                    string eventDescription = splitData[3].Trim(); // Description de l'événement

                    double windSpeed = ParseDouble(splitData[4].Trim());
                    double ambientTemp = ParseDouble(splitData[5].Trim());
                    double rotorSpeed = ParseDouble(splitData[6].Trim());
                    double power = ParseDouble(splitData[7].Trim());

                    DataPoint dataPoint = new DataPoint(timeInterval, windSpeed, ambientTemp, rotorSpeed, power);

                    // Ajouter le DataPoint dans le dictionnaire par ID d'éolienne
                    if (!turbineData.ContainsKey(turbineID))
                    {
                        turbineData[turbineID] = new List<DataPoint>();
                    }
                    turbineData[turbineID].Add(dataPoint);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Erreur à la ligne {i + 1}: {dataLines[i]}. Exception: {ex.Message}");
                }
            }
        }
        else
        {
            Debug.LogError("Fichier CSV introuvable : " + filePath);
        }
    }

    double ParseDouble(string value)
    {
        double result;
        if (double.TryParse(value, out result))
        {
            return result;
        }
        else
        {
            Debug.LogError($"Impossible de convertir la valeur '{value}' en double.");
            return 0;
        }
    }
}

public class DataPoint
{
    public string timeInterval;
    public double windSpeed;
    public double ambientTemp;
    public double rotorSpeed;
    public double power;

    public DataPoint(string timeInterval, double windSpeed, double ambientTemp, double rotorSpeed, double power)
    {
        this.timeInterval = timeInterval;
        this.windSpeed = windSpeed;
        this.ambientTemp = ambientTemp;
        this.rotorSpeed = rotorSpeed;
        this.power = power;
    }
}
