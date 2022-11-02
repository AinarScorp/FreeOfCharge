using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuConnectPlayers : MonoBehaviour
{
    [SerializeField] PlayerControlAssigner playerControlAssigner;
    [SerializeField] GameObject _reassignControls, playerAssignerTexts;
    [SerializeField] GameObject _section2, _section3;
    int addedPlayersAmount = 0;

    void OnEnable()
    {
        playerAssignerTexts.SetActive(false);
        PlayerInfo[] playerInfos = FindObjectsOfType<PlayerInfo>();
        if (playerInfos.Length > 0)
        {
            _reassignControls.SetActive(true);

            return;
        }

        AssignControls();


    }

    public void AssignControls()
    {
        _reassignControls.SetActive(false);
        playerAssignerTexts.SetActive(true);
        PlayerControlAssigner previousPlayerControlAssigner = FindObjectOfType<PlayerControlAssigner>();
        if (previousPlayerControlAssigner !=null)
        {
            previousPlayerControlAssigner.DestroyTheseControls();
        }

        playerControlAssigner.gameObject.SetActive(true);
        if (playerControlAssigner != null)
        {
            playerControlAssigner.PlayerAdded += CheckNumberPlayers;
        }
    }

    public void GoToThirdSection()
    {
        _section2.SetActive(false);
        _section3.SetActive(true);
    }

    void OnDisable()
    {
        playerControlAssigner.PlayerAdded -= CheckNumberPlayers;
    }


    void CheckNumberPlayers()
    {
        addedPlayersAmount++;
        if (addedPlayersAmount == 2)
        {
            GoToThirdSection();
        }
    }
}