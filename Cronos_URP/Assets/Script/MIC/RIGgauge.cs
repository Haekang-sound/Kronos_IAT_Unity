using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIGgauge : MonoBehaviour
{
    [SerializeField]
    GameObject rig;

    Player player;
    Material mat;
    float setAmount;
    MaterialPropertyBlock mp;
    new Renderer renderer;
    int dissolveAmount;

    // Start is called before the first frame update
    void Start()
    {
        renderer = rig.GetComponent<Renderer>();
        player = Player.Instance;
        //mp = new MaterialPropertyBlock();
        mat = renderer.material;
        if (mat == null)
            Debug.Log("no mat");
        dissolveAmount = Shader.PropertyToID("_GaugeAmount");

    }

    // Update is called once per frame
    void Update()
    {
        setAmount = player.CP / player.MaxCP;
        mat.SetFloat(dissolveAmount, setAmount);
        //mp.SetFloat(dissolveAmount, setAmount);
        //renderer.SetPropertyBlock(mp);
        Debug.Log("mat : " + mat.GetFloat(dissolveAmount));
    }
}
