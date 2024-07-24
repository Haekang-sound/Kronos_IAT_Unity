using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Skill : MonoBehaviour
{
	// 스킬 id
	public int id;

	CinemachineVirtualCamera virtualCamera;

	public TMP_Text TitleText; // 스킬명
	public TMP_Text DescriptionText; // 스킬설명

	public int[] ConnectedSkills; // 연관스킬

	public void UpdateUI()
	{
		// 스킬이름
		TitleText.text = $"{SkillTree.instance.skillLeveles[id]}/{SkillTree.instance.skillCaps[id]}" +
						$"\n{SkillTree.instance.skillNames[id]}";

		// 스킬설명
		DescriptionText.text = $"{SkillTree.instance.SkillDescriptions[id]}\nCost : {SkillTree.instance.skillPoint}/1 sp";

		// skill object에 있는 image 컴포넌트를 가져와서 조건에 따라 색상을 바꿔준다.
		GetComponent<Image>().color = SkillTree.instance.skillLeveles[id] >= SkillTree.instance.skillCaps[id] ? Color.yellow // 스킬 레벨이 스킬상한레벨보다 클 경우 노란색 그렇지 않을경우
			: SkillTree.instance.skillPoint >= 1 ? Color.green : Color.white;   // 스킬포인트가 1보다 많으면 초록색 그렇지 않으면 하얀색
		
		virtualCamera = GetComponent<CinemachineVirtualCamera>();

		// 연관스킬을 돌아본다.
		foreach(var connectedSkill  in ConnectedSkills)
		{
			// 연관스킬을 활성화한다.skill level이 0보다 크다면
			SkillTree.instance.SkillList[connectedSkill].gameObject.SetActive(SkillTree.instance.skillLeveles[id] > 0) ;
			// 커넥터도 같이 활성화 시킨다.
			SkillTree.instance.ConnectorList[connectedSkill].SetActive(SkillTree.instance.skillLeveles[id] > 0) ;
		}

		
	}

	// 스킬을 구매했을때 일어나는 일들

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

