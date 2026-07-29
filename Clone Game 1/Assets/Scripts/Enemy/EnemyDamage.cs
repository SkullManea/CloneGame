using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth playerHealth;
    public PlayerController playerController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bool fromRight = collision.transform.position.x <= transform.position.x;
            // playerController.KnockBack(fromRight);

            playerHealth.TakeDamage(damage);
        }
    }

}
