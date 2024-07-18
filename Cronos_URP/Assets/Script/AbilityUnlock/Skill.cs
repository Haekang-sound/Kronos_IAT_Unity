using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Skill : MonoBehaviour
{
	// ��ų id
	public int id;

	CinemachineVirtualCamera virtualCamera;

	public TMP_Text TitleText; // ��ų��
	public TMP_Text DescriptionText; // ��ų����

	public int[] ConnectedSkills; // ������ų

	public void UpdateUI()
	{
		// ��ų�̸�
		TitleText.text = $"{SkillTree.instance.skillLeveles[id]}/{SkillTree.instance.skillCaps[id]}" +
						$"\n{SkillTree.instance.skillNames[id]}";

		// ��ų����
		DescriptionText.text = $"{SkillTree.instance.SkillDescriptions[id]}\nCost : {SkillTree.instance.skillPoint}/1 sp";

		// skill object�� �ִ� image ������Ʈ�� �����ͼ� ���ǿ� ���� ������ �ٲ��ش�.
		GetComponent<Image>().color = SkillTree.instance.skillLeveles[id] >= SkillTree.instance.skillCaps[id] ? Color.yellow // ��ų ������ ��ų���ѷ������� Ŭ ��� ����� �׷��� �������
			: SkillTree.instance.skillPoint >= 1 ? Color.green : Color.white;   // ��ų����Ʈ�� 1���� ������ �ʷϻ� �׷��� ������ �Ͼ��
		
		virtualCamera = GetComponent<CinemachineVirtualCamera>();

		// ������ų�� ���ƺ���.
		foreach(var connectedSkill  in ConnectedSkills)
		{
			// ������ų�� Ȱ��ȭ�Ѵ�.skill level�� 0���� ũ�ٸ�
			SkillTree.instance.SkillList[connectedSkill].gameObject.SetActive(SkillTree.instance.skillLeveles[id] > 0) ;
			// Ŀ���͵� ���� Ȱ��ȭ ��Ų��.
			SkillTree.instance.ConnectorList[connectedSkill].SetActive(SkillTree.instance.skillLeveles[id] > 0) ;
		}

		
	}

	// ��ų�� ���������� �Ͼ�� �ϵ�

	public void Buy()
	{
		if (SkillTree.instance.skillPoint < 1 || SkillTree.instance.skillLeveles[id] >= SkillTree.instance.skillCaps[id])
		{
			return;
		}
		SkillTree.instance.skillPoint -= 1;
		SkillTree.instance.skillLeveles[id]++;
		SkillTree.instance.UpdateAllskillUI();
		CinemachineBrain.SoloCamera = virtualCamera;

	}

}

