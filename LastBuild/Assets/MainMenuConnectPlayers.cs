using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuConnectPlayers : MonoBehaviour
{
    [SerializeField]PlayerWaiterExecutor _playerWaiterExecutor;
    [SerializeField] GameObject _section2,_section3;
    int addedPlayersAmount = 0;
    void OnEnable()
    {
        if (_playerWaiterExecutor!= null)
        {
            
            _playerWaiterExecutor.PlayerAdded += CheckNumberPlayers;
        }
        if (_playerWaiterExecutor == null)
        {
            _section2.SetActive(false);
            _section3.SetActive(true);
        }
    }

    void CheckNumberPlayers()
    {
        addedPlayersAmount++;
        if (addedPlayersAmount ==2)
        {
            _section2.SetActive(false);
            _section3.SetActive(true);
        }
    }
}
