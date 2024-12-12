using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI_Bind 상위 클래스를 상속받는, 플레이어의 HP/MP 게이지를 관리하는 클래스
/// 
/// </summary>
public class UI_TPCPHUD : UI_Bind
{
    // 싱글턴
    static UI_TPCPHUD instance;

    public static UI_TPCPHUD Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_TPCPHUD>();
                if (instance == null)
                {
                    GameObject hud = new GameObject(typeof(UI_TPCPHUD).Name);
                    instance = hud.AddComponent<UI_TPCPHUD>();

                    DontDestroyOnLoad(hud);
                }
            }
            return instance;
        }
    }


    [SerializeField]
    [Tooltip("TP의 TMP 텍스트입니다")]
    TextMeshProUGUI textTP;
    [SerializeField]
    [Tooltip("TP 슬라이더의 파티클을 회전하기 위한 상위 게임오브젝트입니다")]
    RectTransform fxHolder;
    [SerializeField]
    [Tooltip("TP에 따라 채워지는 링 이미지의 배열입니다. 5개의 멤버가 필요합니다")]
    Image[] circleImageTPs;
    [SerializeField]
    [Tooltip("TP 이미지를 채우고 줄이기 위한 진척도의 배열입니다. 5개의 멤버가 필요합니다")]
    [Range(0, 1)] float[] tpProgress;
    [SerializeField]
    [Tooltip("TP 파티클이 증발하는 효과. 현재 사용하지 않습니다")]
    GameObject tpFx;
    [SerializeField]
    [Tooltip("TP가 진척도에 따라 이미지의 fill이 줄어들 때, 보더를 표시하기 위한 파티클입니다")]
    GameObject tpGlow;
    [SerializeField]
    [Tooltip("CP의 이미지 오브젝트입니다")]
    Image imageCP;
    [SerializeField]
    [Tooltip("CP 이미지를 채우고 줄이기 위한 진척도입니다")]
    [Range(0, 1)] float cpProgress = 0f;

    // ui 움직이는 시간
    public float uiDuration = 0.2f;

    private Player player;
    private float tp;
    private float cp;
    private float maxTp;
    private float maxCp;
    public float TpInterval = 60.0f;

    private Transform parentTrans;
    private ParticleSystem fxPs;
    private ParticleSystem glowPs;
    private Quaternion prevParentRot;
    private ParticleSystem.MainModule fxMain;
    private ParticleSystem.MainModule glowMain;

    private Vector4 green = Color.green;
    private Vector4 dGreen = new Vector4(0, 0.8f, 0, 1);
    private Vector4 red = Color.red;
    private Vector4 red2 = new Vector4(0.8f, 0, 0, 1);
    private Vector4 gray = new Vector4(0.8f, 0.8f, 0.8f, 1);
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 originScale = new Vector3(1, 1, 1);

    public GameObject speedLineUI;

    private Vector4 yellow = Color.yellow;
    private Vector4 yellow2 = new Vector4(1, 0.8f, 0, 1);
    private Vector4 orange = new Vector4(1, 0.5f, 0, 1);
    private Vector4 orange2 = new Vector4(0.8f, 0.5f, 0, 1);
    private Vector4 dOrange = new Vector4(1, 0.3f, 0, 1);
    private Vector4 dOrange2 = new Vector4(0.8f, 0.3f, 0, 1);
    private Vector4 burgundy = new Vector4(1, 0, 0.3f, 1);
    private Vector4 burgundy2 = new Vector4(0.8f, 0, 0.3f, 1);

    private int prevZone = -1;

    private void Start()
    {
        instance = this;

        Bind<TextMeshProUGUI>(typeof(Texts));
        player = Player.Instance;

        if (player == null)
        {
            Debug.Log("플레이어가 없습니다");
        }

        parentTrans = fxHolder.transform;
        fxPs = tpFx.GetComponent<ParticleSystem>();
        glowPs = tpGlow.GetComponent<ParticleSystem>();
        fxMain = fxPs.main;
        glowMain = glowPs.main;
        prevParentRot = parentTrans.rotation;

        textTP.text = GetText((int)Texts.HUD_TPAmount).text;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTpSlider();

        // 플레이어의 CP를 받아온다.
        cp = player.CP;
        maxCp = player.MaxCP;
        cpProgress = cp / maxCp;
        imageCP.fillAmount = cpProgress;
    }

    // 실시간으로 반영하려면 어쨌든 업데이트에서 불러야 할 듯
    void UpdateTpSlider()     
    {
        // 플레이어의 TP를 받아온다.
        if(player.TP >= 0f)
		{
			tp = player.TP;
		}
        else
        {
			tp = 0f;
        }

        // TP의 텍스트를 업데이트. 세자리 숫자로
        textTP.text = tp.ToString("000");
        
        // 원형 슬라이더를 현재/최대로 계산해서 줄여준다.
        for (int i = 0; i < circleImageTPs.Length; i++)
        {
            tpProgress[i] = ((tp - (TpInterval * i)) / TpInterval);
            circleImageTPs[i].fillAmount = tpProgress[i];

            if (tpProgress[i] < 1 && tpProgress[i] > 0)
                CheckGlowColor(i);

            // 파티클 시스템을 슬라이더에 맞게 회전한다.
            fxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, tpProgress[i] * 360f));
            tpGlow.GetComponent<ParticleSystem>().transform.rotation = fxHolder.rotation;
        }
    }

    //파티클이 회전해도 한 방향으로 뿜어져 나오려면
    //private void LateUpdate()
    //{
    //    // 현재 부모 회전
    //    Quaternion curParentRot = parentTrans.rotation;
    //    // 부모 회전의 변화량
    //    Quaternion rotChange = curParentRot * Quaternion.Inverse(prevParentRot);
    //    // 그걸 반대로 하고
    //    Quaternion inverseRotChange = Quaternion.Inverse(rotChange);
    //    // 자식 회전을 그걸로 적용
    //    fxPs.transform.rotation = inverseRotChange * fxPs.transform.rotation;
    //    // 이전 부모 회전 업데이트
    //    prevParentRot = curParentRot;
    //}


    public void ChangeRed()
    {
        StartCoroutine(ChangeColorScale(red, () =>
        {
            //TpGlow.SetActive(true);
            fxMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
        }));
    }

    public void ChangeGreen()
    {
        StartCoroutine(ChangeColorScale(green, () =>
        {
            //TpGlow.SetActive(true);
            fxMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
        }));
    }

    public void CheckGlowColor(int i)
    {
        // 이전 존과 같으면 호출하지 않음
        if (prevZone == i)
            return;

        Debug.Log($"Current TP check zone : {i}");
        prevZone = i;

        switch (i)
        {
            case 4:
                glowMain.startColor = new ParticleSystem.MinMaxGradient(yellow, yellow2);
                break;
            case 3:
                glowMain.startColor = new ParticleSystem.MinMaxGradient(orange, orange2);
                break;
            case 2:
                glowMain.startColor = new ParticleSystem.MinMaxGradient(dOrange, dOrange2);
                break;
            case 1:
                glowMain.startColor = new ParticleSystem.MinMaxGradient(red, red2);
                break;
            case 0:
                glowMain.startColor = new ParticleSystem.MinMaxGradient(burgundy, burgundy2);
                break;
            default: 
                break;
        }
    }

    public IEnumerator ChangeColorScale(Vector4 color, Action onComplete)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < uiDuration)
        {
            elapsedTime += Time.deltaTime;

            textTP.color = Color.Lerp(color, Color.white, elapsedTime / uiDuration);
            textTP.transform.localScale = Vector3.Lerp(hitScale, originScale, elapsedTime / uiDuration);
            
            if (color == green)
            {
                fxMain.startColor = new ParticleSystem.MinMaxGradient(green, dGreen);
            }
            if (color == red)
            {
                fxMain.startColor = new ParticleSystem.MinMaxGradient(red, red2);
            }
            //TpGlow.SetActive(false);
            yield return null;
        }

        onComplete?.Invoke();
    }
}
