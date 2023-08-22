using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Player Layer")]
    [SerializeField] private LayerMask enemyLayer;
    private float cooldownTimer = Mathf.Infinity;
    private Health enemyHealth;
    // Start is called before the first frame update
    private int stageAttack = 0;
    private Animator anim;
    private PlayerMovement playerMovement;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip AttackAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        AudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }
    private void Attack()
    {
        if (stageAttack == 0){
            anim.SetTrigger("attack1");
        }else if (stageAttack == 1){
            anim.SetTrigger("attack2");
        }else if (stageAttack == 2){
            anim.SetTrigger("attack3");
        }
        cooldownTimer = 0;

        AudioSource.PlayOneShot(AttackAudio);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private bool EnemyInSight() {
        //Check if player is in sight
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, enemyLayer);
        if(hit.collider != null) {
            enemyHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }
    private void DamageEnemy() {
        // if player in sight, damage player
        if (EnemyInSight()){
            enemyHealth.TakeDamage(damage);
        }
    }
    private void SetStageAttack(int stage){
        stageAttack = stage;
    }
}
