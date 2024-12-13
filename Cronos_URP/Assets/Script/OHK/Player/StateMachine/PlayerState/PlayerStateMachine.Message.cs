using UnityEngine;

/// <summary>
/// 애니메이터에서 사용하는 파라메터들을 
/// 해쉬로 보관하는 클래스
/// </summary>
public class PlayerHashSet
{
	protected static PlayerHashSet instance;

	public static PlayerHashSet Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			instance = new PlayerHashSet();
			return instance;
		}
	}

	/// Bool
	public int IsMove { get; set; }
	public int NextCombo { get; set; }
	public int IsGuard { get; set; }
	public int IsFalling { get; set; }
	public int IsLockOn { get; set; }
	public int ChargeAttack { get; set; }

	/// Ability
	public int ComAttackBan { get; set; }
	public int NorAttackBan { get; set; }
	public int NorAttackVariation { get; set; }
	public int ComAttackVariation { get; set; }
	public int DodgeAttack { get; set; }
	public int EnforcedCombo { get; set; }
	public int IsTimeStop { get; set; }
	public int IsCPBoomb { get; set; }
	public int IsFlashSlash { get; set; }
	public int IsTimeSlash { get; set; }
	public int IsRushAttack { get; set; }
	public int IsParry { get; set; }

	/// Trigger
	public int GoIdle { get; set; }
	public int Attack { get; set; }
	public int Rattack { get; set; }
	public int Dodge { get; set; }
	public int TimeStop { get; set; }
	public int CPBoomb { get; set; }
	public int CombMove { get; set; }
	public int FlashSlash { get; set; }
	public int ParryAttack { get; set; }
	public int TimeSlash { get; set; }
	public int TimeSlashReady { get; set; }
	public int RushAttack { get; set; }
	public int DamagedA { get; set; }
	public int DamagedB { get; set; }
	public int CombAttack { get; set; }
	public int Down { get; set; }
	public int Death { get; set; }
	public int Respawn { get; set; }

	/// Value
	public int MoveSpeed { get; set; }
	public int SideWalk { get; set; }
	public int Charge { get; set; }
	public int MoveX { get; set; }
	public int MoveY { get; set; }
	public int AnimSpeed { get; set; }

	public PlayerHashSet()
	{
		IsMove = Animator.StringToHash("isMove");
		NextCombo = Animator.StringToHash("NextCombo");
		IsGuard = Animator.StringToHash("isGuard");
		IsFalling = Animator.StringToHash("isFalling");
		IsLockOn = Animator.StringToHash("isLockOn");
		ChargeAttack = Animator.StringToHash("chargeAttack");

		ComAttackBan = Animator.StringToHash("ComAttackBan");
		NorAttackBan = Animator.StringToHash("NorAttackBan");
		NorAttackVariation = Animator.StringToHash("NorAttackVariation");
		ComAttackVariation = Animator.StringToHash("ComAttackVariation");
		DodgeAttack = Animator.StringToHash("DodgeAttack");
		EnforcedCombo = Animator.StringToHash("EnforcedCombo");
		IsTimeStop = Animator.StringToHash("isTimeStop");
		IsCPBoomb = Animator.StringToHash("isCPBoomb");
		IsFlashSlash = Animator.StringToHash("isFlashSlash");
		IsTimeSlash = Animator.StringToHash("isTimeSlash");
		IsRushAttack = Animator.StringToHash("isRushAttack");
		IsParry = Animator.StringToHash("isParry");

		GoIdle = Animator.StringToHash("goIdle");
		Attack = Animator.StringToHash("Attack");
		Rattack = Animator.StringToHash("Rattack");
		Dodge = Animator.StringToHash("Dodge");
		TimeStop = Animator.StringToHash("TimeStop");
		CPBoomb = Animator.StringToHash("CPBoomb");
		CombMove = Animator.StringToHash("combMove");
		FlashSlash = Animator.StringToHash("FlashSlash");
		ParryAttack = Animator.StringToHash("ParryAttack");
		TimeSlash = Animator.StringToHash("TimeSlash");
		TimeSlashReady = Animator.StringToHash("TimeSlashReady");
		RushAttack = Animator.StringToHash("RushAttack");
		DamagedA = Animator.StringToHash("damagedA");
		DamagedB = Animator.StringToHash("damagedB");
		CombAttack = Animator.StringToHash("combAttack");
		Down = Animator.StringToHash("down");
		Death = Animator.StringToHash("Death");
		Respawn = Animator.StringToHash("Respawn");

		MoveSpeed = Animator.StringToHash("MoveSpeed");
		SideWalk = Animator.StringToHash("SideWalk");
		Charge = Animator.StringToHash("Charge");
		MoveX = Animator.StringToHash("moveX");
		MoveY = Animator.StringToHash("moveY");
		AnimSpeed = Animator.StringToHash("animSpeed");
	}
}
