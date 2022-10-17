using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaiterExecutor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moverText;
    [SerializeField] TextMeshProUGUI shooterText;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] bool setMoverFirst;
    int spawnedControlsAmount;
    PlayerInputManager _playerInputManager;

    void Start()
    {
        moverText.gameObject.SetActive(true);
        shooterText.gameObject.SetActive(false);

    }

    public void SetPlayerControls(PlayerInput playerInput)
    {
        spawnedControlsAmount++;
        if (spawnedControlsAmount >2)
        {
            Debug.Log("REPORT CHECK ME");
            return;
        }
        PlayerInfo playerInfo = playerInput.GetComponent<PlayerInfo>();
        if (spawnedControlsAmount ==1)
        {
            moverText.gameObject.SetActive(false);
            shooterText.gameObject.SetActive(true);
            //_playerInputManager.playerPrefab.
            if (setMoverFirst)
            {
                playerInfo.SetPlayerControls(PlayerControls.Mover);
            }
            else
            {
                playerInfo.SetPlayerControls(PlayerControls.Shooter);
            }
        }
        else if (spawnedControlsAmount ==2)
        {
            shooterText.gameObject.SetActive(false);

            if (setMoverFirst)
            {
                playerInfo.SetPlayerControls(PlayerControls.Shooter);
            }
            else
            {
                playerInfo.SetPlayerControls(PlayerControls.Mover);
            }

            playerPrefab.SetActive(true);
        }
    }
}