using UnityEngine;


/// <summary>
/// PlayerPrefs에 저장된 데이터를 삭제하는 클래스 입니다.
/// </summary>
public class DeleteData : MonoBehaviour
{
	public void DeleteDataAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
