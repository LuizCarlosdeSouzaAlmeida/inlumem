using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveScript : MonoBehaviour
{
    private bool isInsideTrigger = false;
    private void Update()
    {
        if (isInsideTrigger && Input.GetKeyDown(KeyCode.W))
        {
            ExecuteAction();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideTrigger = false;
        }
    }
    private void ExecuteAction()
    {
        // Coloque aqui o c�digo que voc� deseja executar quando o jogador pressionar W dentro do trigger.
        Loader.Load(Loader.Scene.BossCutscene);
    }
}
