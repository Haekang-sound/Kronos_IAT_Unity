using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestObjSpawner : MonoBehaviour
{
    public GameObject spawningPrefab;
    public float interval = 1.0f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = interval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Spawn();
            timer = interval;
        }
    }

    void Spawn()
    {
        GameObject instance = Instantiate(spawningPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
