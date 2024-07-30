using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sonity;

public class UI_Scanner : MonoBehaviour
{
    GameObject player;
    GameObject interText;
    bool isPopup;
    bool isInteracting;

    public SoundEvent soundEventInteract;
    public Transform UItransform;

    private void Awake()
    {
        player = GameObject.Find("Player");
        interText = GameObject.Find("UI_Interact");
        isPopup = false;
        isInteracting = false;
    }

    private void Start()
    {
        //interText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger");
            interText.SetActive(true);
            isPopup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger Exit");
            interText.SetActive(false);
            isPopup = false;
        }
    }

    public void ExitInteracting()
    {
        isInteracting = false;
        Debug.Log("Exit InterAction");
    }

    void Update()
    {
        if (isPopup && Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            SoundInteract();
            Debug.Log("Player Interact");
            UI_PowerUp.PowerUp();
            //Destroy(gameObject);
            interText.SetActive(false);
            isInteracting = true;
        }
    }

    void SoundInteract()
    {
        soundEventInteract.Play(UItransform);
    }
}
