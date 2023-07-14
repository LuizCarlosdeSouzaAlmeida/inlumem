using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayerJump : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Detection Parameters")]
    [SerializeField] private float detectionRadius;

    [Header ("Enemy Layer")]
    //[SerializeField] private LayerMask playerLayer;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;

    private Animator anim;
    private Health playerHealth;
    private Transform player; // Referência ao transform do jogador
    private bool checkWall; // Verifica se tem uma parede na frente do inimigo
    private bool isFacingRight = true; // Verifica a direção em que o inimigo está virado
    private bool isFalling = false;
    private Transform playerSpotRight; // Referência ao transform do spotRight do jogador
    private Transform playerSpotLeft; // Referência ao transform do spotLeft do jogador
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSpotRight = GameObject.FindGameObjectWithTag("playerSpotRight").transform;
        playerSpotLeft = GameObject.FindGameObjectWithTag("playerSpotLeft").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        IsFalling();
        // Se não estiver realizando nenhuma ação "importante" (como ataque) então verifica se o player está no raio de alcance e segue ele
        if(anim.GetBool("IsInAction") == false) {
            CheckPlayer();
            FlipEnemy();
        }else{
            body.velocity = new Vector2(0, 0);
        }
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
    private void FlipEnemy(){
        if (player.position.x < transform.position.x && isFacingRight)
        {
            // Inverte a escala no eixo X quando indo para a esquerda
            Flip();
        }
        else if (player.position.x > transform.position.x && !isFacingRight)
        {
            // Restaura a escala padrão quando indo para a direita
            Flip();
        }
    }
    private void CheckPlayer()
    {
        Transform spot;
        float distanceToPlayerX;
        float distanceToPlayerSpotRightX = Mathf.Abs(playerSpotRight.position.x - transform.position.x);
        float distanceToPlayerSpotLeftX = Mathf.Abs(playerSpotLeft.position.x - transform.position.x);
        if (distanceToPlayerSpotRightX <= distanceToPlayerSpotLeftX)
        {
            distanceToPlayerX = distanceToPlayerSpotRightX;
            spot = playerSpotRight;
        }
        else
        {
            distanceToPlayerX = distanceToPlayerSpotLeftX;
            spot = playerSpotLeft;
        }
        // Verifica se o jogador está dentro do raio de detecção no eixo X
        if (distanceToPlayerX <= detectionRadius && distanceToPlayerX > 0.1f)
        {
            anim.SetBool("follow", true);
            // Move o inimigo em direção ao jogador apenas no eixo X
            float step = speed * Time.deltaTime;
            Vector2 targetPosition = new Vector2(spot.position.x, transform.position.y);
            // only move if is facing the player
            if (targetPosition.x > transform.position.x && isFacingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            }
            else if (targetPosition.x < transform.position.x && !isFacingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
            }
           

            // Verifica a direção do movimento para definir a escala
            //if (targetPosition.x < transform.position.x && isFacingRight)
            //{
            //    // Inverte a escala no eixo X quando indo para a esquerda
            //    Flip();
            //}
            //else if (targetPosition.x > transform.position.x && !isFacingRight)
            //{
            //    // Restaura a escala padrão quando indo para a direita
            //    Flip();
            //}
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
    
}
