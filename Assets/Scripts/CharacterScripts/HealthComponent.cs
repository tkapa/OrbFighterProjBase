using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{

    public float maximumHealth = 100.0f;
    public float currentHealth = 0.0f;

    // Use this for initialization
    void Start()
    {
        currentHealth = maximumHealth;
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
    }
}
