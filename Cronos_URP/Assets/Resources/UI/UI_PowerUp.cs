using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PowerUp : MonoBehaviour
{
    static GameObject powerUp;
    static bool isPowerUp;
    static GameObject player;
    static float gravity;

    private void Awake()
    {
        gravity = -9.81f;
        powerUp = GameObject.Find("UI_PowerUp");
        isPowerUp = false;
        player = GameObject.Find("Player");
    }

    private void Start()
    {
        powerUp.SetActive(false);
    }

    private void Update()
    {
        if (isPowerUp && Input.GetKeyDown(KeyCode.X))
        {
            StopPowerUp();
        }
    }

    public static void StopPowerUp()
    {
        ExitPowerUp();
        isPowerUp = false;
    }

    public static void PowerUp()
    {
        powerUp.SetActive(true);
        //PauseManager.PauseGame();
        isPowerUp = true;
    }

    public static void ExitPowerUp()
    {
        powerUp.SetActive(false);
        player.GetComponent<PlayerStateMachine>().Velocity.y = gravity;
        //PauseManager.UnPauseGame();
    }
}
