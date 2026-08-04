using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

}
