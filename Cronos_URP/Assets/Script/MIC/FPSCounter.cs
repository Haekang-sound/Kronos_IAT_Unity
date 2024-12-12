using TMPro;
using UnityEngine;


/// <summary>
/// UI에서 디버깅을 위해 FPS을 띄워주는 클래스
/// </summary>
public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;

        fpsText.text = string.Format("FPS : {0:0.0}", fps);
    }
}
