using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    public enum Direction
    {
        Up, Right, Forward
    }

    public Direction direction;
    public float distance;

    private Vector3 _dir;
    Vector3 _finalPos;
    Vector3 _originalPos;

    //void Start()
    //{
    //    switch (direction)
    //    {
    //        case Direction.Up: _dir = Vector3.up; break;
    //        case Direction.Right: _dir = Vector3.right; break;
    //        case Direction.Forward: _dir = Vector3.forward; break;
    //    }

    //    _originalPos = transform.position;
    //    transform.position += _dir * distance;
    //}

    //private void Update()
    //{
    //    bool shouldMove = false;

    //    switch (direction)
    //    {
    //        case Direction.Up:
    //            shouldMove = _originalPos.y <= transform.position.y && transform.position.y <= _finalPos.y;
    //            break;
    //        case Direction.Right:
    //            shouldMove = _originalPos.x <= transform.position.x && transform.position.x <= _finalPos.x;
    //            break;
    //        case Direction.Forward:
    //            shouldMove = _originalPos.z <= transform.position.z && transform.position.z <= _finalPos.z;
    //            break;
    //    }

    //    if (shouldMove)
    //    {
    //        transform.Translate(_dir * Time.unscaledDeltaTime);
    //    }
    //}

    //public void Animate()
    //{
    //    switch (direction)
    //    {
    //        case Direction.Up: _finalPos.y += distance; break;
    //        case Direction.Right: _finalPos.x += distance; break;
    //        case Direction.Forward: _finalPos.z += distance; break;
    //    }

    //    while (CheckPosition() == false)
    //    {
    //        transform.Translate(_dir * Time.unscaledDeltaTime);
    //    }
    //}

    //private bool CheckPosition()
    //{
    //    Vector3 currentPos = transform.position;

    //    switch (direction)
    //    {
    //        case Direction.Up: return Mathf.Approximately(currentPos.y, _finalPos.y);
    //        case Direction.Right: return Mathf.Approximately(currentPos.x, _finalPos.x);
    //        case Direction.Forward: return Mathf.Approximately(currentPos.z, _finalPos.z);
    //    }

    //    return false;
    //}

}
