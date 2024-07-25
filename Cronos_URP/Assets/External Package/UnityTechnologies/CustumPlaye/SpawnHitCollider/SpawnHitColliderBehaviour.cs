using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SpawnHitColliderBehaviour : PlayableBehaviour
{
    public float damageAmount = 1;

    public LayerMask targetLayers;

    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule
    }

    public enum Direction
    {
        XAxis,
        YAxis,
        ZAxis
    }

    //public Collider newBehaviourVariable;

    public ColliderType colliderType;

    // common
    public Vector3 center;

    // box
    public Vector3 size = new Vector3(1f, 1f, 1f);

    // sphere, capsule
    public float radius = 1f;

    // capsule
    public float height = 1f;
    public Direction direction;
}
