using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawn;
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        Debug.Log("found");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathPoint"))
        {
            Debug.Log(collision.gameObject.name);

            transform.position = respawn.position;
            player.currentStamina = player.maxStamina;
            player.staminaBar.fillAmount = player.currentStamina / player.maxStamina;
        }
    }

}
