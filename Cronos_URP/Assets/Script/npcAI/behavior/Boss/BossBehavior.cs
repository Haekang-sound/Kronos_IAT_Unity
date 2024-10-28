using Message;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BossBehavior : MonoBehaviour, IMessageReceiver
{
    public bool drawGizmos;

    [Header("Behavior Tree")]
    public BehaviorTree phaseOne;
    public BehaviorTree phaseTwo;
    public BehaviorTree phaseTree;

    [Header("Phase 3 Option")]
    public float timeStopInvaliDelay = 3f;
    public UnityEvent TimeStopInvalidate;

    [Header("Damager")]
    public GameObject shoulderDamager;
    public GameObject impactDamager;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public EnemyController controller;

    private HitShake _hitShake;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Damageable _damageable;
    private GroggyStack _groggyStack;
    private MeleeWeapon _meleeWeapon;
    private EffectManager _effectManager;
    private PlayableDirector _playableDirector;
    private BehaviorTreeRunner _behaviortreeRunner;
    private BulletTimeScalable _bulletTimeScalable;

    private bool _onPhaseOne;
    private bool _onPhaseTwo;
    private bool _onPhaseTree;

    private Blackboard _blackboard;
    private readonly float _rotationSpeed = 18f;


    void Awake()
    {
        _blackboard = new Blackboard();

        controller = GetComponent<EnemyController>();

        _hitShake = GetComponent<HitShake>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _damageable = GetComponent<Damageable>();
        _groggyStack = GetComponent<GroggyStack>();
        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        _effectManager = EffectManager.Instance;
        _playableDirector = GetComponent<PlayableDirector>();
        _behaviortreeRunner = GetComponent<BehaviorTreeRunner>();
        _bulletTimeScalable = GetComponent<BulletTimeScalable>();

        if (target == null)
            target = Player.Instance.gameObject;
    }

    //void Start()
    //{
    //}

    void OnEnable()
    {
        shoulderDamager.SetActive(false);
        impactDamager.SetActive(false);

        SceneLinkedSMB<BossBehavior>.Initialise(_animator, this);

        _blackboard.target = target;
        _blackboard.bulletTimeScalable = _bulletTimeScalable;

        UseGravity(true);

        controller.SetFollowNavmeshAgent(false);
        controller.UseNavemeshAgentRotation(true);

        _damageable.onDamageMessageReceivers.Add(this);

        _groggyStack.OnMaxStack.AddListener(BeginGroggy);

        // For Test
        if (_behaviortreeRunner.tree != null)
        {
            _behaviortreeRunner.tree.blackboard = _blackboard;
            _behaviortreeRunner.Bind();
        }

        BulletTime.Instance.OnActive.AddListener(InvalidateBulletTime);
        BulletTime.Instance.OnNormalrize.AddListener(() => _bulletTimeScalable.SetActive(true));
    }

    //private void OnDisable()
    //{
    //}

    //private void OnDisable()
    //{
    //}

    //void OnDestroy()
    //{
    //}

    private bool _aimtarget;

    void Update()
    {
        UpdateBehaviorTree();

        if (_aimtarget)
        {
            LookAtTarget();
        }
    }

    //void FixedUpdate()
    //{
    //}


    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        var dmgMsg = (Damageable.DamageMessage)data;

        switch (type)
        {
            case MessageType.DAMAGED:
				EffectManager.Instance.CreateHitFX(dmgMsg, transform);
				Player.Instance.ChargeCP(dmgMsg.isActiveSkill);
                break;
            case MessageType.DEAD:
				Player.Instance.ChargeCP(dmgMsg.isActiveSkill);
				break;
            case MessageType.RESPAWN:
                break;
            default:
                return;

        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    public void BossEightBeamCoroutine()
    {
        StartCoroutine(_effectManager?.BossEightBeamCoroutine(transform));
    }

    public void BossFireShoot()
    {
        _effectManager?.BossFireShoot(transform);
    }

    public void BossFiveSpear()
    {
        _effectManager?.BossFiveSpear(transform);
    }

    public void BossMoon()
    {
        _effectManager?.BossMoon(transform);
    }

    //////////////////////////////////////////////////////////////////////////////////////

    private void UpdateBehaviorTree()
    {
        if (_onPhaseTwo == false && _damageable.GetHealthPercentage() < 70f)
        {
            ChangePhase(phaseTwo);
            _onPhaseTwo = true;
        }

        if (_onPhaseTree == false && _damageable.GetHealthPercentage() < 30f)
        {
            ChangePhase(phaseTree);
            _onPhaseTree = true;
        }
    }

    public void Active()
    {
        ChangePhase(phaseOne);
        _onPhaseOne = true;
    }

    public float GetDeltaTime()
    {
        return _bulletTimeScalable.GetDeltaTime();
    }

    private void LookAtTarget()
    {
        if (target == null) return;

        // 바라보는 방향 설정
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * _bulletTimeScalable.GetDeltaTime());
    }

    public void Strafe(bool isRingth = true)
    {
        if (target == null) return;

        Vector3 offsetPlayer = target.transform.position - transform.position;

        if (isRingth == false)
        {
            offsetPlayer = transform.position - target.transform.position;
        }

        Vector3 direction = Vector3.Cross(offsetPlayer, Vector3.up);
        controller.SetTarget(transform.position + direction.normalized);

        LookAtTarget();
    }
    public void BeginAttack()
    {
        _meleeWeapon.BeginAttack();
    }

    public void EndAttack()
    {
        _meleeWeapon.EndAttack();
    }

    public void BeginSoulderAttack()
    {
        shoulderDamager?.SetActive(true);
    }

    public void EndSoulderAttack()
    {
        shoulderDamager?.SetActive(false);
    }

    public void BeginImpactAttack()
    {
        impactDamager?.SetActive(true);
        UseGravity(false);
    }

    public void EndImpactAttack()
    {
        impactDamager?.SetActive(false);
        UseGravity(true);
    }

    public void UseGravity(bool gravity)
    {
        _rigidbody.useGravity = gravity;
    }

    public void BeginAiming()
    {
        //_rotationSpeed = 100f;
        _aimtarget = true;
    }

    public void StopAiming()
    {
        //_rotationSpeed = 0f;
        _aimtarget = false;
    }

    public bool CheckDistanceWithTarget(float distance)
    {
        Vector3 toTarget = target.transform.position - transform.position;
        return toTarget.sqrMagnitude < distance * distance;
    }

    public void LightSpeedRushUpgrade()
    {
        //GameObject obj = Resources.Load<GameObject>("Models/Boss/LightSpeedRush_Clon");
        GameObject obj = Resources.Load<GameObject>("Prefabs/Boss/LightSpeedRush_Clon");

        var clone1 = Instantiate(obj, transform.position, transform.rotation);
        var clone2 = Instantiate(obj, transform.position, transform.rotation);

        float offset = 3f;
        var right = transform.position + transform.right * offset;
        var left = transform.position - transform.right * offset;

        clone1.GetComponent<MoveObject>().targetPosition = right;
        clone2.GetComponent<MoveObject>().targetPosition = left;

        clone1.GetComponent<BossLightRushCloneBehavior>().activeTime = 2f;
        clone2.GetComponent<BossLightRushCloneBehavior>().activeTime = 2.4f;

        StartCoroutine(RushAfterSeconds(gameObject, 3f));
    }

    public void AnimatorSetTrigger(string triggername)
    {
        _animator.SetTrigger(triggername);
    }

    public void BeginGroggy()
    {
        ResetAllTriggers();
        AnimatorSetTrigger("groggy");
    }

    public void EndGroggy()
    {
        AnimatorSetTrigger("idle");

        _groggyStack.ResetStack();

        StartResetAllTriggers();

        var currentHP = _damageable.GetHealthPercentage();
        if (currentHP > 70f)
        {
            StartCoroutine(ChangePhaseAfterDelay(phaseOne, 1.25f));
            _onPhaseOne = true;
        }
        else if (_onPhaseTwo == false && currentHP > 30f)
        {
            StartCoroutine(ChangePhaseAfterDelay(phaseTwo, 1.25f));
            _onPhaseTwo = true;
        }
        else //if (_onPhaseTree == false && currentHP <= 30f)
        {
            StartCoroutine(ChangePhaseAfterDelay(phaseTree, 1.25f));
            _onPhaseTree = true;
        }

    }

    public void PlayBT(bool paly)
    {
        _behaviortreeRunner.play = paly;
    }

    public void Death()
    {
        _animator.SetTrigger("death");
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            string originalName = renderer.materials[0].name;
            string modifiedName = originalName.Replace(" (Instance)", "");
            string path = "Materials/Boss/" + modifiedName + "_Dissolve";
            Material material = Resources.Load<Material>(path);
            //material.SetFloat("DissolveAmount", 0f);


            if (material != null)
            {
                material.SetFloat("DissolveAmount", 0f);

                // 새로운 배열을 생성하여 교체
                Material[] newMaterials = renderer.materials;
                newMaterials[0] = material;
                renderer.materials = newMaterials;
            }
            else
            {
                Debug.LogError("Material not found at path: " + path);
            }

            GetComponent<DissolveInstancings>().DoVanish();
        }
    }

    // -----

    public IEnumerator WhenBulletTimeActived(float delay)
    {
        yield return new WaitForSeconds(delay);

        _bulletTimeScalable.SetActive(false);
        TimeStopInvalidate?.Invoke();
    }

    private void InvalidateBulletTime()
    {
        if (_onPhaseTree == true)
        {
            StartCoroutine(WhenBulletTimeActived(timeStopInvaliDelay));
        }
    }

    private IEnumerator ChangePhaseAfterDelay(BehaviorTree bt, float delay)
    {
        yield return new WaitForSeconds(delay);

        ChangePhase(bt);
    }

    private void ChangePhase(BehaviorTree bt)
    {
        ResetAllTriggers();

        _behaviortreeRunner.play = false;

        _behaviortreeRunner.tree = bt;
        _behaviortreeRunner.tree.blackboard = _blackboard;
        _behaviortreeRunner.BindTree();

        _behaviortreeRunner.play = true;
    }

    private void StartResetAllTriggers()
    {
        StartCoroutine(ResetAllTriggersAfterDelay());
    }

    private IEnumerator ResetAllTriggersAfterDelay()
    {
        _animator.SetTrigger("idle");

        yield return new WaitForSeconds(1f);

        ResetAllTriggers();
    }

    private void ResetAllTriggers()
    {
        // 애니메이터의 모든 파라미터를 가져옴
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            // 파라미터가 트리거일 경우 리셋
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                _animator.ResetTrigger(parameter.name);
            }
        }
    }

    private IEnumerator RushAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _animator.SetTrigger("rush");
        OffAim();
    }

    // 여기도 있지롱 보스 이펙트
    public void ChargeAim()
    {
        gameObject.GetComponent<BossChargeAimer>().DoAim();
    }

    // 1 + 1
    public void OffAim()
    {
        gameObject.GetComponent<BossChargeAimer>().OffAim();
    }
}
