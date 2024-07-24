using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
	// singleton 을 위한 구간
	public static SkillTree instance;
	private void Awake() => instance = this;

	// 스킬정보
	public int[] skillLeveles;  // 현재
								// 스킬레벨
	public int[] skillCaps;		// 스킬상한
	public string [] skillNames;// 스킬명
	public string [] SkillDescriptions;	// 스킬설명

	public List<Skill> SkillList;	// 스킬을 받아놓는 리스트
	public GameObject SkillHolder;	// 스킬오브젝트들의 부모오브젝트 

	public List<GameObject> ConnectorList;	// 스킬끼리 연결되는 커넥터를 담아두는 리스트
	public GameObject ConnectorHolder;	// 커넥터오브젝트들의 부모오브젝트

	public int skillPoint; // 스킬을 올릴때 사용하는 포인트

	void OnEnable()
	{
		skillPoint = 20;

		skillLeveles = new int[6]; // 현재 스킬의 레벨
		skillCaps = new int[] { 1,  5, 5, 2, 10, 10}; // 스킬레벨상한

		// 스킬명 미리 갖다 박아놈
		skillNames = new[] { "Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Booster 5", "Booster 6" };

		//스킬설명도 갖다 박아놈
		SkillDescriptions = new[]
		{
			"Dose a thing",
			"Dose a cool thing",
			"Dose a really cool thing ",
			"Dose an awesome thing",
			"Dose this math thing",
			"Dose this compounding thing",
		};

		// 스킬홀더에서 스킬을 하나씩 봅아서 
		foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>())
		{
			// 리스트에 올려준다.
			SkillList.Add(skill);
		}

		// 커넥터 홀더에서 커넥터들을 싹다 뽑아서
		foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
		{
			// 커넥터리스트에 올려준다.
			ConnectorList.Add(connector.gameObject);
		}

		// 스킬마다 id를 박아준다.
		for (var i = 0; i< SkillList.Count; i++) 
		{
			SkillList[i].id = i;
		}

		// 연계되는 스킬을 명시적으로 지정한다.
		SkillList[0].ConnectedSkills = new[] { 1, 2, 3 };
		SkillList[2].ConnectedSkills = new[] { 4,5};

		UpdateAllskillUI();

	}

	// 스킬 ui를 업데이트 하자.
	public void UpdateAllskillUI()
	{
		// 스킬리스트에 들어있는 스킬들을 각각 update 해준다.
		foreach (var skill in SkillList)
		{
			skill.UpdateUI();
		}
	}


}
