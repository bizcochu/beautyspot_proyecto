using System.Collections.Generic;
using UnityEngine;

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
        foreach (var station in allStations)
        {
            // Buscar una estación que coincida con el tipo de servicio y no esté ocupada
            if (station.serviceType == type && !station.isOccupied)
            {
                return station;
            }
        }
        return null;
    }
}