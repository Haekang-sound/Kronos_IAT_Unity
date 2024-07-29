using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParryDamager : SimpleDamager
{
    [SerializeField]
    private bool isParrying;
    public UnityEvent beParryied;

    private void OnTriggerEnter(Collider other)
    {
        DamageCheck(other);
    }

    public void ParryingCheck(Collider other)
    {
        var otherParryDamager = other.GetComponent<ParryDamager>();

        if (otherParryDamager == null ||
            otherParryDamager.isParrying == false)
        {
            return;
        }

        beParryied.Invoke();
    }
}
