using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    float deltaTime = 0.0f;

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
