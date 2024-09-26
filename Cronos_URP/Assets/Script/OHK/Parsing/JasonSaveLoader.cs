using System.Collections;
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
	void Start()
	{
		// StreamReader�� ����Ͽ� BOM�� �����ϰ� ������ UTF-8�� �б�
		string ScenefilePath = Application.dataPath + "/output/SceneName.json";
		string LoadingfilePath = Application.dataPath + "/output/LoadingText.json";
		string QuestfilePath = Application.dataPath + "/output/Quest.json";

		string LoadingData;
		string SceneData;
		string QuestData;

		// UTF-8 ���ڵ��� ����Ͽ� StreamReader �ʱ�ȭ
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

		// JSON �����͸� ��ü ����Ʈ�� ��ø��������
		QuestTexts = JsonConvert.DeserializeObject<List<InGameText>>(QuestData);
		LoadingTexts = JsonConvert.DeserializeObject<List<InGameText>>(LoadingData);
		SceneTexts = JsonConvert.DeserializeObject<List<InGameText>>(SceneData);
	}


	// Update is called once per frame
	void Update()
	{
		Debug.Log(JasonSaveLoader.LoadingTexts[0].text);
		Debug.Log(JasonSaveLoader.SceneTexts[0].text);
		Debug.Log(JasonSaveLoader.QuestTexts[0].text);
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
