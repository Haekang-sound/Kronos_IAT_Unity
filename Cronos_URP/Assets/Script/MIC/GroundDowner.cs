using UnityEngine;

[RequireComponent (typeof(Collider))]
public class GroundDowner : MonoBehaviour
{
    public LayerMask targetLayer;

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
