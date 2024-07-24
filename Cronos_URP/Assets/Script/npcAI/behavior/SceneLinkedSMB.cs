using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// StateMachineBehaviour 를 상속받아 추가 기능성을 더한 클래스.
/// StateMucine을 가진 객체의 모노비헤이버를 불러올 수 있고,
/// Enter / Update /  Exit 이벤트 외에도 애니메이션 트랜지션에 대한 업데이트가 추가되었다.
/// </summary>
/// <typeparam name="TMonoBehaviour">MonoBehaviour를 상속받은 클래스</typeparam>
public class SceneLinkedSMB<TMonoBehaviour> : SealedSMB
        where TMonoBehaviour : MonoBehaviour
{
    protected TMonoBehaviour _monoBehaviour;

    bool _firstFrameHappened;
    bool _lastFrameHappened;

    public static void Initialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        SceneLinkedSMB<TMonoBehaviour>[] sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

        for (int i = 0; i < sceneLinkedSMBs.Length; i++)
        {
            sceneLinkedSMBs[i].InternalInitialise(animator, monoBehaviour);
        }
    }

    protected void InternalInitialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        OnStart(animator);
    }

    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _firstFrameHappened = false;

        OnSLStateEnter(animator, stateInfo, layerIndex);
        OnSLStateEnter(animator, stateInfo, layerIndex, controller);
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (!animator.gameObject.activeSelf)
            return;

        if (animator.IsInTransition(layerIndex) && animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && _firstFrameHappened)
        {
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex) && !_lastFrameHappened && _firstFrameHappened)
        {
            _lastFrameHappened = true;

            OnSLStatePreExit(animator, stateInfo, layerIndex);
            OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && !_firstFrameHappened)
        {
            _firstFrameHappened = true;

            OnSLStatePostEnter(animator, stateInfo, layerIndex);
            OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
        }
    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        _lastFrameHappened = false;

        OnSLStateExit(animator, stateInfo, layerIndex);
        OnSLStateExit(animator, stateInfo, layerIndex, controller);
    }

    /// <summary>
    ///  씬 내 Monobehviour 가 시작 함수를 호출할 때 같이 호출.
    /// </summary>
    public virtual void OnStart(Animator animator) { }

    /// ----------------------------------------------------------------------------------------------------------------------------------------
    /// 트렌지션 -> 스테이트 
    /// ----------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 스테이트의 실행이 처음 시작될 때(트랜지션 -> 스테이트 시) Update 이전에 호출.
    /// </summary>
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// 트렌지션 -> 스테이트 중에 매 프레임마다 호출. OnSLStateEnter 이후에 호출.
    /// </summary>
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// 트렌지션 -> 스테이트 후 첫 번째 프레임에 호출.
    /// </summary>
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// ----------------------------------------------------------------------------------------------------------------------------------------
    /// Update
    /// ----------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 스테이트가 트렌지션 되지 않을 때(트렌지션 중이 아닐 때) 매 프레임마다 호출됨.
    /// </summary>
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// ----------------------------------------------------------------------------------------------------------------------------------------
    /// 스테이트 -> 트렌지션
    /// ----------------------------------------------------------------------------------------------------------------------------------------

    /// 스테이트 -> 트렌지션이 시작된 첫 번째 프레임에 호출됩. 전환의 지속 시간이 1 프레임보다 짧으면 호출되지 않음.
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// 스테이트 -> 트렌지션 중에 매 프레임마다 OnSLStatePreExit 이후에 호출.
    /// </summary>
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after Updates when execution of the state first finshes (after transition from the state).
    /// 현재 스테이트에서 나가는 트렌지션이 완료될 때 호출.
    /// </summary>
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }
}

// 실행 중인 객체에 대한 직접 참조히할 수 있도록 한다.
// 상태가 실행 중인 객체를 직접 참조할 수 있는 가능성을 추가하여 매번 GetComponent를 통해 가져오는 비용을 피할 수 있다.
public abstract class SealedSMB : StateMachineBehaviour
{
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
