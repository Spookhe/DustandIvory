using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Poacher : MonoBehaviour
{
    [Header("Stats")]
    public float health = 50f;
    public float attackRange = 2f;
    public float walkSpeed = 2f;

    [Header("References")]
    public Animator anim;

    private NavMeshAgent agent;
    private Transform targetAnimal;
    private bool knockedOut = false;

    private Coroutine recoveryCoroutine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
        agent.speed = walkSpeed;

        // Register poacher for mission counters
        MissionController.Instance?.RegisterPoacher();
    }

    void Start()
    {
        FindNextAnimal();
    }

    void Update()
    {
        if (knockedOut) return;

        // If current target is gone, find the next closest animal
        if (targetAnimal == null)
        {
            FindNextAnimal();

            if (targetAnimal == null)
            {
                // No animals left, stop moving/attacking
                if (agent.isOnNavMesh)
                    agent.isStopped = true;

                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                return;
            }
        }

        if (!agent.isOnNavMesh) return;

        float dist = Vector3.Distance(transform.position, targetAnimal.position);

        if (dist <= attackRange)
        {
            agent.isStopped = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);

            Animal a = targetAnimal.GetComponent<Animal>();
            if (a != null)
                a.StartPoaching();
            else
                targetAnimal = null; // Animal destroyed, find next
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetAnimal.position);

            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
    }

    private void FindNextAnimal()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");

        if (animals.Length == 0)
        {
            targetAnimal = null;
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject a in animals)
        {
            float dist = Vector3.Distance(transform.position, a.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = a.transform;
            }
        }

        targetAnimal = closest;
    }

    public void TakeTranquilizer(float amount)
    {
        if (knockedOut) return;

        health -= amount;
        if (health <= 0)
            KnockOut();
    }

    private void KnockOut()
    {
        knockedOut = true;

        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        gameObject.tag = "KnockedOut";

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isKnockedOut", true);

        // Start automatic recovery
        recoveryCoroutine = StartCoroutine(AutoRecover());
    }

    private IEnumerator AutoRecover()
    {
        float timer = 0f;
        float recoveryTime = 5f;

        while (timer < recoveryTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Recover();
    }

    public void Recover()
    {
        if (!knockedOut) return;

        knockedOut = false;
        gameObject.tag = "Poacher";

        if (agent != null && !agent.enabled)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        if (anim != null)
        {
            anim.SetBool("isKnockedOut", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }

        FindNextAnimal();
    }

    public void Arrest()
    {
        if (!knockedOut) return;

        // Stop automatic recovery if player arrests
        if (recoveryCoroutine != null)
            StopCoroutine(recoveryCoroutine);

        // Update mission counters and award money
        MissionController.Instance?.PoacherArrested();

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tranquilizer"))
        {
            // Knock out instantly
            TakeTranquilizer(health);
            Destroy(other.gameObject);
        }
    }
}