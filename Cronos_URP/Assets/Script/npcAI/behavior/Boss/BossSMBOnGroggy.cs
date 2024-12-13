using UnityEngine;

/// <summary>
/// 보스의 무력화 상태 전환을 관리하는 클래스 입니다.
/// </summary>
public class BossSMBOnGroggy : SceneLinkedSMB<BossBehavior>
{

    public float groggyTime = 8f;
    private float _timer;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = 0f;
        _monoBehaviour.thisRigidbody.isKinematic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;
        if (_timer > groggyTime)
        {
            _monoBehaviour.EndGroggy();
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = false;
    }
}
