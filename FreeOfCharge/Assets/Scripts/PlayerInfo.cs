using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerControls
{
    NotSet, Mover, Shooter
}
[RequireComponent(typeof(PlayerInput))]
public class PlayerInfo : MonoBehaviour
{
    [SerializeField] PlayerControls _playerControls = PlayerControls.NotSet;

    public PlayerControls Controls => _playerControls;

    public void SetPlayerControls(PlayerControls playerControls)
    {
        _playerControls = playerControls;
    }
}
