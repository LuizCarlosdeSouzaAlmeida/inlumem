using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private float shieldCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public int isDefending = 0;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetButton("ButtonDefend") && cooldownTimer > shieldCooldown && playerMovement.canAttack() && playerMovement.isGrounded())
        {

            Defend();
        }

        cooldownTimer += Time.deltaTime;
    }
    private void Defend()
    {
        anim.SetTrigger("shield");
        cooldownTimer = 0;
    }
    private void SetIsDefending(int isDefending)
    {
        this.isDefending = isDefending;
    }
    public int GetIsDefending()
    {
        return isDefending;
    }
}