using UnityEngine;
using UnityEngine.Playables;

public class TimelineScript : MonoBehaviour
{
    public PlayableDirector playableDirector; // Reference to the PlayableDirector component

    private void Start()
    {
        // Subscribe to the Timeline's "stopped" event
        playableDirector.stopped += OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            Debug.Log("Timeline has ended. Execute your script here.");

            // GameObject cain = GameObject.FindWithTag("Player");

            // // cain.GetComponent<Player>().enabled = true;
            // var movement = cain.GetComponent<PlayerMovement>();
            // var rb = cain.GetComponent<Rigidbody2D>();

            // rb.velocity = Vector2.zero;

            // Debug.Log(movement);

            // // movement.enabled = true;
            // // cain.GetComponent<Player>().enabled = true;
        }
    }
}