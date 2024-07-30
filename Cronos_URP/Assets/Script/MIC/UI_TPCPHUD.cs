using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TPCPHUD : UI_TPCP
{
    // 싱글벙글턴
    static UI_TPCPHUD instance;
    public static UI_TPCPHUD GetInstance() { return instance; }

    [SerializeField]
    TextMeshProUGUI textTP;
    [SerializeField]
    RectTransform FxHolder;
    [SerializeField]
    Image circleImageTP;
    [SerializeField]
    [Range(0, 1)] float TPprogress = 0f;
    [SerializeField]
    GameObject Tpfx;
    [SerializeField]
    GameObject TpGlow;
    [SerializeField]
    Image circleImageCP;
    [SerializeField]
    GameObject CpHolder;
    [SerializeField]
    [Range(0, 1)] float CPprogress = 0f;

    // ui 움직이는 시간
    public float uiDuration = 0.2f;

    Player player;
    float tp;
    float cp;
    float MaxTp;
    float MaxCp;

    Transform parentTrans;
    ParticleSystem ps;
    Quaternion prevParentRot;
    ParticleSystem.MainModule psMain;

    Vector4 orange = new Vector4(1, 0.5f, 0, 1);
    Vector4 dOrange = new Vector4(1, 0.3f, 0, 1);
    Vector4 red = Color.red;
    Vector4 dRed = new Vector4(0.8f, 0, 0, 1);
    Vector4 gray = new Vector4(0.8f, 0.8f, 0.8f, 1);
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 originScale = new Vector3(1, 1, 1);

    private void Start()
    {
        instance = this;

        Bind<TextMeshProUGUI>(typeof(Texts));
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("플레이어가 없당께요");
        }

        parentTrans = FxHolder.transform;
        ps = Tpfx.GetComponent<ParticleSystem>();
        psMain = ps.main;
        prevParentRot = parentTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어의 TP를 받아온다.
        MaxTp = player.MaxTP;
        tp = player.TP;
        textTP.text = tp.ToString("000");
        TPprogress = tp / MaxTp;
        // 원형 슬라이더를 현재/최대로 계산해서 줄여준다.
        circleImageTP.fillAmount = TPprogress;
        // 파티클 시스템을 슬라이더에 맞게 회전한다.
        FxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, TPprogress * 360));
        TpGlow.GetComponent<ParticleSystem>().transform.rotation = FxHolder.rotation;

        if (Input.GetKeyDown(KeyCode.T))
            ChangeOrange();

        if (Input.GetKeyDown(KeyCode.Y))
            ChangeRed();


        // 플레이어의 CP를 받아온다.
        cp = player.CP;
        MaxCp = player.MaxCP;
        CPprogress = cp / MaxCp;
        circleImageCP.fillAmount = CPprogress;
        CpHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CPprogress * -360));
    }

    // 파티클이 회전해도 한 방향으로 뿜어져 나오려면
    private void LateUpdate()
    {
        // 현재 부모 회전
        Quaternion curParentRot = parentTrans.rotation;
        // 부모 회전의 변화량
        Quaternion rotChange = curParentRot * Quaternion.Inverse(prevParentRot);
        // 그걸 반대로 하고
        Quaternion inverseRotChange = Quaternion.Inverse(rotChange);
        // 자식 회전을 그걸로 적용
        ps.transform.rotation = inverseRotChange * ps.transform.rotation;
        // 이전 부모 회전 업데이트
        prevParentRot = curParentRot;
    }

    // 노가드 람다
    public void ChangeRed()
    {
        StartCoroutine(ChangeColorScale(red, () =>
        {
            TpGlow.SetActive(true);
            psMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
        }));
    }

    // 가드 람다
    public void ChangeOrange()
    {
        StartCoroutine(ChangeColorScale(orange, () =>
        {
            TpGlow.SetActive(true);
            psMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
        }));
    }

    // 슬라이더/텍스트의 크기를 키우고 색을 바꾼다.
    public IEnumerator ChangeColorScale(Vector4 color, Action onComplete)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < uiDuration)
        {
            elapsedTime += Time.deltaTime;

            textTP.color = Color.Lerp(color, Color.white, elapsedTime / uiDuration);
            textTP.transform.localScale = Vector3.Lerp(hitScale, originScale, elapsedTime / uiDuration);
            circleImageTP.color = Color.Lerp(color, Color.white, elapsedTime / uiDuration);
            circleImageTP.transform.localScale = Vector3.Lerp(hitScale, originScale, elapsedTime / uiDuration);
            
            if (color == orange)
            {
                psMain.startColor = new ParticleSystem.MinMaxGradient(orange, dOrange);
            }
            if (color == red)
            {
                psMain.startColor = new ParticleSystem.MinMaxGradient(red, dRed);
            }
            TpGlow.SetActive(false);
            yield return null;
        }

        onComplete?.Invoke();
    }
}
