using UnityEngine;

public class AssassinScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attack1Cooldown;
    [SerializeField] private float sliceAttackCooldown;

    [SerializeField] private float rangeAttack1;
    [SerializeField] private float rangeSliceAttack;

    [SerializeField] private int damage;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius;

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
    private bool isFacingRight = true; // Verifica a direção em que o inimigo está virado
    private bool isFalling = false;
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

        IsFalling();
        // Se não estiver realizando nenhuma ação "importante" (como ataque) então verifica se o player está no raio de alcance e segue ele
        if(anim.GetBool("IsInAction") == false) {
            CheckPlayer();
        }
        
        // Verifica se o inimigo está no chão, se estiver, ele não está caindo
        if (IsGrounded())
        {
            isFalling = false;
        }
        
        // Verifica se tem uma parede na frente do inimigo e se ele está no chão, se estiver, ele pula
        if (CheckFrontWall() && IsGrounded()){
            Jump();
        }
        // verifica se o inimigo pode realizar a ação de cair, isto é, se o body.velocity.y < 0 e se não está no chão, e verifica se ele não está caindo, se não estiver, ele cai
        if (canFall() && !isFalling)
        {
            anim.SetTrigger("falling");
            isFalling = true;
        }

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
        if(PlayerInSightSliceAttack()) {
            // dont move
            body.velocity = new Vector2(0, body.velocity.y);
            if(cooldownSliceTimer >= sliceAttackCooldown) {
                cooldownSliceTimer = 0;
                anim.SetTrigger("sliceAttack");
            }
        }
        // verifica se o IsInAction está true, se estiver, o inimigo não se move
        if(anim.GetBool("IsInAction") == true) {
            body.velocity = new Vector2(0, 0);
        }
    }
    private void CheckPlayer(){
         float distanceToPlayerX = Mathf.Abs(player.position.x - transform.position.x);

        // Verifica se o jogador está dentro do raio de detecção no eixo X
        if (distanceToPlayerX <= detectionRadius)
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
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }
    private void Flip()
    {
        // Inverte a escala no eixo X
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = !isFacingRight;
    }
    private void Jump()
    {
        anim.SetTrigger("jump");
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }
    private bool CheckFrontWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * 0.5f, 0, new Vector2(transform.localScale.x + 0.5f, 0), 2f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        anim.SetBool("grounded", raycastHit.collider != null);
        anim.SetBool("IsFalling", false);
        return raycastHit.collider != null;
    }
    public void IsFalling(){
        if (body.velocity.y < 0){
            anim.SetBool("IsFalling", true);
        }
    }
    public bool canFall()
    {
        //return !isGrounded() && body.velocity.y < 0 && !GetIsAttacking();
        return !IsGrounded() && body.velocity.y < 0;
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
    private void MoveAfterSliceAttack(){
        // Tem um bug quando vai realizar o ataque e tá proximo da parede, faz com que o inimigo entre na parede, corrigir com um ray cast verificando a distancia da parede
        if(transform.localScale.x > 0){
            transform.position = new Vector2(transform.position.x + 3.0f, transform.position.y);
        } else {
            transform.position = new Vector2(transform.position.x - 3.0f, transform.position.y);
        }
    }
}
