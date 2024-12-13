using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ĳ���Ͱ� ���� Ƚ�� �̻� ������ ���� ��� '�׷α�' ���·� ��ȯ�Ǵ� 
/// ���� �ý����� �����ϴ� Ŭ�����Դϴ�.
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
			// ���⼭ ��ٴ�.
            _isGroggy = true;
            SoundManager.Instance.PlaySFX("Boss_Groggy_Sound_SE", transform);
        }
    }

}
