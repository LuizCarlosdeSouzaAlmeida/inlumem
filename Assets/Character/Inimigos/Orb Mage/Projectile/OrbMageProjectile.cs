using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMageProjectile : MonoBehaviour
{
    private Animator anim;
    private Health playerHealth;
    [SerializeField] private float rangeAttack1;
    [SerializeField] private float colliderDistance;
    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        //hit = true;
        //boxCollider.enabled = false;
        //anim.SetTrigger("explode");

    //    if (collision.tag == "Player"){
    //        collision.GetComponent<Health>().TakeDamage(1);
    //    }
    //}
    private bool PlayerInSightAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        //Attack
        //puple color
        Gizmos.color = new Color(0.5f, 0, 0.5f, 0.5f); 
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer() {
        // if player in sight, damage player
        if (PlayerInSightAttack()){
            playerHealth.TakeDamage(1);
        }
    }
    public void SetPlace(float x, float y)
    {
        boxCollider.enabled = true;
        transform.position = new Vector3(x, y, transform.position.z);
        gameObject.SetActive(true);
        Debug.Log("x: " + x + " y: " + y);
    }
    private void Desactivate()
    {
        gameObject.SetActive(false);
    }
}
