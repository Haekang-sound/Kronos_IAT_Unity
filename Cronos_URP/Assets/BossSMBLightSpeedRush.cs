using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BossSMBLightSpeedRush : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ResetAiming();

        /// TEMP
        var rb = _monoBehaviour.GetComponent<Rigidbody>();
        float thrust = 1.0f;
        rb.AddForce(0, thrust, 0, ForceMode.Impulse);

        // 오브젝트의 앞 방향으로 이동
        float moveSpeed = 5f; // 이동 속도
        _monoBehaviour.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

    }
}
