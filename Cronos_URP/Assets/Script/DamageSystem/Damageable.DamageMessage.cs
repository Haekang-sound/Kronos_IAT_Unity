using UnityEngine;

/// <summary>
/// 피해 메시지 및 피해 유형과 콤보 유형을 정의하는 클래스입니다.
/// 이 클래스는 적의 피해 처리 및 콤보 시스템을 관리하는데 사용됩니다.
/// </summary>
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