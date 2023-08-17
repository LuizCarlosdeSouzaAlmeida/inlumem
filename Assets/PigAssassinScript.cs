using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAssassinScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeAttack;
    [SerializeField] private int damage;

    [SerializeField] private float stabCooldown;
    [SerializeField] private float rangeStab;
    

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderAttackDistance;
    [SerializeField] private float colliderStabDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownAttackTimer = Mathf.Infinity;
    private float cooldownStabTimer = Mathf.Infinity;
    private Transform player;
    private Transform pigTransform;

    private Animator anim;
    private Health playerHealth;

    private void Awake() {
        pigTransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }
    private void Update() {
        cooldownAttackTimer += Time.deltaTime;
        cooldownStabTimer += Time.deltaTime;
        //Attack only when player in sight
        //Debug.Log(CheckBackPlayer());
        if(PlayerInSightStab()) {
            if(cooldownStabTimer >= attackCooldown && CheckBackPlayer()) {
                cooldownStabTimer = 0;
                anim.SetTrigger("stab");
            }
        }

        if(PlayerInSightAttack()) {
            if(cooldownAttackTimer >= attackCooldown) {
                cooldownAttackTimer = 0;
                anim.SetTrigger("attack");
            }
        }

        
    }
    private bool CheckBackPlayer(){
        if(player.transform.localScale.x < 0.1f && player.transform.position.x < pigTransform.position.x){
            return true;
        }else if(player.transform.localScale.x > 0.1f && player.transform.position.x > pigTransform.position.x){
            return true;
        }else{
            return false;
        }
    }
    private bool PlayerInSightAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeAttack * transform.localScale.x * colliderAttackDistance, 
            new Vector3(boxCollider.bounds.size.x * rangeAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private bool PlayerInSightStab() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeStab * transform.localScale.x * colliderStabDistance + new Vector3(0, -0.4f, 0), 
            new Vector3(boxCollider.bounds.size.x * rangeStab, boxCollider.bounds.size.y - 0.7f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeAttack * transform.localScale.x * colliderAttackDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeStab * transform.localScale.x * colliderStabDistance + new Vector3(0, -0.4f, 0),
            new Vector3(boxCollider.bounds.size.x * rangeStab, boxCollider.bounds.size.y - 0.7f, boxCollider.bounds.size.z));
    }
    private void DamagePlayerAttack() {
        // if player in sight, damage player
        if (PlayerInSightAttack()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void DamagePlayerStab() {
        // if player in sight, damage player
        if (PlayerInSightStab() && CheckBackPlayer()){
            playerHealth.TakeDamage(damage * 2);
        }
    }
}
