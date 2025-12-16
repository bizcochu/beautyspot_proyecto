using UnityEngine;
using UnityEngine.AI;

public class ClickToMoveNoRotation : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Modelo hijo (el que rota visualmente)")]
    public Transform childModel;

    private Animator animator;

    [Header("Rotación")]
    public float rotationSpeed = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 🔴 CLAVE: el agente NO rota el root
        agent.updateRotation = false;

        if (childModel != null)
        {
            animator = childModel.GetComponent<Animator>();
            if (animator == null)
                Debug.LogWarning("No se encontró Animator en childModel");
        }
    }

    void Update()
    {
        HandleClickMovement();
        UpdateRotation();
        UpdateAnimation();
    }

    void HandleClickMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    void UpdateRotation()
    {
        if (childModel == null) return;

        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            Vector3 direction = velocity.normalized;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            childModel.rotation = Quaternion.Slerp(
                childModel.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        bool isWalking = agent.velocity.sqrMagnitude > 0.05f;
        animator.SetBool("isWalking", isWalking);
    }
}
