using UnityEngine;

/// <summary>
/// 닿은 적들을 다운 상태로 만드는 스크립트
/// </summary>
[RequireComponent (typeof(Collider))]
public class GroundDowner : MonoBehaviour
{
    private LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        targetLayer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<CombatZoneEnemy>();

        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            enemy.OnDown.Invoke();
        }
    }
}
