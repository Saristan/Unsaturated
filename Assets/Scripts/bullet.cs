using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 10;
    private int knockType;
    public float knockBack = 1;

    // Start is called before the first frame update
    void Start()
    {
            rb.velocity = transform.right * speed;
            StartCoroutine(BulletDeath());
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        enemy enemy = hitInfo.GetComponent<enemy>();
        
        if (enemy != null)
        {
            GameObject player = References.thePlayer;
            References.thePlayer.GetComponent<rangedWeapon>().StunShotCall(hitInfo);
        }
        
        Destroy(gameObject);
    }

    private IEnumerator BulletDeath()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
