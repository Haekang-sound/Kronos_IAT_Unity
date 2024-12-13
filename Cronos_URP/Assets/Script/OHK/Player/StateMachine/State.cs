
/// <summary>
/// 모든 stateMachine의 기반이되는 SateMachine 클래스
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