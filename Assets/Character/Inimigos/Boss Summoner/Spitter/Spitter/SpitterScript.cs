using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpitterScript : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    
    [SerializeField] private GameObject[] SpitterProjectiles;
    public AIPath[] aIPath;

    private Animator anim;
    private Transform player; // Referência ao transform do jogador
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        DisableAllaIPath();
    }

    // Update is called once per frame
    void Update()
    {
        FlipEnemy();
        cooldownTimer += Time.deltaTime;
        //Attack only when player in sight
        if(CheckClosePlayer()) {
            if(cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
    }
    private void DisableAllaIPath(){
        for (int i = 0; i < aIPath.Length; i++)
        {
            aIPath[i].canMove = false;
        }
    }
    private void FlipEnemy(){
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }
    private bool CheckClosePlayer() {
        return (Vector2.Distance(transform.position, player.position) < 8f);
    }
    private void ProjectileThrow()
    {
        int projectileIndex = FindProjectile();
        aIPath[projectileIndex].canMove = true;
        //SpitterProjectiles[projectileIndex].transform.position = transform.position;
        if (SpitterProjectiles[projectileIndex].GetComponent<SpitterProjectileScript>() != null){
            //SpitterProjectiles[projectileIndex].GetComponent<Health>().SetDead(false);
            //SpitterProjectiles[projectileIndex].GetComponent<Health>().AddHealth(1);
            SpitterProjectiles[projectileIndex].GetComponent<SpitterProjectileScript>().Activate();
        }else{
            Debug.Log("SpitterProjectileScript not found");
        }
        
        
    }
    private int FindProjectile()
    {
        for (int i = 0; i < SpitterProjectiles.Length; i++)
        {
            if (!SpitterProjectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    public void Desactivate() {
        //Desactivate enemy when health is 0
        gameObject.SetActive(false);
    }
    public void DesactivateAllProjectiles(){
        for (int i = 0; i < SpitterProjectiles.Length; i++)
        {
            SpitterProjectiles[i].GetComponent<SpitterProjectileScript>().DesactivateByDeath();
        }
    }
}
