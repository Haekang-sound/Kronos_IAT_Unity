using UnityEngine;

class TestCombatZoneEnemySMBDamaged : SceneLinkedSMB<TestCombatZoneEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.SetBulletTimeScalable(false);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.SetBulletTimeScalable(true);
    }
}
