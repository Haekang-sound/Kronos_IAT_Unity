using UnityEngine;

public partial class Damageable : MonoBehaviour
{
	public enum ComboType
	{
        None = 0,
		ACombo,
		BCombo
	}
	public enum DamageType
    {
        None = 0,
        ATypeHit,
        BTypeHit,
        KockBack,
        Fall,
        OnFallDamaged,
        Down
    }

    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public float amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;
        public bool isActiveSkill;

        public DamageType damageType;
        public ComboType comboType;
    }
}