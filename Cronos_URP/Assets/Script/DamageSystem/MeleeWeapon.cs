using UnityEngine;
public class MeleeWeapon : MonoBehaviour
{
    public SimpleDamager simpleDamager;
    public ParryDamager parryDamaer;

    private void Awake()
    {
        simpleDamager = GetComponentInChildren<SimpleDamager>();
        parryDamaer = GetComponentInChildren<ParryDamager>();

        simpleDamager?.gameObject.SetActive(false);
    }

    public void SetOwner(GameObject owner) => simpleDamager?.SetOwner(owner);

    public void BeginAttack()
    {
        simpleDamager?.gameObject.SetActive(true);
        simpleDamager?.BeginAttack();
    }

    public void EndAttack()
    {
        simpleDamager?.EndAttack();
        simpleDamager?.gameObject.SetActive(false);
    }

    public void BeginCanBeParried()
    {
        parryDamaer?.BeginBeParried();
    }

    public void EndBeCanParried()
    {
        parryDamaer?.EndBeParried();
    }

}
