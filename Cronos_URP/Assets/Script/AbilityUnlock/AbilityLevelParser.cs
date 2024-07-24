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
        //�����͸� ������ ���� ���� �� �����б�
        XmlDocument doc = new XmlDocument();
        doc.Load(filepathLevelData);

        //��Ʈ ����
        XmlElement nodes = doc["root"];

        List<AbilityLevel> levelDatas = new List<AbilityLevel>();

        //��Ʈ���� ��� ������
        foreach (XmlElement node in nodes.ChildNodes)
        {
            // ������ ������ ���� ����
            AbilityLevel readLevel = new AbilityLevel();

            // ����� �Ӽ��� �о����
            readLevel.id = System.Convert.ToInt32(node.GetAttribute(elementId));
            readLevel.maxPoint = System.Convert.ToInt32(node.GetAttribute(elementMaxPoint));
            readLevel.pointNeeded = System.Convert.ToInt32(node.GetAttribute(elementNeedPoint));
            readLevel.nextNodeUnlockCondition = System.Convert.ToInt32(node.GetAttribute(elementNextNodeCondition));
            
            readLevel.childIdNodes = node.GetAttribute(elementChildNodeID).Split(',').Select(int.Parse).ToList<int>();

            readLevel.abilityName = System.Convert.ToString(node.GetAttribute(elementAbilityName));
            readLevel.descriptionText = System.Convert.ToString(node.GetAttribute(elementDescriptionText));

            //������ �����͸� ����Ʈ�� �Է�
            levelDatas.Add(readLevel);
        }

        return levelDatas;
    }

    public List<int> LoadUserDataXML()
    {
        // �����͸� ������ ���� ���� �� �����б�
        XmlDocument doc = new XmlDocument();
        doc.Load(filepathUserData);

        //��Ʈ ����
        XmlElement nodes = doc["root"];

        List<int> UsertData = new List<int>();

        //��Ʈ���� ��� ������
        foreach (XmlElement node in nodes.ChildNodes)
        {
            int userlevel = System.Convert.ToInt32(node.GetAttribute(elementCurrentPoint));
            UsertData.Add(userlevel);
        }

        return UsertData;
    }

    public void SaveUserDataXML(List<AbilityLevel> abilityData)
    {
        //�����͸� ������ ���� ����
        XmlDocument doc = new XmlDocument();

        //root ��� ���� �� ������ ���
        XmlElement rootElement = doc.CreateElement("root");
        doc.AppendChild(rootElement);

        //������ �����͸� ������� ����
        foreach (AbilityLevel data in abilityData)
        {
            //�Ӽ��� ����� ��Ҹ� ����
            XmlElement element = doc.CreateElement("Level");

            //�Ӽ��� ����� ��ҿ� ���
            element.SetAttribute(elementCurrentPoint, data.currentPoint.ToString());

            //root�� �Ϻη� ��Ҹ� ���
            rootElement.AppendChild(element);
        }

        //�����Ͱ� ����� ������ ������ġ�� ����
        //������ ����.
        doc.Save(filepathUserData);
    }
}