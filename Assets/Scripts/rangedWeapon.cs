using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletRight;
    public GameObject bulletLeft;
    public GameObject player;

    public float attackRate = 10f;
    private float nextAttackTime = 0f;

    private bool reloading;
    public float maxReloadTime = 3f;
    private float reloadTime;
    public float reloadSpeed = 3f;

    public int maxAmmo = 10;
    private int ammo;

    public float latchChargeMax = 1.1f;
    private float latchCharge;

    void Start()
    {
        ammo = maxAmmo;
        reloadTime = maxReloadTime;
        reloading = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime && !reloading)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Shoot();
                ammo -= 1;
                nextAttackTime = Time.time + 1f/attackRate;
                reloadTime = 0;
            }
        }

        if(Input.GetButton("Fire2"))
        {
            latchCharge += Time.deltaTime;
        }

        if(Input.GetButtonUp("Fire2") && latchCharge >= latchChargeMax && ammo >= 5 && !reloading)
        {
            StartCoroutine(LatchShot());
            ammo -= 5;
            reloadTime = 0f;
            latchCharge = 0f;
        }

        else if(Input.GetButtonUp("Fire2"))
        {
            latchCharge = 0f;
        }

        

        if(reloadTime < maxReloadTime)
        {
            reloadTime += Time.deltaTime;
        }

        else if(reloadTime >= maxReloadTime && !reloading)
        {
            ammo = maxAmmo;
        }

        if(ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if(player.GetComponent<playerMovement>().isFacingRight)
        {
            Instantiate(bulletRight, firePoint.position, firePoint.rotation);
        }
        else
        {
            Instantiate(bulletLeft, firePoint.position, firePoint.rotation);
        }
    }

    private IEnumerator LatchShot()
    {
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        player.GetComponent<playerMovement>().isDashing = true;
        Debug.Log("Latch");
        float originalGravity = playerRB.gravityScale;
        playerRB.velocity = new Vector2(0f, 0f);
        playerRB.gravityScale = 0f;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Snap");
        float maxSnapSpeed = 80f;
        float snapSpeed = maxSnapSpeed;
        while(snapSpeed > maxSnapSpeed/4.5)
        {
            if(player.GetComponent<playerMovement>().isFacingRight)
            {        
                yield return playerRB.velocity = new Vector2(snapSpeed, 0f);
                snapSpeed -= Time.deltaTime * snapSpeed * 4f;
            }
            else
            {
                yield return playerRB.velocity = new Vector2(-snapSpeed, 0f);
                snapSpeed -= Time.deltaTime * snapSpeed * 4f;
            } 
        }
        playerRB.gravityScale = originalGravity;
        player.GetComponent<playerMovement>().isDashing = false;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadSpeed);
        reloading = false;
    }
}
