using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.Events;

public class BossLevelTrigger : MonoBehaviour
{
    public GameObject[] colliders;
    public UnityEvent onEvent;

    private void Start()
    {
        foreach (GameObject c in colliders)
        {
            c.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Player.Instance.gameObject) return;

        foreach (GameObject c in colliders)
        {
            c.SetActive(true);
        }

        onEvent?.Invoke();

        gameObject.SetActive(false);
    }
}
