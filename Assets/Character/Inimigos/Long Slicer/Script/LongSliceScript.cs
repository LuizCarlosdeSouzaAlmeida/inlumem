using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSliceScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attack1Cooldown;
    [SerializeField] private float sliceAttackCooldown;

    [SerializeField] private float rangeAttack1;
    [SerializeField] private float rangeSliceAttack;
    [SerializeField] private float rangeSliceAttack2;

    [SerializeField] private int damage;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float colliderDistanceSliceAttack;
    [SerializeField] private float colliderDistanceSliceAttack2;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float cooldownSliceTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private Transform player; // Referência ao transform do jogador
    private bool checkWall; // Verifica se tem uma parede na frente do inimigo
    //private bool isFacingRight = true; // Verifica a direção em que o inimigo está virado
    //private bool isFalling = false;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        cooldownSliceTimer += Time.deltaTime;
        
        // Verifica se o inimigo está no chão, se estiver, ele não está caindo
        if (IsGrounded())
        {
            //isFalling = false;
            // verifica o player para realizar a primeira animação de ataque, sendo um ataque curto
            if(PlayerInSightAttack()) {
                // dont move
                body.velocity = new Vector2(0, body.velocity.y);
                // verifica se o cooldown do ataque, se estiver ok, realiza o ataque
                if(cooldownTimer >= attack1Cooldown) {
                    cooldownTimer = 0;
                    anim.SetTrigger("attack1");
                }
            }
            // verifica o player para realizar a animação de slice ataque, sendo um ataque que vai para frente
            if(PlayerInSightSliceAttack() && !CheckFrontWall()) {
                // dont move
                body.velocity = new Vector2(0, body.velocity.y);
                if(cooldownSliceTimer >= sliceAttackCooldown) {
                    cooldownSliceTimer = 0;
                    anim.SetTrigger("sliceAttack1");
                }
            }
        }
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        anim.SetBool("grounded", raycastHit.collider != null);
        anim.SetBool("IsFalling", false);
        return raycastHit.collider != null;
    }
    private bool PlayerInSightAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private bool PlayerInSightSliceAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeSliceAttack * transform.localScale.x * colliderDistanceSliceAttack, new Vector3(boxCollider.bounds.size.x * rangeSliceAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private bool PlayerInSightSliceAttack2() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeSliceAttack2 * transform.localScale.x * colliderDistanceSliceAttack2, new Vector3(boxCollider.bounds.size.x * rangeSliceAttack2, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        //Slice Attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeSliceAttack * transform.localScale.x * colliderDistanceSliceAttack,
            new Vector3(boxCollider.bounds.size.x * rangeSliceAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        //Slice Attack 2
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeSliceAttack2 * transform.localScale.x * colliderDistanceSliceAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeSliceAttack2, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        //Attack
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y+0.5f, boxCollider.bounds.size.z));
    }
    private void DamagePlayer() {
        // if player in sight, damage player
        if (PlayerInSightAttack()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void DamagePlayerSliceAttack() {
        // if player in sight, damage player
        if (PlayerInSightSliceAttack()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void DamagePlayerSliceAttack2() {
        // if player in sight, damage player
        if (PlayerInSightSliceAttack2()){
            playerHealth.TakeDamage(damage);
        }
    }
    private void MoveAfterSliceAttack2(){
        // Tem um bug quando vai realizar o ataque e tá proximo da parede, faz com que o inimigo entre na parede, corrigir com um ray cast verificando a distancia da parede
        if(transform.localScale.x > 0){
            transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
        } else {
            transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
        }
    }
    private void CheckSliceAttack2(){
        if (PlayerInSightSliceAttack2()){
            anim.SetBool("IsInAction", false);
            anim.SetTrigger("sliceAttack2");
        }else{
            if(transform.localScale.x > 0){
                transform.position = new Vector2(transform.position.x + 2.0f, transform.position.y);
            } else {
                transform.position = new Vector2(transform.position.x - 2.0f, transform.position.y);
            }
        }
    }
    private bool CheckFrontWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.5f, 0, new Vector2(transform.localScale.x, 0), 2f, groundLayer);
        return raycastHit.collider != null;
    }
}
