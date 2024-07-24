using UnityEngine;

[System.Serializable]
public class AbilityLevel
{
    [SerializeField] public int maxPoint = 1;
    [SerializeField] public int currentPoint;
    [SerializeField] public int pointNeeded = 1;
    [SerializeField] public int nextNodeUnlockCondition = 1;

    [SerializeField] public string abilityName;
    [SerializeField] public string descriptionText;
}
