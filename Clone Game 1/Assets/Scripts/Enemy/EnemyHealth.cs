using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;
    public int health;
    private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI byeText;
    public GameObject sprite;
    public GameObject BText;

    void Awake()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        health = maxHealth;
        healthText.text = "HP: " + health;
        byeText.text = "Bye Bye";
        BText.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthText != null)
            healthText.text = "HP: " + health;

        if (health <= 0)
        {
            sprite.SetActive(false);
            StartCoroutine(ByeText());
        }
    }

    public IEnumerator ByeText()
    {
        BText.SetActive(true);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
