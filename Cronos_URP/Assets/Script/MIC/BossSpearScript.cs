using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpearScript : MonoBehaviour
{
    public bool act = false;
    public bool sat = false;
    public Player target;
    public Vector3 targetPos;
    public float lookSpeed;
    public float incSpeed = 0.0f;
    public float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (act)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, lookSpeed);
            targetPos = target.transform.position;
        }

        if (sat)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            if (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
            {
                elapsedTime += Time.deltaTime;
                incSpeed += (elapsedTime * elapsedTime);
                transform.position += dir * incSpeed * Time.deltaTime;
            }
        }

    }

    public IEnumerator Saturate(float delay)
    {
        yield return new WaitForSeconds(delay);

        act = false;
        sat = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Ã¢ ÆÄ±«µÊ");
    }
}
