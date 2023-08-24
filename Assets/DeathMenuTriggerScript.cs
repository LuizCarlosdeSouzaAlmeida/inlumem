using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Health playerHealth;
    void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
