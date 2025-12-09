using UnityEngine;

public class MachinePoint : MonoBehaviour
{
    [Header("Tiempo que tarda esta máquina en usarse")]
    public float tiempoDeUso = 5f;
    public bool isOccupied = false;

    // ⭐ NUEVA VARIABLE PARA EL SISTEMA ECONÓMICO ⭐
    [Header("Precio del Servicio")]
    public float servicePrice = 100f;
}