using UnityEngine;

public class FireFloorMaker : MonoBehaviour
{
    public float rayMaxDist = 10.0f;
    public float floorDestroyTime = 5.0f;
    public float fireTerm = 0.2f;
    //public float testHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FireFloor", 0.0f, fireTerm);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireFloor()
    {
        Debug.Log("불장판 까는 중");
        Debug.DrawRay(transform.position, Vector3.down * rayMaxDist, Color.white);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayMaxDist, LayerMask.GetMask("Ground")))
        {
            Vector3 hitPoint = hit.point;

            GameObject fFloor = EffectManager.Instance.SpawnEffect("BossFX_FireFloor", hitPoint);
            //fFloor.transform.position += new Vector3(0, testHeight, 0);
            fFloor.transform.forward = transform.forward;
            Destroy(fFloor, floorDestroyTime);
        }
    }
}
