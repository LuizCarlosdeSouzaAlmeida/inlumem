using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float MoveAttackCooldown;
    [SerializeField] private float GroundAttackCooldown;
    [SerializeField] private float UpperAttackCooldown;

    [SerializeField] private float rangeMoveAttack;
    [SerializeField] private float rangeGroundAttack;
    [SerializeField] private float rangeGroundAttack2;
    [SerializeField] private float rangeUpperAttack;
    [SerializeField] private float rangeUpperAttack2;

    [Header("Collider Parameters")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private float ColliderDistanceMoveAttack;
    [SerializeField] private float ColliderDistanceGroundAttack;
    [SerializeField] private float ColliderDistanceGroundAttack2;
    [SerializeField] private float ColliderDistanceUpperAttack;
    [SerializeField] private float ColliderDistanceUpperAttack2;

    [SerializeField] private float YOffsetMoveAttack;
    [SerializeField] private float YOffsetGroundAttack;
    [SerializeField] private float YOffsetGroundAttack2;
    [SerializeField] private float YOffsetUpperAttack;
    [SerializeField] private float YOffsetUpperAttack2;

    [Header("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Cooldown Parameters")]
    private float CooldownTimerMoveAttack = Mathf.Infinity;
    private float CooldownTimerGroundAttack = Mathf.Infinity;
    private float CooldownTimerUpperAttack = Mathf.Infinity;


    private Animator anim;
    private Health playerHealth;
    private Transform player; // Referência ao transform do jogador
    private int State = 0;
    private bool IsAttacking = false;
    private bool CanFollow = false;
    [SerializeField] private int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CooldownTimerGroundAttack = 0;
        CooldownTimerMoveAttack = 0;
        CooldownTimerUpperAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownTimerMoveAttack += Time.deltaTime;
        CooldownTimerGroundAttack += Time.deltaTime;
        CooldownTimerUpperAttack += Time.deltaTime;
        if (State == 0)
        {
            if (CooldownTimerUpperAttack >= UpperAttackCooldown)
            {
                anim.SetTrigger("UpperAttack");
                State = 4;
                CooldownTimerUpperAttack = 0;
            }else if(CooldownTimerGroundAttack >= GroundAttackCooldown)
            {
                State = 2;
                anim.SetTrigger("GroundAttack");
                State = 4;
                CooldownTimerGroundAttack = 0;
            } else if (CooldownTimerMoveAttack >= MoveAttackCooldown)
            {
                State = 1;
                CanFollow = true;
                anim.SetTrigger("FollowMoveAttack");
            }
        }
        
    }
    void FixedUpdate()
    {
        Flip();
        if (State == 1)
        {
            if (PlayerInSightMoveAttack())
            {
                CooldownTimerMoveAttack = 0;
                State = 4;
                Debug.Log("Player in sight");
                anim.SetTrigger("MoveAttack");
            }
        }
        if (State == 1 && CanFollow)
        { 
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("playerSpotBottomCenter").transform.position, 5f * Time.deltaTime);
        }
        if (State == 4 && CanFollow)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("playerSpotBottomCenter").transform.position, 2f * Time.deltaTime);
        }
    }
    private void Flip()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private bool PlayerInSightMoveAttack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetMoveAttack, 0) + transform.right * rangeMoveAttack * transform.localScale.x * ColliderDistanceMoveAttack,
            new Vector3(boxCollider.bounds.size.x * rangeMoveAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }
    private bool PlayerInSightGroundAttack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack, 0) + transform.right * rangeGroundAttack * transform.localScale.x * ColliderDistanceGroundAttack,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        RaycastHit2D hit2 = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack, 0) + transform.right * rangeGroundAttack * transform.localScale.x * -ColliderDistanceGroundAttack,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        if (hit2.collider != null)
        {
            playerHealth = hit2.transform.GetComponent<Health>();
        }
        return hit.collider || hit2.collider != null;
    }
    private bool PlayerInSightGroundAttack2()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack2, 0) + transform.right * rangeGroundAttack2 * transform.localScale.x * ColliderDistanceGroundAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        RaycastHit2D hit2 = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack2, 0) + transform.right * rangeGroundAttack2 * transform.localScale.x * -ColliderDistanceGroundAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        if (hit2.collider != null)
        {
            playerHealth = hit2.transform.GetComponent<Health>();
        }
        return hit.collider || hit2.collider != null;
    }
    private bool PlayerInSightUpperAttack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack, 0) + transform.right * rangeUpperAttack * transform.localScale.x * ColliderDistanceUpperAttack,
                       new Vector3(boxCollider.bounds.size.x * rangeUpperAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        RaycastHit2D hit2 = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack, 0) + transform.right * rangeUpperAttack * transform.localScale.x * -ColliderDistanceUpperAttack,
                       new Vector3(boxCollider.bounds.size.x * rangeUpperAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        if (hit2.collider != null)
        {
            playerHealth = hit2.transform.GetComponent<Health>();
        }
        return hit.collider || hit2.collider != null;
    }
    private bool PlayerInSightUpperAttack2()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack2, 0) + transform.right * rangeUpperAttack2 * transform.localScale.x * ColliderDistanceUpperAttack2,
                       new Vector3(boxCollider.bounds.size.x * rangeUpperAttack2, boxCollider.bounds.size.y - 2.5f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        RaycastHit2D hit2 = Physics2D.BoxCast(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack2, 0) + transform.right * rangeUpperAttack2 * transform.localScale.x * -ColliderDistanceUpperAttack2,
                       new Vector3(boxCollider.bounds.size.x * rangeUpperAttack2, boxCollider.bounds.size.y - 2.5f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        if (hit2.collider != null)
        {
            playerHealth = hit2.transform.GetComponent<Health>();
        }
        return hit.collider || hit2.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetMoveAttack, 0) + transform.right * rangeMoveAttack * transform.localScale.x * ColliderDistanceMoveAttack,
            new Vector3(boxCollider.bounds.size.x * rangeMoveAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack, 0) + transform.right * rangeGroundAttack * transform.localScale.x * ColliderDistanceGroundAttack,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack, 0) + transform.right * rangeGroundAttack * transform.localScale.x * -ColliderDistanceGroundAttack,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack, 0) + transform.right * rangeUpperAttack * transform.localScale.x * ColliderDistanceUpperAttack,
            new Vector3(boxCollider.bounds.size.x * rangeUpperAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack2, 0) + transform.right * rangeUpperAttack2 * transform.localScale.x * ColliderDistanceUpperAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeUpperAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack, 0) + transform.right * rangeUpperAttack * transform.localScale.x * -ColliderDistanceUpperAttack,
            new Vector3(boxCollider.bounds.size.x * rangeUpperAttack, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetUpperAttack2, 0) + transform.right * rangeUpperAttack2 * transform.localScale.x * -ColliderDistanceUpperAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeUpperAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));

        Gizmos.color = new Color(1f, 1f, 1f, 1f);
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack2, 0) + transform.right * rangeGroundAttack2 * transform.localScale.x * ColliderDistanceGroundAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));
        Gizmos.color = new Color(1f, 1f, 1f, 1f);
        Gizmos.DrawWireCube(boxCollider.bounds.center + new Vector3(0, YOffsetGroundAttack2, 0) + transform.right * rangeGroundAttack2 * transform.localScale.x * -ColliderDistanceGroundAttack2,
            new Vector3(boxCollider.bounds.size.x * rangeGroundAttack2, boxCollider.bounds.size.y - 2f, boxCollider.bounds.size.z));
    }

    private void DamagePlayerMoveAttack()
    {
        // if player in sight, damage player
        if (PlayerInSightMoveAttack())
        {
            playerHealth.TakeDamage(1);
        }
    }
    private void DamagePlayerGroundAttack()
    {
        // if player in sight, damage player
        if (PlayerInSightGroundAttack())
        {
            playerHealth.TakeDamage(2);
        }
    }
    private void DamagePlayerGroundAttack2()
    {
        // if player in sight, damage player
        if (PlayerInSightGroundAttack2())
        {
            playerHealth.TakeDamage(2);
        }
    }
    private void DamagePlayerUpperAttack()
    {
        // if player in sight, damage player
        if (PlayerInSightUpperAttack())
        {
            playerHealth.TakeDamage(2);
        }
    }
    private void DamagePlayerUpperAttack2()
    {
        // if player in sight, damage player
        if (PlayerInSightUpperAttack2())
        {
            playerHealth.TakeDamage(2);
        }
    }

    private void SetCanFollow(int state)
    {
        if (state == 1)
        {
            CanFollow = true;
        }
        else
        {
            CanFollow = false;
        }
    }

    private void SetState(int state)
    {
        State = state;
    }
    public void MoveToBase()
    {
        transform.position = new Vector2(0, -4f);
    }
    private void SetBoxCollider(int State)
    {
        if (State == 0)
        {
            boxCollider.enabled = false;
        }
        else
        { 
            boxCollider.enabled = true;
        }
    }
}
