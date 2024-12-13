using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가중치를 기반으로 랜덤하게 자식 노드를 선택하여 실행하는 노드입니다.
/// </summary>
public class WeightedRandomSelector : CompositeNode
{
    public List<float> weights = new List<float>();
    private float randomValue;

    protected override void OnStart()
    {
        float totalWeight = 0;
        // 총합 가중치 계산
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        // 랜덤 값을 생성
        randomValue = Random.Range(0f, totalWeight + 1f);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // weigiht 값이 childern 보다 적다면
        // weight 값을 가진 childern 만 실행
        int totalIndexCount = children.Count;
        if (weights.Count < children.Count)
        {
            totalIndexCount = weights.Count;
        }

        float cumulativeWeight = 0f;

        // 랜덤 값에 따라 자식 노드 선택
        for (int i = 0; i < totalIndexCount; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                // 선택된 자식 노드 실행
                return children[i].Update();
            }
        }

        return State.Failure;
    }

    private void TotalWeight()
    {

    }
}