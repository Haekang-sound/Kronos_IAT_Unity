using UnityEngine;

public class BoxColliderAdjuster : MonoBehaviour
{
    public bool drawGizmos;

    public Vector3 newCenter;
    public Vector3 newSize;

    Vector3 _originalCenter;
    Vector3 _originalSize;

    private Collider _collider;

    void Start()
    {
        _collider = GetComponent<Collider>();

        if (_collider is BoxCollider boxCollider)
        {
            _originalCenter = boxCollider.center;
            _originalSize = boxCollider.size;
            newSize = _originalSize;
        }
    }

    public void Adjust()
    {
        if (_collider is BoxCollider boxCollider)
        {
            boxCollider.center = newCenter;
            boxCollider.size = newSize;
        }
    }

    public void Reset()
    {
        if (_collider is BoxCollider boxCollider)
        {
            boxCollider.center = _originalCenter;
            boxCollider.size = _originalSize;
        }
    }

    public void OnDrawGizmos()
    {
        if (drawGizmos == false) { return; }

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.magenta;

        Gizmos.DrawWireCube(newCenter, newSize);
    }
}