using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            // If a point was found, set the agent's position to that point
            agent.Warp(hit.position);
        }

    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            if (agent.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
        }
    }
}