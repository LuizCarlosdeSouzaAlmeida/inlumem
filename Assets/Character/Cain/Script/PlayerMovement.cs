using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    //[Header("Dash count e Roll cooldown")]
    private float dashCount = 1;
    [SerializeField] private float rollCooldown;

    public bool isAttacking = false;
    private bool isFalling = false;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private float horizontalInput;

    [SerializeField] public float dashForce = 10f;
    [SerializeField] public float rollForce = 5f;
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim.SetBool("CheckDeadCompleted", true);
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.gravityScale = 3;

        //Flip player when moving left-right
        //Flip personagem, se está indo para direita ou esquerda
        Flip();


        //Set animator parameters
        // Setar os parâmetros booleanos run e grounded do animator
        SetParameters();

        // Verificar Input de dash e executar a animação correspondente
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded() && dashCount > 0)
        {
            anim.SetTrigger("dash");
            dashCount = 0;
        }


        // Ainda falta configurar um cooldown para o roll
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded())
        {
            anim.SetTrigger("roll");
        }


        // Executar animação de falling caso as condições sejam atendidas
        if (canFall() && !isFalling)
        {
            anim.SetTrigger("falling");
            isFalling = true;
        }

        //Executar jump caso tecla de espaço seja pressionada
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        //Parar o personagem no eixo X caso ele esteja realizando um ataque e esteja no chão
        if (GetIsAttacking() && isGrounded())
        {
            body.velocity = new Vector2(0, body.velocity.y); body.velocity = new Vector2(horizontalInput * speed * 0.5f, body.velocity.y);
            //body.velocity = new Vector2(0, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }

        // Verificar se o personagem está com o parâmetro IsInAction ativo, e aplicar força de dash ou roll 
        if (anim.GetBool("IsInAction"))
        {
            DashOrRollForce();
        }
        if (isGrounded())
        {
            dashCount = 1;
            isFalling = false;
            coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
            jumpCounter = extraJumps; //Reset jump counter to extra jump value
        }
        else
            coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
    }
    void FixedUpdate()
    {
        //Verificar se o personagem está no chão
        CheckIDLEAnimation();
    }
    private void Flip()
    {
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(2, 2, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-2, 2, 1);
    }


    private void SetParameters()
    {
        anim.SetBool("run", horizontalInput != 0 && isGrounded());
        anim.SetBool("grounded", isGrounded());
    }


    private void Jump()
    {
        isFalling = false;
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        //SoundManager.instance.PlaySound(jumpSound);
        if (isGrounded())
        {
            if (anim.GetBool("IsInAction") == false)
            {
                anim.SetTrigger("jump");
            }

            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else
        {
            //If not on the ground and coyote counter bigger than 0 do a normal jump
            if (coyoteCounter > 0)
            {
                if (anim.GetBool("IsInAction") == false)
                {
                    anim.SetTrigger("jump");
                }
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
            {
                if (jumpCounter > 0)
                {
                    if (anim.GetBool("IsInAction") == false)
                    {
                        anim.SetTrigger("jump");
                    }
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void DashOrRollForce()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("roll"))
        {
            body.AddForce(new Vector2(rollForce * horizontalInput, 0f), ForceMode2D.Impulse); // Roll no eixo X
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
        {
            body.velocity = new Vector2(body.velocity.x, 0);
            body.AddForce(new Vector2(dashForce * horizontalInput, 0f), ForceMode2D.Impulse); // Roll no eixo X
        }
    }
    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    public bool canFall()
    {
        //return !isGrounded() && body.velocity.y < 0 && !GetIsAttacking();
        return !isGrounded() && body.velocity.y < 0;
    }

    // Falta aplicar condições do canAttack()
    public bool canAttack()
    {
        return true;
        //return horizontalInput == 0 && isGrounded() && !onWall();
    }
    private void CheckIDLEAnimation()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetBool("IsInAction", false);
            GetComponent<PlayerMeleeAttack>().stageAttack = 0;
            GetComponent<PlayerShield>().isDefending = 0;
        }

    }

    private void FreezeAnimation()
    {
        anim.speed = 0;
    }
    private void ResumeAnimation()
    {
        anim.speed = 1;
    }

    public bool CharacterIsCloseToGround(float distance)
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, distance, groundLayer);
    }

    private void SetTrueIsAttacking()
    {
        isAttacking = true;
    }
    private void SetFalseIsAttacking()
    {
        isAttacking = false;
    }
    private bool GetIsAttacking()
    {
        return isAttacking;
    }
    private void SetTrueIsInAction()
    {
        anim.SetBool("IsInAction", true);
    }
    private void SetFalseIsInAction()
    {
        anim.SetBool("IsInAction", false);
    }
}