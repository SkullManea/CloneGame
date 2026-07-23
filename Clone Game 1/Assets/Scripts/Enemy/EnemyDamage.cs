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
            playerController.KBCounter = playerController.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerController.KnockFromRight = true;
            }

            playerController.KBCounter = playerController.KBTotalTime;
            if (collision.transform.position.x >= transform.position.x)
            {
                playerController.KnockFromRight = false;
            }

            playerHealth.TakeDamage(damage);
        }
    }

}
