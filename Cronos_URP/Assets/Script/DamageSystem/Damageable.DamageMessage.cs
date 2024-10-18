using UnityEngine;

public partial class Damageable : MonoBehaviour
{
	public enum ComboType
	{
        None = 0,
		InterruptibleCombo, // enemy가 공격중일때는 피격상태로 넘어가지 않음
		UninterruptibleCombo // 이전과 똑같음
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