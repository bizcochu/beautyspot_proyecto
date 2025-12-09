using UnityEngine;
using UnityEngine.AI;

public class ClickToMoveNoRotation : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Modelo hijo (el que debe rotar)")]
    public Transform childModel; // El modelo que debe rotar hacia donde camina
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (childModel != null)
        {
            animator = childModel.GetComponent<Animator>();
            if (animator == null)
                Debug.LogWarning("No se encontró Animator en childModel");
        }

        // Permitir que el agente rote automáticamente para simplificar la rotación
        agent.updateRotation = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        // Actualizar el parámetro Walking del Animator según la velocidad
        if (animator != null)
        {
            bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
            animator.SetBool("isWalking", isWalking);
        }
    }
}
