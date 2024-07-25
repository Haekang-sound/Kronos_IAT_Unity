using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 0f, 0f)]
[TrackClipType(typeof(SpawnHitColliderClip))]
[TrackBindingType(typeof(Transform))]
public class SpawnHitColliderTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SpawnHitColliderMixerBehaviour>.Create (graph, inputCount);
    }
}
