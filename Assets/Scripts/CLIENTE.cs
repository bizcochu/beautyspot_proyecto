using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum ServiceType { CorteDePelo, LavadoDePelo, HacerPermanente }

[RequireComponent(typeof(NavMeshAgent))]
public class Client : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    [HideInInspector] public Transform doorWaypoint;
    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public Transform lastMachineUsed;

    public event Action<Client> OnArrived;

    public MachinePoint targetMachine;
    public Transform exitPoint;

    
    public ServiceType requestedService;

    private bool isUsingMachine = false;
    private bool hasArrivedNotified = false;
    private State state = State.Idle;
    private enum State { Searching, Moving, Using, Leaving, Idle }

    
    public string[] dialogosCorteDePelo = new string[] {
        "Quiero que me cortes el pelo, pero que no sea demasiado corto.",
        "Necesito un corte rápido, algo fresco pero simple.",
        "Mi pelo está sin forma, ¿podrías darle estilo?",
        "Quiero un cambio de look, puedes hacer lo que quieras.",
        "Solo quiero emparejar las puntas, gracias."
    };

    public string[] dialogosLavadoDePelo = new string[] {
        "He tenido un día terrible, solo quiero un buen lavado de pelo.",
        "Mi cabello está muy grasoso, necesito un lavado urgente.",
        "¿Pueden lavarme el pelo con agua tibia? Me relaja mucho.",
        "Quiero un lavado profundo, tengo muchos productos acumulados.",
        "Solo quiero que me laven el pelo suave, por favor."
    };

    public string[] dialogosHacerPermanente = new string[] {
        "Quiero hacerme una permanente suave, algo natural.",
        "¿Podrías hacerme una permanente? Quiero más volumen.",
        "Estoy lista para una permanente completa, ¡cámbiame el look!",
        "Quiero rizos definidos, ¿puedes hacerme una permanente?",
        "Hace tiempo que quiero una permanente, hoy me atreví."
    };

    void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        state = State.Idle;
        GenerateRandomRequest();
    }

    
    void GenerateRandomRequest()
    {
        int r = Random.Range(0, 3);
        if (r == 0) requestedService = ServiceType.CorteDePelo;
        else if (r == 1) requestedService = ServiceType.LavadoDePelo;
        else requestedService = ServiceType.HacerPermanente;
    }

    
    public string GetRequestDialogue()
    {
        switch (requestedService)
        {
            case ServiceType.CorteDePelo: return ObtenerDialogoAleatorio(dialogosCorteDePelo);
            case ServiceType.LavadoDePelo: return ObtenerDialogoAleatorio(dialogosLavadoDePelo);
            case ServiceType.HacerPermanente: return ObtenerDialogoAleatorio(dialogosHacerPermanente);
            default: return "Hola, necesito un servicio.";
        }
    }

    private string ObtenerDialogoAleatorio(string[] categoria)
    {
        if (categoria.Length == 0) return "...";
        return categoria[Random.Range(0, categoria.Length)];
    }

    void Update()
    {
        if (agent == null) return;

        if (currentTarget != null && HasReachedDestination())
        {
            if (!hasArrivedNotified)
            {
                hasArrivedNotified = true;
                OnArrived?.Invoke(this);
            }

            if (targetMachine != null && state != State.Using && !isUsingMachine)
            {
                targetMachine.isOccupied = true;
                EnterUseState();
            }
        }
        UpdateWalkingAnimation();
    }

    void UpdateWalkingAnimation()
    {
        if (animator == null || agent == null) return;
        bool isAgentMoving = agent.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isAgentMoving);
    }

    bool HasReachedDestination()
    {
        if (agent.pathPending) return false;
        float stoppingThreshold = agent.stoppingDistance + 0.2f;
        float distanceCheck = agent.remainingDistance;

        if (distanceCheck <= stoppingThreshold)
        {
            return (distanceCheck > 0f && distanceCheck <= stoppingThreshold) || !agent.hasPath;
        }
        return false;
    }

    public void MoveTo(Transform destination)
    {
        if (destination == null || agent == null) return;
        if (!agent.enabled) agent.enabled = true;

        currentTarget = destination;
        agent.SetDestination(destination.position);
        state = State.Moving;
        hasArrivedNotified = false;

        if (animator != null) animator.SetBool("isSitting", false);
    }

    public void AssignMachineTarget(Transform machineTransform)
    {
        if (machineTransform == null) return;

        MachinePoint mp = machineTransform.GetComponent<MachinePoint>();
        if (mp != null)
        {
            targetMachine = mp;
            mp.isOccupied = true;
        }
        else
        {
            targetMachine = null;
        }
        MoveTo(machineTransform);
    }

    void EnterUseState()
    {
        state = State.Using;
        isUsingMachine = true;

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isSitting", true);
        }

        lastMachineUsed = targetMachine != null ? targetMachine.transform : lastMachineUsed;
        StartCoroutine(UseMachine());
    }

    IEnumerator UseMachine()
    {
        if (targetMachine == null) yield break;
        float duration = Mathf.Max(0.01f, targetMachine.tiempoDeUso);
        yield return new WaitForSeconds(duration);
        FinishAndLeave();
    }

    public void FinishAndLeave()
    {
        if (targetMachine != null)
        {
            lastMachineUsed = targetMachine.transform;
            targetMachine.isOccupied = false;
            targetMachine = null;
        }

        isUsingMachine = false;
        state = State.Leaving;

        if (animator != null) animator.SetBool("isSitting", false);

        if (doorWaypoint != null) MoveTo(doorWaypoint);
        else Destroy(gameObject, 5f);
    }

    void OnDestroy()
    {
        if (targetMachine != null) targetMachine.isOccupied = false;
    }
}
