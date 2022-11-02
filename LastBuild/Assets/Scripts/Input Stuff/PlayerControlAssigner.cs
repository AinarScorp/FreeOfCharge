using System;
using System.Collections;
using System.Collections.Generic;
using Player.Driver;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlAssigner : MonoBehaviour
{
    const int ONE_PLAYER_SPAWNED = 1;
    const int TWO_PLAYERS_SPAWNED = 2;

    static PlayerControlAssigner Instance;
    public event Action PlayerAdded;
    
    [SerializeField] TextMeshProUGUI moverText;
    [SerializeField] TextMeshProUGUI shooterText;

    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] GameObject _playerPrefab;
    int spawnedControlsAmount;

    public int SpawnedControlsAmount => spawnedControlsAmount;

    List<PlayerInfo> playerInfos = new List<PlayerInfo>();
    void Awake()
    {
        if (Instance != null)
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
        if (spawnedControlsAmount < 2) return;
        if (playerPrefab != null)
        {
            playerPrefab.gameObject.SetActive(true);
        }
    }

    public void SetPlayerControls(PlayerInput playerInput)
    {
        spawnedControlsAmount++;
        if (spawnedControlsAmount > TWO_PLAYERS_SPAWNED)
        {
            Debug.Log("REPORT CHECK ME");
            return;
        }

        PlayerInfo playerInfo = playerInput.GetComponent<PlayerInfo>();
        playerInfos.Add(playerInfo);
        if (spawnedControlsAmount == ONE_PLAYER_SPAWNED)
        {
            moverText.gameObject.SetActive(false);
            shooterText.gameObject.SetActive(true);

            playerInfo.SetPlayerControls(PlayerControls.Mover);
        }

        if (spawnedControlsAmount == TWO_PLAYERS_SPAWNED)
        {
            shooterText.gameObject.SetActive(false);
            
            playerInfo.SetPlayerControls(PlayerControls.Shooter);
            ActivatePlayer(_playerPrefab);
            Destroy(_playerInputManager, 1f);
        }
        PlayerAdded?.Invoke();
    }

    public void DestroyTheseControls()
    {
        for (int p = 0; p < playerInfos.Count; p++)
        {
            Destroy(playerInfos[p].gameObject);
        }

        Instance = null;
        Destroy(this.gameObject);
    }
}