using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] public float startingHealth;

    [Header ("Collider Parameters")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D boxCollider;
    //[SerializeField] private GameObject light;


    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private DamageFlash _damageFlash;
    private PlayerDeathCheckPoint playerDeathCheckPoint;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        _damageFlash = GetComponent<DamageFlash>();
        if(GetComponent<PlayerMovement>() != null){
            playerDeathCheckPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeathCheckPoint>();
        }
        
    }
    public void TakeDamage(float _damage)
    {
        if (GetComponent<PlayerMovement>() != null){
            if (GetComponent<PlayerShield>().GetIsDefending() == 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
                Debug.Log("Player took damage");
                _damageFlash.CallDamageFlash();
                if (currentHealth > 0) 
                {
                    //StartCoroutine(Invunerability());
                }
                else
                {
                    if (!dead)
                    {

                        anim.SetTrigger("die");
                        //light.m_Intensity = 0;
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
                if(GetComponent<GhoulScript>() != null || GetComponent<SpitterScript>() != null || GetComponent<PigAssassinScript>()){
                    anim.SetTrigger("hit");
                }else{
                    Debug.Log("entrou aqui");
                    _damageFlash.CallDamageFlash();
                }
            
                
            }
            else
            {
                if (!dead)
                {
                    Debug.Log("Enemy died");
                    anim.SetTrigger("die");
                    //anim.SetBool("IsAlive", false);
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
                    if(GetComponent<WarpScript>() != null){
                        //GetComponent<WarpScript>().Desactivate();
                        GetComponent<WarpScript>().enabled = false;
                        boxCollider.enabled = false;
                    }
                    if(GetComponent<SpitterScript>() != null){
                        //GetComponent<WarpScript>().Desactivate();
                        GetComponent<SpitterScript>().DesactivateAllProjectiles();
                        GetComponent<SpitterScript>().enabled = false;
                        
                        boxCollider.enabled = false;
                    }
                    if (GetComponent<BossScript>() != null)
                    {
                        GetComponent<BossScript>().MoveToBase();
                        GetComponent<BossScript>().enabled = false;
                        boxCollider.enabled = false;
                    }
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
    public float GetHealth()
    {
        return startingHealth;
    }
    public void SetDead(bool _dead)
    {
        dead = _dead;
    }
    public void AliveAgain()
    {
        Debug.Log("Alive again");
        dead = false;
        currentHealth = startingHealth;
    }
    private void RevivePlayer(){
        playerDeathCheckPoint.WarpToSafeGround();
        //light.m_Intensity = 0;
        if(GetComponent<PlayerMovement>() != null)
            anim.SetTrigger("backToLife");
            anim.SetBool("IsInAction", false);
            GetComponent<PlayerMovement>().enabled = true;
        dead = false;
        currentHealth = startingHealth;
        //body.velocity = new Vector2(0, 0);
    }
}