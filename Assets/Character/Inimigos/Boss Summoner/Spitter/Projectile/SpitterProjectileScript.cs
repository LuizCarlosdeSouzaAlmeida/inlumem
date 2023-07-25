using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpitterProjectileScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator anim;
    public AIPath aIPath;

    [SerializeField] private Transform firePoint;
    void Start()
    {
        anim = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Traps"));
        //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("CheckPoint"));
        Activate();
        //aIPath.canMove = false;
        //transform.position = GetComponentInParent<Transform>().position;
        // change parent position to 0 0 0
        //transform.parent.position = firePoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            boxCollider.enabled = false;
            anim.SetTrigger("die");
            DesativateMovement();
            collision.GetComponent<Health>().TakeDamage(1);
            gameObject.SetActive(false);
        }
    }
    public void Activate(){
        gameObject.SetActive(true);
        
        transform.parent.position = firePoint.position;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Traps"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("CheckPoint"));
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
        transform.parent.position = firePoint.position;
        gameObject.SetActive(true);
        aIPath.enabled = true;
        aIPath.canMove = true;
    }
    public void Desactivate() {
        //Desactivate enemy when health is 0
        gameObject.SetActive(false);
        aIPath.enabled = false;
        aIPath.canMove = false;
        //gameObject.parent.SetActive(false);
    }
    public void DesactivateByDeath() {
        //Desactivate enemy when health is 0
        anim = GetComponent<Animator>();
        anim.SetTrigger("die");
    }
    public void DesativateMovement() {
        aIPath.canMove = false;
    }
    public void ActivateMovement() {
        aIPath.canMove = true;
    }
}
