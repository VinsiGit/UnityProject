using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public AudioSource enemySound;

    private bool isCooldown = false;


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
        StartCoroutine(PlaySoundRandomly());

    }
    IEnumerator PlaySoundRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 6));
            enemySound.Play();
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCooldown)
        {
            StartCoroutine(StopAndCooldown());
        }
        if (player != null)
        {
            agent.SetDestination(player.position);
            if (agent.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
        }
    }
    IEnumerator StopAndCooldown()
    {
        isCooldown = true;
        agent.isStopped = true;
        agent.speed = 0f;
        agent.acceleration = 600f;

        // Get the Animator on the child GameObject named "body" and disable it
        Animator bodyAnimator = transform.Find("body").GetComponent<Animator>();
        if (bodyAnimator != null)
        {
            bodyAnimator.enabled = false;
        }

        yield return new WaitForSeconds(1.0f);
        agent.isStopped = false;
        agent.speed = 8f;
        agent.acceleration = 4f;

        // Enable the Animator again after the agent starts moving
        if (bodyAnimator != null)
        {
            bodyAnimator.enabled = true;
        }

        yield return new WaitForSeconds(1.0f); // 1.0s additional wait to complete 2s cooldown
        isCooldown = false;
    }
}