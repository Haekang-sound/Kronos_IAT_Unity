using System.Collections;
using UnityEngine;

public class BossLightRushCloneBehavior : MonoBehaviour
{
    public float activeTime;
    public float rotationSpeed;
    public float lifeTime = 3f;

    public GameObject target;
    private Animator animator;

    private float elapsedTime;

    private void Awake()
    {
        target = Player.Instance.gameObject;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(RushAfterSeconds());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > lifeTime + activeTime)
        {
            Destroy(gameObject);
        }

        LookAtTarget();
    }

    public void BeginAiming()
    {
        rotationSpeed = 1080f;
    }

    public void StopAiming()
    {
        rotationSpeed = 0f;
    }

    public void ResetAiming()
    {
        rotationSpeed = 1f;
    }

    public void LookAtTarget()
    {
        if (target == null) return;

        // 바라보는 방향 설정
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    private IEnumerator RushAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        Rush();
    }

    private void Rush()
    {
        if (animator != null)
        {
            animator.SetTrigger("rush");
        }
    }
}
