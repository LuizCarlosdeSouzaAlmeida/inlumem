using System.Collections;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

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
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("rangedAttack");
        cooldownTimer = 0;
        
        //fireballs[FindFireball()].transform.position = firePoint.position;
        //fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        StartCoroutine(WaitForAttackSound());
    }
    // when triggered, this method will call fireball().
    private void FlechaThrow()
    {
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private IEnumerator WaitForAttackSound()
    {
        yield return new WaitForSeconds(0.4f);
        AudioSource.PlayOneShot(AttackAudio);
    }
}