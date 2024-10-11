using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class JasonSaveLoader : MonoBehaviour
{
	public static List<InGameText> LoadingTexts;
	public static List<InGameText> SceneTexts;
	public static List<InGameText> QuestTexts;


	// Start is called before the first frame update
	private void Awake()
	{
		Initialize();
	}
	public void Initialize()
	{
		// StreamReader를 사용하여 BOM을 무시하고 파일을 UTF-8로 읽기
		string ScenefilePath = Application.dataPath + "/output/SceneName.json";
		string LoadingfilePath = Application.dataPath + "/output/LoadingText.json";
		string QuestfilePath = Application.dataPath + "/output/Quest.json";

		string LoadingData;
		string SceneData;
		string QuestData;

		// UTF-8 인코딩을 사용하여 StreamReader 초기화
		using (StreamReader reader = new StreamReader(LoadingfilePath, new UTF8Encoding(false)))
		{
			LoadingData = reader.ReadToEnd();
		}
		using (StreamReader reader = new StreamReader(ScenefilePath, new UTF8Encoding(false)))
		{
			SceneData = reader.ReadToEnd();
		}
		using (StreamReader reader = new StreamReader(QuestfilePath, new UTF8Encoding(false)))
		{
			QuestData = reader.ReadToEnd();
		}

		// JSON 데이터를 객체 리스트로 디시리얼라이즈
		QuestTexts = JsonConvert.DeserializeObject<List<InGameText>>(QuestData);
		LoadingTexts = JsonConvert.DeserializeObject<List<InGameText>>(LoadingData);
		SceneTexts = JsonConvert.DeserializeObject<List<InGameText>>(SceneData);
	}


	// Update is called once per frame
	void Update()
	{
	}

	public class InGameText
	{
		[JsonProperty("ID")]
		public int ID;

		[JsonProperty("Index")]
		public int Index;

		[JsonProperty("Text")]
		public string text;

		public void Print()
		{
			Debug.Log(ID);
			Debug.Log(Index);
			Debug.Log(text);
		}
	}
}
