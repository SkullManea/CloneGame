using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;
    public int health;
    private TextMeshProUGUI healthText;

    void Awake()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        health = maxHealth;
        healthText.text = "HP: " + health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthText != null)
            healthText.text = "HP: " + health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
