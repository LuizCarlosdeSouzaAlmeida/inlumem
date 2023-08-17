using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayingSwordScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            collision.GetComponent<Health>().TakeDamage(1);
        }
        if(collision.tag == "Ground"){
            anim.SetTrigger("ground");
            rb.mass = 0;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            
            boxCollider.enabled = false;
        }
    }
    public void breakChain(){
        anim.SetTrigger("break");
    }
    private void fall(){
        anim.SetTrigger("fall");
        rb.mass = 1;
        rb.gravityScale = 6;
    }
}
