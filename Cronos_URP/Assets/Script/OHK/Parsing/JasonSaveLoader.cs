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
		// StreamReader를 사용하여 BOM을 무시하고 파일을 UTF-8로 읽기
		string filePath = Application.dataPath + "/output/data.json";
		string jsonData;

		// UTF-8 인코딩을 사용하여 StreamReader 초기화
		using (StreamReader reader = new StreamReader(filePath, new UTF8Encoding(false)))
		{
			jsonData = reader.ReadToEnd();
		}

		// JSON 데이터를 객체 리스트로 디시리얼라이즈
		nameValues = JsonConvert.DeserializeObject<List<NameValue>>(jsonData);

		// 리스트의 모든 객체 출력
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
		[JsonProperty("지역이름")]
		public string name;

		[JsonProperty("값")]
		public int value;

		public void Print()
		{
			Debug.Log(name);
			Debug.Log(value);
		}
	}
}
