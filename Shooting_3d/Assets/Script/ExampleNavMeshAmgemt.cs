using UnityEngine;
using UnityEngine.AI;

public class ExampleNavMeshAmgemt : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Transform targetPosition;
    [SerializeField]
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("NO tienes un animator en " + gameObject.name, gameObject);
        }
        agent.SetDestination(targetPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Distancia de destino
        //agent.remainingDistance

        //Velociadad actual
        //agent.velocity

        animator.SetFloat("Walking", agent.velocity.magnitude);
    }
}
