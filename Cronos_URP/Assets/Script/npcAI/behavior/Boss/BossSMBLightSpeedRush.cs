using UnityEngine;

public class BossSMBLightSpeedRush : SceneLinkedSMB<BossBehavior>
{
    [SerializeField]
    public float scale = 1f;

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rootmotion = animator.deltaPosition;

        rootmotion *= scale;

        _monoBehaviour.transform.position += rootmotion;
    }

}
