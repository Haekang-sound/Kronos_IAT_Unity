using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SpawnHitColliderClip : PlayableAsset, ITimelineClipAsset
{
    public SpawnHitColliderBehaviour template = new SpawnHitColliderBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpawnHitColliderBehaviour>.Create (graph, template);
        SpawnHitColliderBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
