using UnityEngine;

public class LevelDoorDamagerbleTrigger : MonoBehaviour
{
    public Damageable damageable;

    void Awake()
    {
        var doorCtrl = GetComponent<DoorController>();
        if (doorCtrl != null)
        {
            damageable?.OnDeath.AddListener(doorCtrl.PlayTimeline);
        }
    }

    private void OnDestroy()
    {
    }
}
