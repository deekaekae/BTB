using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float health = 100f;

    private Animator animator;
    private Transform player;
    private float nextAttackTime = 0f;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time >= nextAttackTime)
        {
            PerformLightAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void PerformLightAttack()
    {
        animator.SetTrigger("LightAttack");
        isAttacking = true;
    }

    public void PerformHeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");
        isAttacking = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("GetHit");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        // Additional death logic (e.g., disable enemy)
    }
}
