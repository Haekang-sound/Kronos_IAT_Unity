using UnityEngine;
using UnityEngine.Playables;

public class SpawnHitColliderMixerBehaviour : PlayableBehaviour
{
    GameObject[] gameObjects;
    bool[] isHitColliders;

    Transform _spawnPosition;


    // 주의: 이 함수는 Play모드가 아닌 Edit모드에서도(플레이 버튼을 누르지 않았을 때도) 호출됨.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        _spawnPosition = playerData as Transform;

        // 트랙에 존재하는 클립의 갯수
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            // 현재 클립이 트랙에서 blending에 비례하여 얼마나 트랙을 점유하고 있는지
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<SpawnHitColliderBehaviour> inputPlayable = (ScriptPlayable<SpawnHitColliderBehaviour>)playable.GetInput(i);
            SpawnHitColliderBehaviour input = inputPlayable.GetBehaviour();

            if (_spawnPosition != null && inputWeight == 1 && isHitColliders[i] == false)
            {

                switch (input.colliderType)
                {
                    case SpawnHitColliderBehaviour.ColliderType.Box:
                        {
                            CreateHitCollider(i, "HitBox", _spawnPosition);
                            CreateHitBoxCollider(i, input);
                        }
                        break;
                    case SpawnHitColliderBehaviour.ColliderType.Sphere:
                        {
                            CreateHitCollider(i, "HitSphere", _spawnPosition);
                            CreateHitSphereCollider(i, input);
                        }
                        break;
                    case SpawnHitColliderBehaviour.ColliderType.Capsule:
                        {
                            CreateHitCollider(i, "HitCapsule", _spawnPosition);
                            CreateHitCapsuleCollider(i, input);
                        }
                        break;
                }
            }
            else if (inputWeight == 0 && isHitColliders[i] == true)
            {
                DeleteHitColliders(i);
            }

        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    public override void OnGraphStart(Playable playable)
    {

    }

    public override void OnGraphStop(Playable playable)
    {

    }

    public override void OnPlayableCreate(Playable playable)
    {
        int inputCount = playable.GetInputCount();
        gameObjects = new GameObject[inputCount];
        isHitColliders = new bool[inputCount];

    }

    public override void OnPlayableDestroy(Playable playable)
    {
        // 트랙에 존재하는 클립의 갯수
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            DeleteHitColliders(i);
        }
    }

    private void DeleteHitColliders(int i)
    {
        if (isHitColliders[i] == true)
        {
            isHitColliders[i] = false;
            gameObjects[i].GetComponent<GameObjectDestroyer>().DestroyGameObject();
            gameObjects[i] = null;
        }
    }

    void CreateHitCollider(int i, string name, Transform transform)
    {
        isHitColliders[i] = true;
        gameObjects[i] = new GameObject(name);

        gameObjects[i].transform.position = transform.position;
        gameObjects[i].transform.rotation = transform.rotation;

        // var gameObjectDestroyer = 
        gameObjects[i].AddComponent<GameObjectDestroyer>();
    }

    void AddDamagerComponent(int i, SpawnHitColliderBehaviour input)
    {
        var damager = gameObjects[i].AddComponent<SimpleDamager>();
        damager.drawGizmos = true;
        damager.damageAmount = input.damageAmount;
        damager.targetLayers = input.targetLayers;
        damager.BeginAttack();
    }

    private void CreateHitBoxCollider(int i, SpawnHitColliderBehaviour input)
    {


        var collider = gameObjects[i].AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = input.center;
        collider.size = input.size;

        AddDamagerComponent(i, input);
    }

    private void CreateHitSphereCollider(int i, SpawnHitColliderBehaviour input)
    {
        var collider = gameObjects[i].AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.center = input.center;
        collider.radius = input.radius;

        AddDamagerComponent(i, input);
    }

    private void CreateHitCapsuleCollider(int i, SpawnHitColliderBehaviour input)
    {
        var collider = gameObjects[i].AddComponent<CapsuleCollider>();
        collider.isTrigger = true;
        collider.center = input.center;
        collider.radius = input.radius;
        collider.height = input.height;
        collider.direction = (int)input.direction;

        AddDamagerComponent(i, input);
    }
}

