using UnityEngine;

public class LevelDoorDamagerbleTrigger : MonoBehaviour
{
    public Damageable damageable;
    public GameObject timeline;

    void Awake()
    {
        if (timeline != null)
        {
            damageable?.OnDeath.AddListener(() => timeline.SetActive(true));
        }
    }

    private void OnDestroy()
    {
    }
}
