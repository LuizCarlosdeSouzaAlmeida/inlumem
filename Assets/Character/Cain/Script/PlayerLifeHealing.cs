using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHealing : MonoBehaviour
{
    [SerializeField] private float healingCooldown;
    private Health playerHealth;
    private Animator anim;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip AttackAudio;


    private float cooldownTimer = Mathf.Infinity;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<Health>();
        AudioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetButton("ButtonHealing") && cooldownTimer > healingCooldown && playerMovement.isGrounded()){
            cooldownTimer = 0;
            LifeHealing();
        }
        cooldownTimer += Time.deltaTime;
    }
    private void LifeHealing()
    {
        Debug.Log("Current Health: " + playerHealth.currentHealth);
        Debug.Log("Max Health: " + playerHealth.GetHealth());
        if (playerHealth.currentHealth < playerHealth.GetHealth()){
            anim.SetTrigger("lifeHealing");
            AudioSource.PlayOneShot(AttackAudio);
        }
    }
}
