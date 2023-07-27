using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask playerLayer;

    public int maxHealth = 100;
    int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, float KBForceX, float KBForceY)
    {
        currentHealth -= damage;
        rb.velocity = new Vector2(KBForceX, KBForceY);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Debug.Log(gameObject + " died");
    }
}
