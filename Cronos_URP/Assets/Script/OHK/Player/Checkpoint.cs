using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public int priority;
    public float healTP;
    public float healCP;

    private bool _isActive;

    public UnityEvent OnActive;

    private void Awake()
    {
		//gameObject.layer = LayerMask.NameToLayer("Checkpoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive == true)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        //player.isDecreaseTP = false;


        OnActive?.Invoke();
        _isActive = true;

        player.SetCheckpoint(this);

        // �÷��̾� TP ���� ����
        //Player.Instance.TP += healTP;
		healTP = Player.Instance.TP;
		healCP = Player.Instance.CP;

        SaveLoadManager.SaveAllData();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isActive == true)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        player.isDecreaseTP = true;

        Debug.Log("�÷��̾��� ü���� �ٽ� �پ��" + Player.Instance.TP);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.75f;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
