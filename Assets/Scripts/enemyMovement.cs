using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public GameObject player = References.thePlayer;
    public float damage = 55f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    // Update is called once per frame
    void Update()
    {
        /*if(Physics2D.OverlapCircle(transform.position, 1f, playerLayer))
        {
            player.GetComponent<playerHealth>().TakeDamage(damage);
            Debug.Log("Contact");
        }*/
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
