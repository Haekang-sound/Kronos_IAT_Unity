using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField]
    bool _isInitialized = false;


    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (_isInitialized == false)
        {
            DataManager.DeleteAllData();
            _isInitialized = true;
        }

        DontDestroyOnLoad(gameObject);
    }


    // 
    //     public PlayerData PlayerDT { get; set; }
    //     public bool isRespawn { get; set; } = false;
    // 
    //     private void Awake()
    //     {
    //         DontDestroyOnLoad(gameObject);
    //     }
    // 
    //     public void SwitchScene(string NextScene)
    //     {
    //         SetCursorInactive();
    //         if (NextScene == "TitleScene")
    //         {
    //             Cursor.visible = true;
    //         }
    //         SceneManager.LoadScene(NextScene);
    //     }

    //     // Update is called once per frame
    //     void Update()
    //     {
    //         if (Input.GetKeyDown(KeyCode.Alpha4))
    //         {
    //             SetCursorInactive();
    //         }
    //     }
    //     void SetCursorInactive()
    //     {
    //         Cursor.visible = !Cursor.visible; // 마우스 안보이게 하기
    //         if(Cursor.visible)
    // 		{
    // 			Cursor.lockState = CursorLockMode.None;
    // 		}
    // 		else
    // 		{
    // 			Cursor.lockState = CursorLockMode.Locked;
    // 		}
    //     }

    //     public IEnumerator SceneTransition(string NextScene)
    //     {
    //         SceneManager.LoadScene(NextScene);
    //         yield return new WaitForEndOfFrame();
    //         GameObject temp = GameObject.Find("Player");
    //         temp.GetComponent<Player>().StartPlayer();
    //         temp.GetComponent<Transform>().SetPositionAndRotation(new Vector3(0f, 7f, 0f), Quaternion.identity);
    //         yield break;
    //     }


}
