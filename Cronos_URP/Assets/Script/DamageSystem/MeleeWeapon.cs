using UnityEngine;
public class MeleeWeapon : MonoBehaviour
{
    public SimpleDamager simpleDamager;
    public ParryDamager parryDamaer;
	public BoxColliderAdjuster bAdjuster;

    private void Awake()
    {
        if (simpleDamager == null )
            simpleDamager = GetComponentInChildren<SimpleDamager>();
        if (parryDamaer == null)
            parryDamaer = GetComponentInChildren<ParryDamager>();
		bAdjuster = GetComponentInChildren<BoxColliderAdjuster>();

		simpleDamager?.gameObject.SetActive(false);
    }

    public void SetOwner(GameObject owner) => simpleDamager?.SetOwner(owner);

    public void BeginAttack()
    {
        simpleDamager?.gameObject.SetActive(true);

        if (parryDamaer)
        {
            parryDamaer.inAttack = true;
        }
    }

    public void EndAttack()
    {
        simpleDamager?.gameObject.SetActive(false);

        if (parryDamaer)
        {
            parryDamaer.inAttack = false;
        }
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
