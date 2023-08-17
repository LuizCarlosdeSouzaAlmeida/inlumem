using UnityEngine;

public class AssassinScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attack1Cooldown;
    [SerializeField] private float sliceAttackCooldown;

    [SerializeField] private float rangeAttack1;
    [SerializeField] private float rangeSliceAttack;

    [SerializeField] private int damage;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float cooldownSliceTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private Transform player; // Referência ao transform do jogador
    private bool checkWall; // Verifica se tem uma parede na frente do inimigo
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        cooldownSliceTimer += Time.deltaTime;
        

        // verifica o player para realizar a primeira animação de ataque, sendo um ataque curto
        if(PlayerInSightAttack() && IsGrounded() && !anim.GetBool("IsInAction")) {
            // dont move
            body.velocity = new Vector2(0, body.velocity.y);
            // verifica se o cooldown do ataque, se estiver ok, realiza o ataque
            if(cooldownTimer >= attack1Cooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack1");
            }
        }
        // verifica o player para realizar a animação de slice ataque, sendo um ataque que vai para frente
        if(PlayerInSightSliceAttack() && !CheckFrontWall()  && IsGrounded() && !anim.GetBool("IsInAction")) {
            // dont move
            body.velocity = new Vector2(0, body.velocity.y);
            if(cooldownSliceTimer >= sliceAttackCooldown) {
                cooldownSliceTimer = 0;
                anim.SetTrigger("sliceAttack");
            }
        }
    }
    private bool PlayerInSightAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private void PlayerInSightAttack2() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        anim.SetTrigger("attack2");
    }
    private bool PlayerInSightSliceAttack() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeSliceAttack * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * rangeSliceAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeSliceAttack * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeSliceAttack, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeAttack1 * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAttack1, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        // color purple
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 1f);

        // Calculate the end point of the raycast
        Vector2 raycastEndPoint = boxCollider.bounds.center + new Vector3((transform.localScale.x + 3.0f) * 2f, 0f, 0f);

        // Draw the BoxCast raycast
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        Gizmos.DrawLine(boxCollider.bounds.center, raycastEndPoint);
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
    private void MoveAfterSliceAttack(){
        // Tem um bug quando vai realizar o ataque e tá proximo da parede, faz com que o inimigo entre na parede, corrigir com um ray cast verificando a distancia da parede
        if(transform.localScale.x > 0){
            transform.position = new Vector2(transform.position.x + 3.0f, transform.position.y);
        } else {
            transform.position = new Vector2(transform.position.x - 3.0f, transform.position.y);
        }
    }
    private bool CheckFrontWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.5f, 0, new Vector2(transform.localScale.x, 0), 4f, groundLayer);
        //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.5f, 0, new Vector2(transform.localScale.x, 0), 4f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
