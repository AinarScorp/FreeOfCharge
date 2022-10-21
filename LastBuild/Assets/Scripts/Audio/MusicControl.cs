using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
   [FMODUnity.EventRef]
   public string music = "event:/Music/Music";

   private FMOD.Studio.EventInstance musicEv;

   private void Start()
   {
      musicEv = FMODUnity.RuntimeManager.CreateInstance(music);

      musicEv.start();
   }

   public void MainMenuMusic()
   {
      musicEv.setParameterByName("Main Menu", 1f);
   }

   public void TutorialMusic()
   {
      musicEv.setParameterByName("Tutorial", 1f);
   }
}
