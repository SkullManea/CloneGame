using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collieded with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            Debug.Log(enemyHealth);

            if (enemyHealth != null)
            {
                Debug.Log("Found!");
                enemyHealth.TakeDamage(damage);
            }
            else
                Debug.Log("Not Found");
        }
    }
}
