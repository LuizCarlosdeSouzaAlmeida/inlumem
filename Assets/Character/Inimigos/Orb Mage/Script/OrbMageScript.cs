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

    [Header("Movement Parameters")]
    [SerializeField] private float speed;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius;

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
    private bool checkWall; // Verifica se tem uma parede na frente do inimigo
    private bool isFacingRight = true; // Verifica a direção em que o inimigo está virado
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

        if(anim.GetBool("IsInAction") == false) {
            CheckPlayer();
        }
        if (PlayerInSightAttack() && anim.GetBool("IsInAction") == false) {
            // verifica se o cooldown do ataque, se estiver ok, realiza o ataque
            if(cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        if (PlayerInSightRangeAttack() && anim.GetBool("IsInAction") == false) {
            if(cooldownSliceTimer >= rangeAttackCooldown) {
                cooldownSliceTimer = 0;
                anim.SetTrigger("rangeAttack");
            }
        }
    }
    private void CheckPlayer(){
         float distanceToPlayerX = Mathf.Abs(player.position.x - transform.position.x);

        // Verifica se o jogador está dentro do raio de detecção no eixo X
        if (distanceToPlayerX <= detectionRadius && !CheckFrontWall())
        {
            anim.SetBool("follow", true);
            // Move o inimigo em direção ao jogador apenas no eixo X
            float step = speed * Time.deltaTime;
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

            // Verifica a direção do movimento para definir a escala
            if (targetPosition.x < transform.position.x && isFacingRight)
            {
                // Inverte a escala no eixo X quando indo para a esquerda
                Flip();
            }
            else if (targetPosition.x > transform.position.x && !isFacingRight)
            {
                // Restaura a escala padrão quando indo para a direita
                Flip();
            }
        }
        else
        {
            anim.SetBool("follow", false);
        }
    }
    private void Flip()
    {
        // Inverte a escala no eixo X
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = !isFacingRight;
    }
    private bool CheckFrontWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.5f, 0, new Vector2(transform.localScale.x + 0.5f, 0), 2f, groundLayer);
        return raycastHit.collider != null;
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
    private void SetIsInAction(int value)
    {
        if (value == 1)
        {
            anim.SetBool("IsInAction", true);
        }
        else
        {
            anim.SetBool("IsInAction", false);
        }
    }
    private void CallRangeAttack()
    {
        // OrbMageProjectile receive the coordinate X of the player and y from the enemy
        projectileOrb.GetComponent<OrbMageProjectile>().SetPlace(player.position.x, transform.position.y);
    }
}
