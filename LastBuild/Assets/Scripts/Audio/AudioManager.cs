using System.Collections;
using System.Collections.Generic;
using Player.Driver;
using UnityEngine;
//using Marcos;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private ShipMovement playerPrefab;
    
        void FindPlaceHolder()
        {
            PlayerInfo[] playerInfo = FindObjectsOfType<PlayerInfo>();
            if (playerInfo == null) return;
            
            foreach (var info in playerInfo)
            {
                if (info.Controls == PlayerControls.Mover)
                {
                    //if you care about mover/driver YOU CAN ADD STUFF HERE
                }
                if (info.Controls == PlayerControls.Shooter)
                {
                    //if you care about shooter
                    SetupShootSound(info);
                }
            }
        }

        void SetupShootSound(PlayerInfo info)
        {
            //InputHandler inputHandler = info.GetComponent<InputHandler>();
            //inputHandler.DoubleButtonPressed += PlayShootSound;
        }


        void PlayShootSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot ("event:/Delivery Zones/Pickup Particles/Pickup Particles");
        }
}
