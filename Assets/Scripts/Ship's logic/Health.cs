using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int collisionDamage = 10;
    public float collisionVelocity = 10;
    public Slider healthBar;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.value = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("OnCollisionEnter2D = " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > collisionVelocity && currentHealth > 0)
        {
            float k = collision.relativeVelocity.magnitude / collisionVelocity;

            currentHealth -= collisionDamage * k;
            healthBar.value -= collisionDamage * k / maxHealth;
            if (currentHealth <= 0) GameController.Instance.Lose();
            //print("damage = " + collisionDamage * k / maxHealth);
        }
    }

}
