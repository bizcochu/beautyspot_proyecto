using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Asegúrate de tener esto

public class StationManager : MonoBehaviour
{
    public static StationManager Instance;

    // Lista única de todas las estaciones en la escena
    public List<MachinePoint> allStations = new List<MachinePoint>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Auto-registrar todas las estaciones si la lista está vacía
        if (allStations.Count == 0)
        {
            allStations.AddRange(FindObjectsByType<MachinePoint>(FindObjectsSortMode.None));
        }
    }

    public MachinePoint GetAvailableStation(ServiceType type)
    {
        // 1. Crear una lista de estaciones disponibles
        List<MachinePoint> availableStations = new List<MachinePoint>();

        foreach (var station in allStations)
        {
            if (station.serviceType == type && !station.isOccupied)
            {
                availableStations.Add(station);
            }
        }

        // 2. Elegir una estación aleatoria de la lista de disponibles
        if (availableStations.Count > 0)
        {
            int randomIndex = Random.Range(0, availableStations.Count); // El límite superior es exclusivo
            return availableStations[randomIndex];
        }

        // Si no hay ninguna disponible, devuelve null
        return null;
    }
}