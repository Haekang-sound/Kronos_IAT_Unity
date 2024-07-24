
/// 모든 상태의 기반이 되는 추상클래스 State
public abstract class State
{
	public abstract void Enter();
	public abstract void Tick();
	public abstract void FixedTick();
	public abstract void LateTick();
	public abstract void Exit();
}