using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Poacher : MonoBehaviour
{
    [Header("Stun System")]
    public int maxHits = 1;
    private int currentHits = 0;

    [Header("Stats")]
    public float attackRange = 2f;
    public float walkSpeed = 2f;

    [Header("References")]
    public Animator anim;

    private NavMeshAgent agent;
    private Transform targetAnimal;
    private bool knockedOut = false;
    private Coroutine recoveryCoroutine;

    [Header("UI / Indicators")]
    public GameObject healthBarPrefab;
    public GameObject stunIndicatorPrefab;

    private WorldHealthBar currentHealthBar;
    private StunIndicator currentStunIndicator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
        agent.speed = walkSpeed;

        MissionController.Instance?.RegisterPoacher();
    }

    void Start()
    {
        FindNextAnimal();

        // HEALTH BAR
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab);
            currentHealthBar = hb.GetComponent<WorldHealthBar>();

            if (currentHealthBar != null)
            {
                currentHealthBar.target = this.transform;

                // CREATE SEGMENTS
                currentHealthBar.CreateSegments(maxHits);

                // SET FULL HEALTH
                currentHealthBar.SetHealth(maxHits);
            }
        }

        // STUN INDICATOR
        if (stunIndicatorPrefab != null)
        {
            GameObject si = Instantiate(stunIndicatorPrefab);
            currentStunIndicator = si.GetComponent<StunIndicator>();

            if (currentStunIndicator != null)
            {
                currentStunIndicator.target = this.transform;
                currentStunIndicator.SetStunned(false);
            }
        }
    }

    void Update()
    {
        if (knockedOut) return;

        if (targetAnimal == null)
        {
            FindNextAnimal();
            if (targetAnimal == null)
            {
                if (agent.isOnNavMesh) agent.isStopped = true;
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
                targetAnimal = null;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetAnimal.position);

            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
    }

    public void TakeTranquilizer(float amount)
    {
        if (knockedOut) return;

        currentHits++;

        int remaining = maxHits - currentHits;

        if (currentHealthBar != null)
            currentHealthBar.SetHealth(remaining);

        if (currentHits >= maxHits)
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

        if (currentStunIndicator != null)
            currentStunIndicator.SetStunned(true);

        recoveryCoroutine = StartCoroutine(AutoRecover());
    }

    private IEnumerator AutoRecover()
    {
        float timer = 0f;
        float recoveryTime = 7f;

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

        anim.SetBool("isKnockedOut", false);
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);

        if (currentStunIndicator != null)
            currentStunIndicator.SetStunned(false);

        // RESET
        currentHits = 0;

        if (currentHealthBar != null)
            currentHealthBar.SetHealth(maxHits);

        FindNextAnimal();
    }

    public void Arrest()
    {
        if (!knockedOut) return;

        if (recoveryCoroutine != null)
            StopCoroutine(recoveryCoroutine);

        MissionController.Instance?.PoacherArrested();

        if (currentHealthBar != null)
            Destroy(currentHealthBar.gameObject);

        if (currentStunIndicator != null)
            Destroy(currentStunIndicator.gameObject);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tranquilizer"))
        {
            TakeTranquilizer(1f);
            Destroy(other.gameObject);
        }
    }

    private void FindNextAnimal()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");

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
}