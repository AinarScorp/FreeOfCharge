using System;
using System.Collections;
using System.Collections.Generic;
using Player.Driver;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaiterExecutor : MonoBehaviour
{
    static PlayerWaiterExecutor Instance;
    public event Action PlayerAdded;
    [SerializeField] TextMeshProUGUI moverText;
    [SerializeField] TextMeshProUGUI shooterText;

    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] bool setMoverFirst;
    int spawnedControlsAmount;

    void Awake()
    {
        if (Instance !=null)
        {
            Instance.ActivatePlayer(_playerPrefab);
            moverText.gameObject.SetActive(false);
            shooterText.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        moverText.gameObject.SetActive(true);
        shooterText.gameObject.SetActive(false);
        _playerInputManager.enabled = true;

    }
    

    void ActivatePlayer(GameObject playerPrefab)
    {
        if (spawnedControlsAmount<2) return;
        if (playerPrefab != null)
        {
            playerPrefab.gameObject.SetActive(true);
        }
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
        PlayerAdded?.Invoke();
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
        if (spawnedControlsAmount ==2)
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
            ActivatePlayer(_playerPrefab);
            Destroy(_playerInputManager,1f);

        }
    }
}