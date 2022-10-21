using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerControls
{
    NotSet, Mover, Shooter
}
[RequireComponent(typeof(PlayerInput))]
public class PlayerInfo : MonoBehaviour
{
    static PlayerInfo Instance1, Instance2;
    [SerializeField] PlayerControls _playerControls = PlayerControls.NotSet;

    public PlayerControls Controls => _playerControls;



    public void SetPlayerControls(PlayerControls playerControls)
    {
        _playerControls = playerControls;
        if (playerControls == PlayerControls.Mover)
        {
            if (Instance1!=null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance1 = this;
            DontDestroyOnLoad(this.gameObject);
            
        }
        else if (playerControls == PlayerControls.Shooter)
        {
            if (Instance2!=null)
            {
                Destroy(this.gameObject);
                return;

            }
            Instance2 = this;
            DontDestroyOnLoad(this.gameObject);
        }


    }
}