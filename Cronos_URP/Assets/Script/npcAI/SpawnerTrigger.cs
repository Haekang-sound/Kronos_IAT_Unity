using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public List<SpawnEnemyWave> spawners;

    private GameObject player;
    private bool isActiveOnce;

    private void Start()
    {
        player = Player.Instance.gameObject;

        foreach (var spawner in spawners)
        {
            spawner.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActiveOnce)
            return;

        isActiveOnce = false;

        if (other.gameObject == player)
        {
            foreach (var spawner in spawners)
            {
                spawner.enabled = true;
            }
        }
    }
}
