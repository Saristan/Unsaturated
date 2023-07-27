using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttacks : MonoBehaviour
{
    public Transform c1;
    public Transform c2;
    public Transform dashC;
    public Transform chargeC1;
    public Transform chargeC2;

    public LayerMask enemyLayers;
    public GameObject player;
    public Transform attackBox;

    private float chargeAttackTimer = 0f;
    public float chargeAttackWarm = 1.5f;

    public int attackDamage = 40;
    public float attackRate = 2.5f;
    private float nextAttackTime = 0f;
    
    public float knockBack = 6f;
    public float upKnockMult = 1.3f;
    public float selfKnockBack = 1.2f;
    public int airSlashCount;


    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if(!player.GetComponent<playerMovement>().IsGrounded())
            {
                if(player.GetComponent<playerMovement>().isFacingRight)
                {
                    attackBox.eulerAngles = Vector3.forward * 90 * Input.GetAxisRaw("Vertical");
                }
                else
                {
                    attackBox.eulerAngles = Vector3.forward * 90 * -Input.GetAxisRaw("Vertical");
                }
            }
            else
            {
                attackBox.eulerAngles = Vector3.forward * 0;

                airSlashCount = 0;
                if(Input.GetButton("Up"))
                {
                    if(player.GetComponent<playerMovement>().isFacingRight)
                    {
                        attackBox.eulerAngles = Vector3.forward * 90 * Input.GetAxisRaw("Vertical");
                    }
                    else
                    {
                        attackBox.eulerAngles = Vector3.forward * 90 * -Input.GetAxisRaw("Vertical");
                    }
                }
            }

            if (Input.GetButton("Fire1"))
            {
                chargeAttackTimer += Time.deltaTime;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (chargeAttackTimer >= chargeAttackWarm)
                {
                    ChargeAttack();
                }
                chargeAttackTimer = 0f;
            }

            if (Input.GetButtonDown("Fire1") && player.GetComponent<playerMovement>().isDashing)
                {
                    Debug.Log("DashAttack");
                    DashAttack();
                }
            else if (Input.GetButtonDown("Fire1"))
                {
                    BaseAttack();
                    nextAttackTime = Time.time + 1f/attackRate;
                } 
        }
        
    }

    private void BaseAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(c1.position, c2.position, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            playerMovement pm = player.GetComponent<playerMovement>();

            if(Input.GetButton("Up"))
            {
                enemy.GetComponent<enemy>().TakeDamage(attackDamage, (enemy.transform.position.x - transform.position.x) * 1.3f, knockBack * upKnockMult);
            }
            else if(Input.GetButton("Down") && !pm.IsGrounded())
            {
                enemy.GetComponent<enemy>().TakeDamage(attackDamage, 0f, -knockBack);
                rb.velocity = new Vector2(0f, knockBack * selfKnockBack);
                pm.doubleJump = 0;
                airSlashCount = 0;
            }
            else{
                if (enemy.transform.position.x <= transform.position.x)
                {
                    enemy.GetComponent<enemy>().TakeDamage(attackDamage, -knockBack, 0f);
                }
                else if (enemy.transform.position.x > transform.position.x)
                {
                    enemy.GetComponent<enemy>().TakeDamage(attackDamage, knockBack, 0f);
                }

                if(!pm.IsGrounded() && airSlashCount < 5)
                {
                    rb.velocity = new Vector2(0f, knockBack);
                    airSlashCount += 1;
                    if(!enemy.GetComponent<enemyMovement>().IsGrounded())
                    {
                        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(enemy.GetComponent<Rigidbody2D>().velocity.x, knockBack);
                    }
                }
            }
        }
    }
    
    private void DashAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(c1.position, dashC.position, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            float airDashKB = 1f;

            if(!enemy.GetComponent<enemyMovement>().IsGrounded())
            {
                airDashKB = 1.4f;
            }

            if (enemy.transform.position.x <= transform.position.x)
            {
                enemy.GetComponent<enemy>().TakeDamage(attackDamage, -knockBack * 1.5f * airDashKB, knockBack * 1.2f);
            }
            if (enemy.transform.position.x > transform.position.x)
            {
                enemy.GetComponent<enemy>().TakeDamage(attackDamage, knockBack * 1.5f * airDashKB, knockBack * 1.2f);
            }
        }
    }

    private void ChargeAttack()
    {
        if(Input.GetButton("Down") && !player.GetComponent<playerMovement>().IsGrounded())
        {
            StartCoroutine(DownCharge());
            Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(c1.position, c2.position, enemyLayers);
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<enemy>().TakeDamage(attackDamage, 0f, knockBack * -6f);
            }
        }

        if(Input.GetButton("Up"))
        {
            StartCoroutine(UpCharge());
        }

        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(chargeC1.position, chargeC2.position, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                if (enemy.transform.position.x <= transform.position.x)
                {
                    enemy.GetComponent<enemy>().TakeDamage(attackDamage, -knockBack * .7f, knockBack * 1.5f);
                }
                if (enemy.transform.position.x > transform.position.x)
                {
                    enemy.GetComponent<enemy>().TakeDamage(attackDamage, knockBack * .7f, knockBack * 1.5f);
                }
            }
        }
    }

    private IEnumerator UpCharge()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        player.GetComponent<playerMovement>().isDashing = true;
        float maxSnapSpeed = 40f;
        float snapSpeed = maxSnapSpeed;
        StartCoroutine(UpChargeDamage());
        while(snapSpeed < maxSnapSpeed/4.5)
        {     
            yield return rb.velocity = new Vector2(0f, snapSpeed);
        }
        player.GetComponent<playerMovement>().isDashing = false;
    }

    private IEnumerator DownCharge()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        player.GetComponent<playerMovement>().isDashing = true;
        float maxSnapSpeed = -30f;
        float snapSpeed = maxSnapSpeed;
        float airTime = 0f;
        //StartCoroutine(DownChargeDamage());
        while(!player.GetComponent<playerMovement>().IsGrounded())
        {
            yield return rb.velocity = new Vector2(0f, snapSpeed);
            airTime += Time.deltaTime;
        }
        Debug.Log("Down");
        player.GetComponent<playerMovement>().isDashing = false;
    }

    private IEnumerator UpChargeDamage()
    {
        for(int i = 0; i < 4; i++)
        {
            Debug.Log("Boom");
            yield return new WaitForSeconds(0.2f);
        }
    }
    
}
