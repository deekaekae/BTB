using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float attackDuration = 1f;
    public float stunDuration = 2f;

    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;
    public float health = 100f;

    private NavMeshAgent agent;
    private Transform player;
    private float nextAttackTime = 0f;
    private float timer;
    private Animator animator;
    private bool isAttacking = false;
    private bool isStunned = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing from the enemy.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component missing from the enemy.");
        }

        timer = wanderTimer;
    }

    void Update()
    {
        if (isDead) return;

        timer += Time.deltaTime;

        if (!isAttacking && !isStunned)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
            }
            else
            {
                Wander();
            }
        }

        UpdateAnimation();
    }

    private void Wander()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        agent.speed = patrolSpeed;
        agent.isStopped = false;
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        agent.isStopped = false;
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        if (Time.time >= nextAttackTime)
        {
            // Check for parry state
            PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
            if (playerCombat != null && playerCombat.IsParrying())
            {
                StartCoroutine(Stun(stunDuration));
            }
            else
            {
                PerformLightAttack();
            }

            nextAttackTime = Time.time + attackCooldown;
        }

        // Start a coroutine to handle the end of the attack
        StartCoroutine(EndAttackAfterDuration(attackDuration));
    }

    private IEnumerator EndAttackAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
        agent.isStopped = false;
        Debug.Log("Attack ended. Resuming chase.");
        ChasePlayer();
    }

    private IEnumerator Stun(float duration)
    {
        isStunned = true;
        agent.isStopped = true;
        animator.SetTrigger("Stunned");
        Debug.Log("Enemy is stunned.");
        yield return new WaitForSeconds(duration);
        isStunned = false;
        agent.isStopped = false;
        Debug.Log("Stun ended. Resuming chase.");
        ChasePlayer();
    }

    public void PerformLightAttack()
    {
        animator.SetTrigger("LightAttack");
        Debug.Log("Light attack triggered.");
        DealDamage(lightAttackDamage);
    }

    public void PerformHeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");
        Debug.Log("Heavy attack triggered.");
        DealDamage(heavyAttackDamage);
    }

    private void DealDamage(float damage)
    {
        // Assuming the player has a PlayerHealth component
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Dealt " + damage + " damage to player.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("GetHit");
        Debug.Log("Enemy took damage: " + damage);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Immediately stop the agent and reset animation parameters
        agent.isStopped = true;
        agent.speed = 0;

        // Reset animator parameters
        animator.SetBool("isMoving", false);
        animator.SetBool("isRoaming", false);

        Debug.Log("Enemy died.");

        // Wait for the death animation to finish before disabling components
        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimationLength);

        // Disable enemy components to ensure it's properly "dead"
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        this.enabled = false; // Disable the script to stop further updates

        // Optionally: Destroy the enemy after a shorter delay
        Destroy(gameObject, 2f); // Adjust the delay as needed
    }

    private void UpdateAnimation()
    {
        if (isDead) return;

        Vector3 velocity = agent.velocity;
        bool isMoving = velocity.magnitude > 0.1f;

        animator.SetBool("isMoving", isMoving);

        if (agent.speed == patrolSpeed)
        {
            animator.SetBool("isRoaming", true);
        }
        else
        {
            animator.SetBool("isRoaming", false);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
