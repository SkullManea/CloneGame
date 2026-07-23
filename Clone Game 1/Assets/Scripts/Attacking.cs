using Unity.VisualScripting;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //enemyscript enemy = other.GetComponent<enemyscript>();
            //enemy.TakeDamage(damage); Take Damage assigned in enemy script

            //TakeDamage(int damage) health -=damage
        }
    }
}
