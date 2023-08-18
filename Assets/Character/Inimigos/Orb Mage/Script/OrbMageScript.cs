using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMageScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeAttackCooldown;

    [SerializeField] private float rangeAttack;
    [SerializeField] private float rangeRangedAttack;

    [SerializeField] private int damage;

    [SerializeField] private GameObject projectileOrb;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header ("Collider Parameters")]
    //[SerializeField]private Rigidbody2D body;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float colliderDistanceRangeAttack;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float cooldownSliceTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private Transform player; // Referência ao transform do jogador
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        cooldownSliceTimer += Time.deltaTime;

        if (PlayerInSightAttack() && anim.GetBool("IsInAction") == false && IsGrounded()) {
            // verifica se o cooldown do ataque, se estiver ok, realiza o ataque
            if(cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        if (PlayerInSightRangeAttack() && anim.GetBool("IsInAction") == false && IsGrounded()) {
            if(cooldownSliceTimer >= rangeAttackCooldown) {
                cooldownSliceTimer = 0;
                anim.SetTrigger("rangeAttack");
            }
        }
    }
    
   
    private bool PlayerInSightAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, -0.4f, 0) + transform.right * rangeAttack * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * rangeAttack, boxCollider.bounds.size.y-0.7f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private bool PlayerInSightRangeAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, -0.4f, 0) + transform.right * rangeRangedAttack * transform.localScale.x * colliderDistanceRangeAttack, 
            new Vector3(boxCollider.bounds.size.x * rangeRangedAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        //Attack
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, -0.4f, 0) + transform.right * rangeAttack * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAttack, boxCollider.bounds.size.y-0.7f, boxCollider.bounds.size.z));
        //Ranged Attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeRangedAttack * transform.localScale.x * colliderDistanceRangeAttack,
            new Vector3(boxCollider.bounds.size.x * rangeRangedAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        
    }
    private void DamagePlayer() {
        // if player in sight, damage player
        if (PlayerInSightAttack()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void CallRangeAttack()
    {
        // OrbMageProjectile receive the coordinate X of the player and y from the enemy
        projectileOrb.GetComponent<OrbMageProjectile>().SetPlace(player.position.x, player.position.y);
        //projectileOrb.GetComponent<OrbMageProjectile>().SetPlace(player.position.x, transform.position.y);
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
