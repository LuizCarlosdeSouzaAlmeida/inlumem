using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private Health playerHealth;
    //private SafeGroundSaver safeGroundSaver;
    private SafeGroundCheckPointSaver safeGroundCheckPointSaver;

    private void Start()
    {
        //safeGroundSaver = GameObject.FindGameObjectWithTag("Player").GetComponent<SafeGroundSaver>();
        safeGroundCheckPointSaver = GameObject.FindGameObjectWithTag("Player").GetComponent<SafeGroundCheckPointSaver>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.TakeDamage(1);
            //safeGroundSaver.WarpToSafeGround();
            safeGroundCheckPointSaver.WarpToSafeGround();
        }
        
    }
}
