using UnityEngine;
using UnityEngine.Events;

public class GroggyStack : MonoBehaviour
{
    public int maxStack;

    public UnityEvent OnMaxStack;

    [SerializeField]
    private int _currentStack;

    // =====

    public void AddStack()
    {
        _currentStack++;
        Check();
    }

    public void ResetStack()
    {
        _currentStack = 0;
    }

    // -----

    private void Check()
    {
        if (_currentStack >= maxStack)
        {
            ResetStack();
            OnMaxStack.Invoke();
        }
    }

}
