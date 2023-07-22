using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;

    [Header ("Collider Parameters")]
    [SerializeField]private Rigidbody2D body;
    [SerializeField]private BoxCollider2D boxCollider;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        if (GetComponent<PlayerMovement>() != null){
            if (GetComponent<PlayerShield>().GetIsDefending() == 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
                Debug.Log("Player took damage");
                if (currentHealth > 0) 
                {
                    StartCoroutine(Invunerability());
                }
                else
                {
                    if (!dead)
                    {
                        anim.SetTrigger("die");
                        
                        if(GetComponent<PlayerMovement>() != null)
                            GetComponent<PlayerMovement>().enabled = false;
                        dead = true;
                    }
                }
            }else{
                Debug.Log("Player blocked damage");
                
            }
        }else{
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            if (currentHealth > 0) 
            {
                StartCoroutine(Invunerability());
            }
            else
            {
                if (!dead)
                {
                    anim.SetTrigger("die");
                    anim.SetBool("IsAlive", false);
                    if(GetComponent<EnemyFollowPlayer>() != null){
                        GetComponent<EnemyFollowPlayer>().enabled = false;
                        boxCollider.enabled = false;
                        body.gravityScale = 0;
                        body.mass = 0;
                        body.velocity = new Vector2(0, 0);
                    }
                        

                    if(GetComponentInParent<EnemyFollowPlayerJump>() != null){
                        GetComponentInParent<EnemyFollowPlayerJump>().enabled = false;
                        boxCollider.enabled = false;
                        body.gravityScale = 0;
                        body.mass = 0;
                        body.velocity = new Vector2(0, 0);
                    }
                        

                    //if(GetComponent<MeleeEnemy>() != null)
                    //    GetComponent<MeleeEnemy>().enabled = false;    

                    //if(GetComponent<AssassinScript>() != null)
                       //GetComponent<AssassinScript>().enabled = false;

                    //if(GetComponent<OrbMageScript>() != null)
                        //GetComponent<OrbMageScript>().enabled = false;

                    //if(GetComponent<LongSliceScript>() != null)
                        //GetComponent<LongSliceScript>().enabled = false;
                    dead = true;
                }
            }
        }
        
        
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    // method make player invulnerable for a short period of time



    // Não está funcionando, o personagem pisca mas toma dano ainda, consertar.
    private IEnumerator Invunerability()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            
            spriteRend.color =  new Color(0, 0, 0, 0.5f);

            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }
}