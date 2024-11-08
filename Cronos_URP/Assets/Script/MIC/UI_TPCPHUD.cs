using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TPCPHUD : UI_TPCP
{
    // �̱ۺ�����
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
    TextMeshProUGUI textTP;
    [SerializeField]
    RectTransform FxHolder;
    [SerializeField]
    Image[] circleImageTPs;
    [SerializeField]
    [Range(0, 1)] float[] TPprogress;
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

    // ui �����̴� �ð�
    public float uiDuration = 0.2f;

    Player player;
    float tp;
    float cp;
    float MaxTp;
    float MaxCp;
    public float TpInterval = 60.0f;

    Transform parentTrans;
    ParticleSystem fxPs;
    ParticleSystem glowPs;
    Quaternion prevParentRot;
    ParticleSystem.MainModule fxMain;
    ParticleSystem.MainModule glowMain;

    Vector4 green = Color.green;
    Vector4 dGreen = new Vector4(0, 0.8f, 0, 1);
    Vector4 red = Color.red;
    Vector4 red2 = new Vector4(0.8f, 0, 0, 1);
    Vector4 gray = new Vector4(0.8f, 0.8f, 0.8f, 1);
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 originScale = new Vector3(1, 1, 1);

    public GameObject speedLineUI;

    Vector4 yellow = Color.yellow;
    Vector4 yellow2 = new Vector4(1, 0.8f, 0, 1);
    Vector4 orange = new Vector4(1, 0.5f, 0, 1);
    Vector4 orange2 = new Vector4(0.8f, 0.5f, 0, 1);
    Vector4 dOrange = new Vector4(1, 0.3f, 0, 1);
    Vector4 dOrange2 = new Vector4(0.8f, 0.3f, 0, 1);
    Vector4 burgundy = new Vector4(1, 0, 0.3f, 1);
    Vector4 burgundy2 = new Vector4(0.8f, 0, 0.3f, 1);

    private int prevZone = -1;

    private void Start()
    {
        instance = this;

        Bind<TextMeshProUGUI>(typeof(Texts));
        player = Player.Instance;

        if (player == null)
        {
            Debug.Log("�÷��̾ ���粲��");
        }

        parentTrans = FxHolder.transform;
        fxPs = Tpfx.GetComponent<ParticleSystem>();
        glowPs = TpGlow.GetComponent<ParticleSystem>();
        fxMain = fxPs.main;
        glowMain = glowPs.main;
        prevParentRot = parentTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTpSlider();

        // ��ƼŬ �ý����� �����̴��� �°� ȸ���Ѵ�.
        //FxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, TPprogress * 360f));
        //TpGlow.GetComponent<ParticleSystem>().transform.rotation = FxHolder.rotation;

        //if (Input.GetKeyDown(KeyCode.T))
        //    ChangeGreen();

        //if (Input.GetKeyDown(KeyCode.Y))
        //    ChangeRed();


        // �÷��̾��� CP�� �޾ƿ´�.
        cp = player.CP;
        MaxCp = player.MaxCP;
        CPprogress = cp / MaxCp;
        circleImageCP.fillAmount = CPprogress;
        //CpHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CPprogress * -360));
    }

    void UpdateTpSlider()     
    {
        // �÷��̾��� TP�� �޾ƿ´�.
        //MaxTp = player.MaxTP;
        //TPprogress = tp / MaxTp;
        if(player.TP >= 0f)
		{
			tp = player.TP;
		}
        else
        {
			tp = 0f;
        }

        textTP.text = tp.ToString("000");

        // ���� �����̴��� ����/�ִ�� ����ؼ� �ٿ��ش�.
        //circleImageTP.fillAmount = TPprogress;
        for (int i = 0; i < circleImageTPs.Length; i++)
        {
            TPprogress[i] = ((tp - (TpInterval * i)) / TpInterval);
            circleImageTPs[i].fillAmount = TPprogress[i];

            if (TPprogress[i] < 1 && TPprogress[i] > 0)
                CheckGlowColor(i);

            // ��ƼŬ �ý����� �����̴��� �°� ȸ���Ѵ�.
            FxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, TPprogress[i] * 360f));
            TpGlow.GetComponent<ParticleSystem>().transform.rotation = FxHolder.rotation;
        }
    }

    //��ƼŬ�� ȸ���ص� �� �������� �վ��� ��������
    private void LateUpdate()
    {
        // ���� �θ� ȸ��
        Quaternion curParentRot = parentTrans.rotation;
        // �θ� ȸ���� ��ȭ��
        Quaternion rotChange = curParentRot * Quaternion.Inverse(prevParentRot);
        // �װ� �ݴ�� �ϰ�
        Quaternion inverseRotChange = Quaternion.Inverse(rotChange);
        // �ڽ� ȸ���� �װɷ� ����
        fxPs.transform.rotation = inverseRotChange * fxPs.transform.rotation;
        // ���� �θ� ȸ�� ������Ʈ
        prevParentRot = curParentRot;
    }


    // ������ ����
    public void ChangeRed()
    {
        StartCoroutine(ChangeColorScale(red, () =>
        {
            //TpGlow.SetActive(true);
            fxMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
        }));
    }

    // �ʷϻ� ����
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
        // ���� ���� ������ ȣ������ ����
        if (prevZone == i)
            return;

        Debug.Log("Current TP check zone : " + i);
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

    // �����̴�/�ؽ�Ʈ�� ũ�⸦ Ű��� ���� �ٲ۴�.
    public IEnumerator ChangeColorScale(Vector4 color, Action onComplete)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < uiDuration)
        {
            elapsedTime += Time.deltaTime;

            textTP.color = Color.Lerp(color, Color.white, elapsedTime / uiDuration);
            textTP.transform.localScale = Vector3.Lerp(hitScale, originScale, elapsedTime / uiDuration);
            // �����̴� ���� �ȹٲ۴�
            //circleImageTP.color = Color.Lerp(color, Color.white, elapsedTime / uiDuration);
            //circleImageTP.transform.localScale = Vector3.Lerp(hitScale, originScale, elapsedTime / uiDuration);
            
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
