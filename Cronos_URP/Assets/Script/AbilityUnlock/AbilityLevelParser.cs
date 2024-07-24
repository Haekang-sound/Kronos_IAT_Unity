using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class AbilityLevelParser
{
    string filepathLevelData = "Assets/Respirces/Text/AbilityLevelData";
    string filepathUserData = "Assets/Respirces/Text/AbilityUserData";

    // Ability Level Data
    readonly string elementId = "ID";
    readonly string elementMaxPoint = "Max_Point";
    readonly string elementNeedPoint = "Need_Point";
    readonly string elementCurrentPoint = "Current_Point";
    readonly string elementNextNodeCondition = "Next_Node_Condition";
    readonly string elementAbilityName = "Ability_Name";
    readonly string elementDescriptionText = "Description_Text";
    readonly string elementChildNodeID = "Child_Node_ID";

    public List<AbilityLevel> LoadLevelDataXML()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        doc.Load(filepathLevelData);

        //루트 설정
        XmlElement nodes = doc["root"];

        List<AbilityLevel> levelDatas = new List<AbilityLevel>();

        //루트에서 요소 꺼내기
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 가져온 데이터 담을 변수
            AbilityLevel readLevel = new AbilityLevel();

            // 요소의 속성을 읽어오기
            readLevel.id = System.Convert.ToInt32(node.GetAttribute(elementId));
            readLevel.maxPoint = System.Convert.ToInt32(node.GetAttribute(elementMaxPoint));
            readLevel.pointNeeded = System.Convert.ToInt32(node.GetAttribute(elementNeedPoint));
            readLevel.nextNodeUnlockCondition = System.Convert.ToInt32(node.GetAttribute(elementNextNodeCondition));
            
            readLevel.childIdNodes = node.GetAttribute(elementChildNodeID).Split(',').Select(int.Parse).ToList<int>();

            readLevel.abilityName = System.Convert.ToString(node.GetAttribute(elementAbilityName));
            readLevel.descriptionText = System.Convert.ToString(node.GetAttribute(elementDescriptionText));

            //가져온 데이터를 리스트에 입력
            levelDatas.Add(readLevel);
        }

        return levelDatas;
    }

    public List<int> LoadUserDataXML()
    {
        // 데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        doc.Load(filepathUserData);

        //루트 설정
        XmlElement nodes = doc["root"];

        List<int> UsertData = new List<int>();

        //루트에서 요소 꺼내기
        foreach (XmlElement node in nodes.ChildNodes)
        {
            int userlevel = System.Convert.ToInt32(node.GetAttribute(elementCurrentPoint));
            UsertData.Add(userlevel);
        }

        return UsertData;
    }

    public void SaveUserDataXML(List<AbilityLevel> abilityData)
    {
        //데이터를 형성할 문서 생성
        XmlDocument doc = new XmlDocument();

        //root 요소 생성 및 문서에 등록
        XmlElement rootElement = doc.CreateElement("root");
        doc.AppendChild(rootElement);

        //기존의 데이터를 기반으로 제작
        foreach (AbilityLevel data in abilityData)
        {
            //속성을 담아줄 요소를 생성
            XmlElement element = doc.CreateElement("Level");

            //속성을 만들어 요소에 등록
            element.SetAttribute(elementCurrentPoint, data.currentPoint.ToString());

            //root의 일부로 요소를 등록
            rootElement.AppendChild(element);
        }

        //데이터가 저장된 문서를 파일위치에 저장
        //덮어씌우기 가능.
        doc.Save(filepathUserData);
    }
}