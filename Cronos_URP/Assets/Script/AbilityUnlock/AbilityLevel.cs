using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityLevel
{
    [SerializeField] public int id;
    [SerializeField] public int maxPoint = 1;
    [SerializeField] public int currentPoint;
    [SerializeField] public int pointNeeded = 1;
    [SerializeField] public int nextNodeUnlockCondition = 1;
    [SerializeField] public List<int> childIdNodes = new List<int>();

    [SerializeField] public string abilityName;
    [SerializeField] public string descriptionText;

    public bool IsNextNodeUnlock()
    {
        return currentPoint >= nextNodeUnlockCondition;
    }

    public float GetPercentageOFNextNodeUnlock()
    {
        return Mathf.Min(currentPoint / nextNodeUnlockCondition, 1f);
    }
}