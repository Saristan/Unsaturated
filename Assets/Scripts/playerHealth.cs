using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public int playerPhase;
    public float maxHealth = 100f;
    private float modMaxHealth;
    private float currentHealth;

    public Transform playerLight;
    public Transform playerDark;

    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        modMaxHealth = maxHealth;
        currentHealth = maxHealth;
        playerLight.localScale = new Vector3(1f, 1f, 1f);
        playerDark.localScale = new Vector3(0f, 0f, 0f);
        invincible = false;
    }

    public void TakeDamage(float damage)
    {
        if(!invincible)
        {
            StartCoroutine(DamageAnim(currentHealth, currentHealth - damage, maxHealth));
            StartCoroutine(damageBuffer());

            currentHealth -= damage;

            if(currentHealth <= 0 && playerPhase < 10)
            {
                //PhaseUp();
            }
            else if(currentHealth <= 0 && playerPhase == 10)
            {
                Die();
            }
        }
    }

    private void PhaseUp()
    {
        playerPhase += 1;
        modMaxHealth -= 10f;
        currentHealth = modMaxHealth;
    }

    private void Die()
    {
        Debug.Log("Dead");
    }

    private IEnumerator DamageAnim(float start, float end, float max)
    {
        float healthScale;
        float healthChange = start;
        while(healthChange >= end)
        {
            //Debug.Log(start + " " + end + " " + max + " " + healthChange);
            healthScale = 1 - healthChange/max;
            yield return playerDark.localScale = new Vector3(healthScale, healthScale, healthScale);
            healthChange -= Time.deltaTime * 50f;
        }
    }

    private IEnumerator damageBuffer()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

}
