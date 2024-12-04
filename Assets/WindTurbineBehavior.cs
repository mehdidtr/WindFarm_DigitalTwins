using UnityEngine;
 
public class WindTurbineBehavior : MonoBehaviour
{
    public string turbineID;  // Identifiant unique de l'Ã©olienne
    public GameObject blades; // Objet reprÃ©sentant les pales
    private float windSpeed;
 
    void Update()
    {
        // Faire tourner les pales proportionnellement Ã  la vitesse du vent
        if (blades != null)
        {
            float rotationSpeed = windSpeed * 10f; // Ajustez le facteur pour une vitesse rÃ©aliste
            blades.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
 
    public void UpdateData(float newWindSpeed, float ambientTemperature, float rotorSpeed, float power)
    {
        windSpeed = newWindSpeed;
 
        // Vous pouvez afficher les autres donnÃ©es pour du dÃ©bogage ou d'autres comportements
        Debug.Log($"Turbine {turbineID} - WindSpeed: {windSpeed}, Temp: {ambientTemperature}, Rotor: {rotorSpeed}, Power: {power}");
    }
}