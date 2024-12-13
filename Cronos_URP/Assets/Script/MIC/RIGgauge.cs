using UnityEngine;


/// <summary>
/// 플레이어의 MP 게이지를 등짝의 오브젝트에 보여주는 클래스
/// 구동방식은 디졸브 마테리얼과 똑같다
/// </summary>
public class RIGgauge : MonoBehaviour
{
    [SerializeField]
    private GameObject rig;

    private Player player;
    private Material mat;
    private float setAmount;
    private MaterialPropertyBlock mp;
    new private Renderer renderer;
    private int dissolveAmount;

    // Start is called before the first frame update
    void Start()
    {
        renderer = rig.GetComponent<Renderer>();
        player = Player.Instance;
        mat = renderer.material;

        if (mat == null)
        {
            Debug.Log("no mat");
        }

        dissolveAmount = Shader.PropertyToID("_GaugeAmount");
    }

    // Update is called once per frame
    void Update()
    {
        setAmount = player.CP / player.MaxCP;
        mat.SetFloat(dissolveAmount, setAmount);
    }
}
