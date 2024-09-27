using UnityEngine;
using UnityEngine.Events;

public class GroggyStack : MonoBehaviour
{
    public int maxStack;

    public UnityEvent OnMaxStack;

    [SerializeField]
    private int _currentStack;
    [SerializeField]
    private bool _isGroggy;

    // =====

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
            OnMaxStack.Invoke();
            _isGroggy = true;
        }
    }

}
