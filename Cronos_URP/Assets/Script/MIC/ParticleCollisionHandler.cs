using UnityEngine;


/// <summary>
/// 오브젝트의 파티클이 벽에 닿으면 삭제하는 클래스
/// </summary>
public class ParticleCollisionHandler : MonoBehaviour
{
    public ParticleSystem ps;
    public LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        wallLayer = LayerMask.GetMask("Wall");
        var col = ps.collision;
        col.enabled = true;
        col.type = ParticleSystemCollisionType.World;
        col.collidesWith = wallLayer;
        col.sendCollisionMessages = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (((1 << other.layer) & wallLayer) != 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
