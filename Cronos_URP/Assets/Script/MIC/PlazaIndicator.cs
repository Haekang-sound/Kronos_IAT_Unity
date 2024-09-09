using UnityEngine;

public class PlazaIndicator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
    }
}
