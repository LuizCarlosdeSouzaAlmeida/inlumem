using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGhoul : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] public float startingHealth;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private SpriteRenderer spriteRend;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float _damage)
    {
        
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0) 
        {
            anim.SetTrigger("hit");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                    

                if(GetComponentInParent<EnemyFollowPlayer>() != null){
                    GetComponentInParent<EnemyFollowPlayer>().enabled = false;
                    boxCollider.enabled = false;
                    body.gravityScale = 0;
                    body.mass = 0;
                    body.velocity = new Vector2(0, 0);
                }
                dead = true;
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}
