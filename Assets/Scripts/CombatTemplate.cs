using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatTemplate : MonoBehaviour
{
    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;
    public float attackRange = 1.5f;
    public float blockStrength = 5f;
    public float parryWindow = 0.5f;
    public bool isBlocking = false;
    public bool isParrying = false;
    public bool isStunned = false;
    public float currentHealth;
    public float maxHealth = 100f;

    protected Animator animator;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from the character.");
        }
    }

    protected virtual void Update()
    {
        // Handle common logic, e.g., for being stunned
        if (isStunned)
        {
            // Handle stun logic
        }
    }

    protected virtual void LightAttack()
    {
        // Trigger light attack animation
        animator.SetTrigger("LightAttack");
        Debug.Log("Light Attack triggered.");
    }

    protected virtual void HeavyAttack()
    {
        // Trigger heavy attack animation
        animator.SetTrigger("HeavyAttack");
        Debug.Log("Heavy Attack triggered.");
    }

    protected virtual void StartBlocking()
    {
        isBlocking = true;
        if (animator != null)
        {
            animator.SetBool("IsBlocking", true);
            Debug.Log("Set IsBlocking to true.");
        }
    }

    protected virtual void StopBlocking()
    {
        isBlocking = false;
        if (animator != null)
        {
            animator.SetBool("IsBlocking", false);
            Debug.Log("Set IsBlocking to false.");
        }
    }

    protected virtual void StartParry()
    {
        isParrying = true;
        // Trigger parry animation
        animator.SetTrigger("Parry");
        Debug.Log("Parry started.");
        Invoke("EndParry", parryWindow);
    }

    protected virtual void EndParry()
    {
        isParrying = false;
        Debug.Log("Parry ended.");
    }

    public virtual void TakeDamage(float damage)
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
            // Handle parry effect, e.g., stun the attacker
            damage = 0;
        }

        currentHealth -= damage;
        animator.SetTrigger("GetHit"); // Trigger get hit animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Trigger death animation
        animator.SetTrigger("Die");
        Debug.Log("Character died.");
        // Handle character death (disable movement, etc.)
    }

    public void Stun()
    {
        isStunned = true;
        // Trigger stun animation
        animator.SetTrigger("Stun");
        Debug.Log("Character stunned.");
        // Start coroutine to end stun after some time
        StartCoroutine(EndStun());
    }

    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(2f); // Example stun duration
        isStunned = false;
        Debug.Log("Stun ended.");
    }
}
