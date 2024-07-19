using UnityEngine;
public class MeleeWeapon : MonoBehaviour
{
    private SimpleDamager _damager;

    private void Awake()
    {
        _damager = GetComponentInChildren<SimpleDamager>();
    }

    public void BeginAttack()
    {
        _damager.BeginAttack();
        _damager.gameObject.SetActive(true);
    }

    public void EndAttack()
    {
        _damager.gameObject.SetActive(false);
        _damager.EndAttack();
    }
}
