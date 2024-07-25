using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

class PlayTimeline : ActionNode
{
    public TimelineAsset timeline;

    private PlayableDirector _director;

    protected override void OnStart()
    {
        _director = blackboard.monobehaviour.GetComponent<PlayableDirector>();

        _director.playableAsset = timeline;

        IEnumerable<TrackAsset> temp = timeline.GetOutputTracks();
        foreach (var kvp in temp)
        {
            if (kvp is AnimationTrack)
            {
                _director.SetGenericBinding(kvp, blackboard.monobehaviour.GetComponent<Animator>());
            }
            else if (kvp is SpawnHitColliderTrack)
            {
                _director.SetGenericBinding(kvp, blackboard.monobehaviour.transform);
            }
        }

        _director.Play();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.monobehaviour == null || _director == null)
        {
            return State.Failure;
        }

        if (_director.state == PlayState.Paused)
        {
            return State.Success;
        }

        return State.Running;
    }

}