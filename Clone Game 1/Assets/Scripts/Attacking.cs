using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attacking : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;

    public int attackDamage;

    public float cooldownTime = .5f;
    public float cooldownTimer = 0f;

    private void Update()
    {
      
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);       
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");
         if (cooldownTimer <= 0)
        {
            {
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                foreach (var enemy in enemiesInRange)
                {
                    //enemy.GetComponent<HealthManager>().TakeDamage(attackDamage);
                }

                cooldownTimer = cooldownTime; //resets timer
            }  
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
