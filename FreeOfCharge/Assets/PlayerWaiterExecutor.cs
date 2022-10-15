using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaiterExecutor : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] bool setMoverFirst;
    int spawnedControlsAmount;
    PlayerInputManager _playerInputManager;

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
