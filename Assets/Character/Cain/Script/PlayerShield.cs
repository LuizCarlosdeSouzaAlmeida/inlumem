using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private float shieldCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && cooldownTimer > shieldCooldown && playerMovement.canAttack()){
            Defend();
        }

        cooldownTimer += Time.deltaTime;
    }
    private void Defend()
    {
        anim.SetTrigger("shield");
        cooldownTimer = 0;
    }
    private void SetIsDefending(bool isDefending)
    {
        anim.SetBool("isDefending", isDefending);
    }
}
