using UnityEngine;

public class BossSMBLightSpeedRush : SceneLinkedSMB<BossBehavior>
{
    [SerializeField]

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var toTarget = _monoBehaviour.target.transform.position - _monoBehaviour.transform.position;
        
        var rootmotion = animator.deltaPosition;

        var _scale = toTarget.magnitude * rootmotion.magnitude;

        rootmotion *= _scale;

        _monoBehaviour.transform.position += rootmotion;
    }

}
