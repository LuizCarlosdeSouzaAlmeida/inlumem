using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayingSwordTriggerScript : MonoBehaviour
{
    [SerializeField] private GameObject SwayingSword;
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            SwayingSword.GetComponent<SwayingSwordScript>().breakChain();
        }
    }
}
