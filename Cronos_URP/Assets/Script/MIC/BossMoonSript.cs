using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoonSript : MonoBehaviour
{
    public float fallSpeed = 0.1f;
    float elapsedTime = 0.0f;
    bool fall = false;

    void Start()
    {
        Invoke("MoonFall", 1.0f);
        Destroy(gameObject, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fall && transform.position.y > 0.5f)
        {
            elapsedTime += Time.deltaTime;
            transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
        }
    }

    public void MoonFall()
    {
        fall = true;
    }
}
