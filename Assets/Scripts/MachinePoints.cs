using UnityEngine;

public class MachinePoint : MonoBehaviour
{
    [Header("Configuración de Estación")]
    public ServiceType serviceType; // Define qué servicio ofrece esta estación

    [Header("Tiempo que tarda esta máquina en usarse")]
    public float tiempoDeUso = 5f;
    public bool isOccupied = false;

    // ⭐ NUEVA VARIABLE PARA EL SISTEMA ECONÓMICO ⭐
    [Header("Precio del Servicio")]
    public float servicePrice = 100f;

    [Header("Punto de Interacción")]
    [Tooltip("Arrastra aquí un objeto hijo vacío que indique la posición y rotación exacta para sentarse.")]
    public Transform sitPoint;
}