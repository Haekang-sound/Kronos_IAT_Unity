using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 전투 구역을 관리하는 클래스입니다. 이 클래스는 특정 구역 안에 적들이 존재하며, 
/// 플레이어가 그 구역에 들어오면 전투가 시작되고, 모든 적을 처치하면 구역이 클리어된 것으로 간주됩니다.
/// </summary>
public class CombatZone : MonoBehaviour
{
    public bool drawGizmos;

    public bool isClear;
    public UnityEvent onClear;
    public GameObject target;
    public CombatZoneEnemy[] enemyList;

    private bool _isTargetIn;
    public GameObject Detect(Transform detector, bool useHeightDifference = true)
    {
        if (_isTargetIn)
        {
            return target;
        }

        return null;
    }

    private void Awake()
    {
        if (target == null)
        {

            if (Player.Instance != null)
            {
                target = Player.Instance.gameObject;
            }
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

    private void Update()
    {
        CheckClear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            _isTargetIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            _isTargetIn = false;
        }
    }

    // -----

    public void DebugKIllAll()
    {
        foreach(var enemy in enemyList)
        {
            if(enemy != null)
            {
                enemy.GetComponent<Damageable>().OnDeath.Invoke();
                enemy.GetComponent<ReplaceWithRagdoll>().Replace();
                enemy.GetComponent<EnemyController>().Release();
            }
        }
    }

    // -----

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
        onClear.Invoke();
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
