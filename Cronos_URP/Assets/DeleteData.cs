using UnityEngine;


/// <summary>
/// PlayerPrefs�� ����� �����͸� �����ϴ� Ŭ���� �Դϴ�.
/// </summary>
public class DeleteData : MonoBehaviour
{
	public void DeleteDataAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
