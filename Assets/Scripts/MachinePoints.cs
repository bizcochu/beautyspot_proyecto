using UnityEngine;
 public class MachinePoint : MonoBehaviour
  { 
    [Header("Configuración de Estación")]
    public ServiceType serviceType;
    [Header("Tiempo que tarda esta máquina en usarse")]
    public float tiempoDeUso = 5f;
    public bool isOccupied = false;
    [Header("Precio del Servicio")]
    public float servicePrice = 100f;
    [Header("Punto de Interacción")]
    [Tooltip("Objeto hijo donde el cliente se sentará.")]
    public Transform sitPoint;
    [Header("Partículas al usar estación")]
    [Tooltip("Arrastra aquí un Particle System (debes arrastrar el objeto que lo tiene).")]
    public ParticleSystem particles;
    }