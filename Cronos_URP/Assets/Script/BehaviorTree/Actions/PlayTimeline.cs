using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 타임라인을 지정하고, 타임라인의 트랙에 맞춰 적절한 바인딩을 설정하여 타임라인을 실행합니다.
/// 타임라인이 재생되고 있는 동안 'Running' 상태를 유지하고, 타임라인이 일시 정지되면 'Success'를 반환합니다.
/// </summary>
class PlayTimeline : ActionNode
{
    public TimelineAsset timeline;

    private PlayableDirector _director;

    protected override void OnStart()
    {
        _director = context.gameObject.GetComponent<PlayableDirector>();

        _director.playableAsset = timeline;

        IEnumerable<TrackAsset> temp = timeline.GetOutputTracks();
        foreach (var kvp in temp)
        {
            if (kvp is AnimationTrack)
            {
                _director.SetGenericBinding(kvp, context.gameObject.GetComponent<Animator>());
            }
            else if (kvp is SpawnHitColliderTrack)
            {
                _director.SetGenericBinding(kvp, context.gameObject.transform);
            }
        }

        _director.Play();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if ( _director == null)
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