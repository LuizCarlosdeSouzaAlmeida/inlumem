using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineScript : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public UnityEvent onEndEvents;

    private void Start()
    {
        playableDirector.stopped += OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            onEndEvents?.Invoke();
        }
    }
}