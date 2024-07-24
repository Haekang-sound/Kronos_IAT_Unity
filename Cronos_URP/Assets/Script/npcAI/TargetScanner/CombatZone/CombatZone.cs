using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatZone : MonoBehaviour
{
    public bool drawGizmos;

    public bool isClear;
    public GameObject target;
    public CombatZoneEnemy[] enemyList;

    private bool isTargetIn;
    public GameObject Detect(Transform detector, bool useHeightDifference = true)
    {
        if (isTargetIn)
        {
            return target;
        }

        return null;
    }

    private void Awake()
    {
        if (target == null)
        {
            target = Player.Instance.gameObject;
        }
    }

    private void Start()
    {
        foreach (var enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.combatZone = this;
            }
        }
    }

    private void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

    }

    private void LateUpdate()
    {
        CheckClear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            isTargetIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            isTargetIn = false;
        }
    }

    private void CheckClear()
    {
        if (isClear)
        {
            return;
        }

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                return;
            }
        }

        isClear = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (isClear)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
        }

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
