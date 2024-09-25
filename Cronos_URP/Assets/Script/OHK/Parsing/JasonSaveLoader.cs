using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class JasonSaveLoader : MonoBehaviour
{
	public static List<NameValue> nameValues;
	// Start is called before the first frame update
	void Start()
	{
		// StreamReader�� ����Ͽ� BOM�� �����ϰ� ������ UTF-8�� �б�
		string filePath = Application.dataPath + "/output/data.json";
		string jsonData;

		// UTF-8 ���ڵ��� ����Ͽ� StreamReader �ʱ�ȭ
		using (StreamReader reader = new StreamReader(filePath, new UTF8Encoding(false)))
		{
			jsonData = reader.ReadToEnd();
		}

		// JSON �����͸� ��ü ����Ʈ�� ��ø��������
		nameValues = JsonConvert.DeserializeObject<List<NameValue>>(jsonData);

		// ����Ʈ�� ��� ��ü ���
		foreach (var nameValue in nameValues)
		{
			nameValue.Print();
		}
	}
	

	// Update is called once per frame
	void Update()
	{

	}

	public class NameValue
	{
		[JsonProperty("�����̸�")]
		public string name;

		[JsonProperty("��")]
		public int value;

		public void Print()
		{
			Debug.Log(name);
			Debug.Log(value);
		}
	}
}
