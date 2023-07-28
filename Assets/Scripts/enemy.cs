using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask playerLayer;
    public bool stunnable = true;
    public float stunTime = 1.5f;

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

    public IEnumerator StunBuffer()
    {
        stunnable = false;
        Debug.Log("Stun Resist");
        yield return new WaitForSeconds(stunTime);
        stunnable = true;
    }

    private void Die()
    {
        //Debug.Log(gameObject + " died");
    }
}
