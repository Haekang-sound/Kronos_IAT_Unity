
using UnityEngine;

public class PlayerHashSet
{
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
	protected static PlayerHashSet instance;

	public PlayerHashSet()
	{
		isMove = Animator.StringToHash("isMove");
		NextCombo = Animator.StringToHash("NextCombo");
		isGuard = Animator.StringToHash("isGuard");
		isFalling = Animator.StringToHash("isFalling");
		isLockOn = Animator.StringToHash("isLockOn");
		chargeAttack = Animator.StringToHash("chargeAttack");

		ComAttackBan = Animator.StringToHash("ComAttackBan");
		NorAttackBan = Animator.StringToHash("NorAttackBan");
		NorAttackVariation = Animator.StringToHash("NorAttackVariation");
		ComAttackVariation = Animator.StringToHash("ComAttackVariation");
		DodgeAttack = Animator.StringToHash("DodgeAttack");
		EnforcedCombo = Animator.StringToHash("EnforcedCombo");
		isTimeStop = Animator.StringToHash("isTimeStop");
		isCPBoomb = Animator.StringToHash("isFlashSlash");
		isFlashSlash = Animator.StringToHash("isFlashSlash");
		isTimeSlash = Animator.StringToHash("isTimeSlash");
		isRushAttack = Animator.StringToHash("isRushAttack");
		isParry = Animator.StringToHash("isParry");

		goIdle = Animator.StringToHash("goIdle");
		Attack = Animator.StringToHash("Attack");
		Rattack = Animator.StringToHash("Rattack");
		Dodge = Animator.StringToHash("Dodge");
		TimeStop = Animator.StringToHash("TimeStop");
		CPBoomb = Animator.StringToHash("CPBoomb");
		combMove = Animator.StringToHash("combMove");
		FlashSlash = Animator.StringToHash("FlashSlash");
		ParryAttack = Animator.StringToHash("ParryAttack");
		TimeSlash = Animator.StringToHash("TimeSlash");
		TimeSlashReady = Animator.StringToHash("TimeSlashReady");
		RushAttack = Animator.StringToHash("RushAttack");
		damagedA = Animator.StringToHash("damagedA");
		damagedB = Animator.StringToHash("damagedB");
		combAttack = Animator.StringToHash("combAttack");
		down = Animator.StringToHash("down");

		MoveSpeed = Animator.StringToHash("MoveSpeed");
		SideWalk = Animator.StringToHash("SideWalk");
		Charge = Animator.StringToHash("Charge");
		moveX = Animator.StringToHash("moveX");
		moveY = Animator.StringToHash("moveY");
		animSpeed = Animator.StringToHash("animSpeed");

	}
	/// Bool
	public int isMove { get; set; }
	public int NextCombo { get; set; }
	public int isGuard { get; set; }
	public int isFalling { get; set; }
	public int isLockOn { get; set; }
	public int chargeAttack { get; set; }

	/// Ability
	public int ComAttackBan { get; set; }
	public int NorAttackBan { get; set; }
	public int NorAttackVariation { get; set; }
	public int ComAttackVariation { get; set; }
	public int DodgeAttack { get; set; }
	public int EnforcedCombo { get; set; }
	public int isTimeStop { get; set; }
	public int isCPBoomb { get; set; }
	public int isFlashSlash { get; set; }
	public int isTimeSlash { get; set; }
	public int isRushAttack { get; set; }
	public int isParry { get; set; }

	/// Trigger
	public int goIdle { get; set; }
	public int Attack { get; set; }
	public int Rattack { get; set; }
	public int Dodge { get; set; }
	public int TimeStop { get; set; }
	public int CPBoomb { get; set; }
	public int combMove { get; set; }
	public int FlashSlash { get; set; }
	public int ParryAttack { get; set; }
	public int TimeSlash { get; set; }
	public int TimeSlashReady { get; set; }
	public int RushAttack { get; set; }
	public int damagedA { get; set; }
	public int damagedB { get; set; }
	public int combAttack { get; set; }
	public int down { get; set; }
	/// Value
	public int MoveSpeed { get; set; }
	public int SideWalk { get; set; }
	public int Charge { get; set; }
	public int moveX { get; set; }
	public int moveY { get; set; }
	public int animSpeed { get; set; }

}
