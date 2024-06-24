using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TPCPHUD : UI_TPCP
{
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

    // ui �����̴� �ð�
    public float uiDuration = 0.2f;

    Player player;
    float tp;
    float cp;
    float MaxTp = 300.0f;
    float MaxCp = 100.0f;

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
        Bind<TextMeshProUGUI>(typeof(Texts));
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("�÷��̾ ���粲��");
        }

        parentTrans = FxHolder.transform;
        ps = Tpfx.GetComponent<ParticleSystem>();
        psMain = ps.main;
        prevParentRot = parentTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾��� TP�� �޾ƿ´�.
        tp = player.TP;
        textTP.text = tp.ToString("000");
        TPprogress = tp / MaxTp;
        // ���� �����̴��� ����/�ִ�� ����ؼ� �ٿ��ش�.
        circleImageTP.fillAmount = TPprogress;
        // ��ƼŬ �ý����� �����̴��� �°� ȸ���Ѵ�.
        FxHolder.rotation = Quaternion.Euler(new Vector3(0, 0, TPprogress * 360));
        TpGlow.GetComponent<ParticleSystem>().transform.rotation = FxHolder.rotation;

        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(ChangeColorScale(orange, ()=> 
            { 
                TpGlow.SetActive(true);
                psMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
            }));

        if (Input.GetKeyDown(KeyCode.Y))
            StartCoroutine(ChangeColorScale(red, () => 
            { 
                TpGlow.SetActive(true);
                psMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, gray);
            }));


        // �÷��̾��� CP�� �޾ƿ´�.
        //cp = player.CP;
        //CPprogress = cp / MaxCp;
        circleImageCP.fillAmount = CPprogress;
        CpHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, CPprogress * -360));
    }

    // ��ƼŬ�� ȸ���ص� �� �������� �վ��� ��������
    private void LateUpdate()
    {
        // ���� �θ� ȸ��
        Quaternion curParentRot = parentTrans.rotation;
        // �θ� ȸ���� ��ȭ��
        Quaternion rotChange = curParentRot * Quaternion.Inverse(prevParentRot);
        // �װ� �ݴ�� �ϰ�
        Quaternion inverseRotChange = Quaternion.Inverse(rotChange);
        // �ڽ� ȸ���� �װɷ� ����
        ps.transform.rotation = inverseRotChange * ps.transform.rotation;
        // ���� �θ� ȸ�� ������Ʈ
        prevParentRot = curParentRot;
    }

    // �����̴�/�ؽ�Ʈ�� ũ�⸦ Ű��� ���� �ٲ۴�.
    IEnumerator ChangeColorScale(Vector4 color, Action onComplete)
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
