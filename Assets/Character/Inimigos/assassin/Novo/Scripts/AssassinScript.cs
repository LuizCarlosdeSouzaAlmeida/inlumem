using UnityEngine;

public class AssassinScript : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius;
    [SerializeField]private Rigidbody2D body;
    private Animator anim;
    [SerializeField]private BoxCollider2D boxCollider;

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
        IsFalling();
        CheckPlayer();
        if (IsGrounded())
        {
            isFalling = false;
        }
        
        if (CheckFrontWall() && IsGrounded()){
            Jump();
        }
        if (canFall() && !isFalling)
        {
            anim.SetTrigger("falling");
            isFalling = true;
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

    // Método para verificar se tem uma parede na frente do personagem para ele realizar um pulo
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
}
