using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusic : MonoBehaviour
{
    SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = SoundManager.Instance;
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        Debug.Log("Play Title BGM");
        sm.PlayBGM("Title_Sound_Fix_SE");
    }
}
