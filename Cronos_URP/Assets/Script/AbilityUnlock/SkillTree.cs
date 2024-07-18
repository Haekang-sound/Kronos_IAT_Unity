using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
	// singleton �� ���� ����
	public static SkillTree instance;
	private void Awake() => instance = this;

	// ��ų����
	public int[] skillLeveles;  // ����
								// ��ų����
	public int[] skillCaps;		// ��ų����
	public string [] skillNames;// ��ų��
	public string [] SkillDescriptions;	// ��ų����

	public List<Skill> SkillList;	// ��ų�� �޾Ƴ��� ����Ʈ
	public GameObject SkillHolder;	// ��ų������Ʈ���� �θ������Ʈ 

	public List<GameObject> ConnectorList;	// ��ų���� ����Ǵ� Ŀ���͸� ��Ƶδ� ����Ʈ
	public GameObject ConnectorHolder;	// Ŀ���Ϳ�����Ʈ���� �θ������Ʈ

	public int skillPoint; // ��ų�� �ø��� ����ϴ� ����Ʈ

	void OnEnable()
	{
		skillPoint = 20;

		skillLeveles = new int[6]; // ���� ��ų�� ����
		skillCaps = new int[] { 1,  5, 5, 2, 10, 10}; // ��ų��������

		// ��ų�� �̸� ���� �ھƳ�
		skillNames = new[] { "Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Booster 5", "Booster 6" };

		//��ų���� ���� �ھƳ�
		SkillDescriptions = new[]
		{
			"Dose a thing",
			"Dose a cool thing",
			"Dose a really cool thing ",
			"Dose an awesome thing",
			"Dose this math thing",
			"Dose this compounding thing",
		};

		// ��ųȦ������ ��ų�� �ϳ��� ���Ƽ� 
		foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>())
		{
			// ����Ʈ�� �÷��ش�.
			SkillList.Add(skill);
		}

		// Ŀ���� Ȧ������ Ŀ���͵��� �ϴ� �̾Ƽ�
		foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
		{
			// Ŀ���͸���Ʈ�� �÷��ش�.
			ConnectorList.Add(connector.gameObject);
		}

		// ��ų���� id�� �ھ��ش�.
		for (var i = 0; i< SkillList.Count; i++) 
		{
			SkillList[i].id = i;
		}

		// ����Ǵ� ��ų�� ��������� �����Ѵ�.
		SkillList[0].ConnectedSkills = new[] { 1, 2, 3 };
		SkillList[2].ConnectedSkills = new[] { 4,5};

		UpdateAllskillUI();

	}

	// ��ų ui�� ������Ʈ ����.
	public void UpdateAllskillUI()
	{
		// ��ų����Ʈ�� ����ִ� ��ų���� ���� update ���ش�.
		foreach (var skill in SkillList)
		{
			skill.UpdateUI();
		}
	}


}
