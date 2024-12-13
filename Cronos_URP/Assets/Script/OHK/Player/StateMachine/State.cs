
/// <summary>
/// ��� stateMachine�� ����̵Ǵ� SateMachine Ŭ����
/// 
/// ohk    v1
/// </summary>
public abstract class State
{
	public abstract void Enter();
	public abstract void Tick();
	public abstract void FixedTick();
	public abstract void LateTick();
	public abstract void Exit();
}