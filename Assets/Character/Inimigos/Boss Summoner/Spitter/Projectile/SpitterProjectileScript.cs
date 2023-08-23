using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterProjectileScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator anim;
    private bool followPlayer = false;

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
        if(followPlayer){
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("playerSpotCenter").transform.position, 5f * Time.deltaTime);
        }else{

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            //boxCollider.enabled = false;
            anim.SetTrigger("die");
            //DesativateMovement();
            collision.GetComponent<Health>().TakeDamage(1);
            gameObject.SetActive(false);
            //boxCollider.enabled = false;
            //gameObject.SetActive(false);
            //followPlayer = false;
        }else if(collision.tag == "Ground"){
            anim.SetTrigger("explode");
            gameObject.SetActive(false);
            //DesativateMovement();
            //gameObject.SetActive(false);
            //followPlayer = false;
        }
    }
    public void Activate(){
        //gameObject.SetActive(true);
        
        transform.parent.position = firePoint.position;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Traps"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("CheckPoint"));
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
        transform.parent.position = firePoint.position;
        gameObject.SetActive(true);
    }
    public void Desactivate() {
        //Desactivate enemy when health is 0
        gameObject.SetActive(false);
        //gameObject.parent.SetActive(false);
    }
    public void DesactivateByDeath() {
        //Desactivate enemy when health is 0
        anim = GetComponent<Animator>();
        anim.SetTrigger("explode");
    }
    public void SetFollowPlayer(int follow){
        if(follow == 1){
            transform.position = firePoint.position;
            gameObject.SetActive(true);
            //boxCollider.enabled = true;
            followPlayer = true;
        }else{
            gameObject.SetActive(false);
            followPlayer = false;
        }
    }
}
