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


    [HideInInspector] public float distance = 1;
    [HideInInspector] public float groundThreshold = .1f;
    [HideInInspector] public float fallAnimationThreshold = 0.1f;
    
    private bool isAttacking = false;
    private bool IsInAction = false;
    private bool isFalling = false;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private Movement movement;
    public float dashForce = 10f;
    public float rollForce = 5f;
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(2, 2, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-2, 2, 1);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0 && isGrounded());
        anim.SetBool("grounded", isGrounded());



         if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isGrounded())
            {
                anim.SetTrigger("roll");
                //body.velocity = new Vector2(body.velocity.x, 0f); // Parar o movimento no eixo Y
                //body.AddForce(new Vector2(dashForce, 0f), ForceMode2D.Impulse); // Dash no eixo X
            }else{
                anim.SetTrigger("dash");
                body.velocity = new Vector2(body.velocity.x, 0f); // Parar o movimento no eixo Y
                body.AddForce(new Vector2(dashForce, 0f), ForceMode2D.Impulse); // Dash no eixo X
            }
        }
        // outro código
        if (canFall() && !isFalling)
        {
            IsInAction = false;
            anim.SetTrigger("falling");
            isFalling = true;
            //if (CharacterIsCloseToGround(fallAnimationThreshold))
            //{
            //    Debug.Log("Olá, mundo!"); // Imprime "Olá, mundo!" no console
                //ResumeAnimation();
            //}
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        body.gravityScale = 3;
        if (GetIsAttacking() && isGrounded()){
            body.velocity = new Vector2(0, body.velocity.y);
        }else{
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("roll") || !anim.GetCurrentAnimatorStateInfo(0).IsName("dash"))
        //{
        //    SetFalseIsInAction();
        //    Debug.Log("Entrou");
        //}
        if(IsInAction){
            body.velocity = new Vector2(body.velocity.x, 0);
            body.AddForce(new Vector2(rollForce * horizontalInput, 0f), ForceMode2D.Impulse); // Roll no eixo X
        }
        if (isGrounded())
        {
            isFalling = false;
            coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
            jumpCounter = extraJumps; //Reset jump counter to extra jump value
        }
        else
            coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
    }

    private void Jump()
    {
        isFalling = false;
         if (coyoteCounter <= 0 && jumpCounter <= 0) return; 
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        //SoundManager.instance.PlaySound(jumpSound);
        IsInAction = false;
        if (isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        else
        {
            //If not on the ground and coyote counter bigger than 0 do a normal jump
            if (coyoteCounter > 0)
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canFall()
    {
        return !isGrounded() && body.velocity.y < 0 && !GetIsAttacking();
    }
    public bool canAttack()
    {
        return true;
        //return horizontalInput == 0 && isGrounded() && !onWall();
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
	private void SetTrueIsAttacking(){
        isAttacking = true;
    }
    private void SetFalseIsAttacking(){
        isAttacking = false;
    }
    private bool GetIsAttacking(){
        return isAttacking;
    }
    private void SetTrueIsInAction(){
        IsInAction = true;
    }
    private void SetFalseIsInAction(){
        IsInAction = false;
    }
    private bool GetIsInAction(){
        return IsInAction;
    }
}