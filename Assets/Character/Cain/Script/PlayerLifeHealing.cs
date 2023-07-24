using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeHealing : MonoBehaviour
{
    private Health playerHealth;
    private Animator anim;
    private AudioSource AudioSource;
    [SerializeField] private AudioClip AttackAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<Health>();
        AudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            Debug.Log("Healing");
            LifeHealing();
        }
            
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
