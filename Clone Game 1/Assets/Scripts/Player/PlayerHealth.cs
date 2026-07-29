using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public TextMeshProUGUI healthText;


    void Start()
    {
        health = maxHealth;
        healthText.text = "HP: " + health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "HP: " + health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
