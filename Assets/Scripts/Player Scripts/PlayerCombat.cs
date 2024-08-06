using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayers;   // Layer mask to specify what is considered an enemy

    public float attackCooldown = 1f;  // Cooldown between attacks
    private float nextAttackTime = 0f;
    public float criticalChance = 0.1f;
    public float temporaryDamageBoost = 0f;
    public float baseDamage = 10f;
    public float attackRange = 2f; // Range of the attack
    public float blockStrength = 5f; // Strength of the block

    private PlayerHealth playerHealth;
    private Animator animator;

    public bool isParrying = false;
    public bool isBlocking = false;
    public float parryWindow = 0.5f;  // Duration of the parry window

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component is missing on the player!");
        }
    }

    void Update()
    {
        HandleCombatInput();
    }

    private void HandleCombatInput()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))  // Light Attack (Default: left mouse button or Ctrl)
            {
                LightAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else if (Input.GetButtonDown("Fire2"))  // Heavy Attack (Default: right mouse button or Alt)
            {
                HeavyAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        if (Input.GetButtonDown("Block"))  // Block (Default: left shift)
        {
            StartBlocking();
        }
        else if (Input.GetButtonUp("Block"))  // Stop Block
        {
            StopBlocking();
        }

        if (Input.GetButtonDown("Parry"))  // Parry (Default: E)
        {
            StartParry();
        }
    }

    private void LightAttack()
    {
        animator.SetTrigger("LightAttack");
        Debug.Log("Player performed a light attack.");
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);  // Debug log to check if enemy is detected
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Debug.Log("Enemy health before damage: " + enemyAI.health);  // Log enemy health before damage
                enemyAI.TakeDamage(baseDamage + temporaryDamageBoost);
                Debug.Log("Enemy health after damage: " + enemyAI.health);  // Log enemy health after damage
            }
        }
    }

    private void HeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");
        Debug.Log("Player performed a heavy attack.");
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);  // Debug log to check if enemy is detected
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Debug.Log("Enemy health before damage: " + enemyAI.health);  // Log enemy health before damage
                enemyAI.TakeDamage((baseDamage * 2) + temporaryDamageBoost); // Assuming heavy attack does double damage
                Debug.Log("Enemy health after damage: " + enemyAI.health);  // Log enemy health after damage
            }
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        animator.SetBool("isBlocking", true);
        Debug.Log("Player started blocking.");
    }

    private void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("isBlocking", false);
        Debug.Log("Player stopped blocking.");
    }

    private void StartParry()
    {
        animator.SetTrigger("Parry");
        isParrying = true;
        Debug.Log("Player attempted a parry.");
        Invoke("EndParry", parryWindow);
    }

    private void EndParry()
    {
        isParrying = false;
        Debug.Log("Player ended parry.");
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    public void TakeDamage(float damage)
    {
        if (isBlocking)
        {
            damage -= blockStrength;
            if (damage < 0) damage = 0;
            Debug.Log("Blocked damage: " + damage);
        }
        else if (isParrying)
        {
            Debug.Log("Parried the attack!");
            // The logic for parrying the attack would be handled elsewhere
            damage = 0;
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void AddTemporaryDamageBoost(float amount, float duration)
    {
        temporaryDamageBoost += amount;
        Invoke("RemoveTemporaryDamageBoost", duration);
    }

    private void RemoveTemporaryDamageBoost()
    {
        temporaryDamageBoost = 0f;
    }
}
