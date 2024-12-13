using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 캐릭터가 일정 횟수 이상 공격을 받을 경우 '그로기' 상태로 전환되는 
/// 스택 시스템을 관리하는 클래스입니다.
/// </summary>
public class GroggyStack : MonoBehaviour
{
    public int maxStack;

    [HideInInspector]
    public UnityEvent OnMaxStack;

    [SerializeField]
	public int _currentStack;
    [SerializeField]
    private bool _isGroggy;

    // -----

    public void AddStack()
    {
        if (_isGroggy)
            return;

        _currentStack++;
        Check();
    }

    public void ResetStack()
    {
        _currentStack = 0;
        _isGroggy = false;
    }

    // -----

    private void Check()
    {
        if (_isGroggy == false && _currentStack >= maxStack)
        {
            OnMaxStack?.Invoke();
			// 여기서 잠근다.
            _isGroggy = true;
            SoundManager.Instance.PlaySFX("Boss_Groggy_Sound_SE", transform);
        }
    }

}
