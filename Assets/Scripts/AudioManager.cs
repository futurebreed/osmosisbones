using FMOD;
using FMOD.Studio;
using System;
using UnityEngine;

public class AudioManager
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private AudioManager()
    {
        // Mandatory for later dynamic audio spawning
        lowlevelSystem = FMODUnity.RuntimeManager.LowlevelSystem;
        uint version;
        lowlevelSystem.getVersion(out version);

        FMODUnity.RuntimeManager.LoadBank("Master Bank");
    }

    private FMOD.System lowlevelSystem;

    private EventInstance bgAudioInstance;
    private bool dialogRunning = false;

    public void PlayBackgroundAudio(Guid eventToLoad, GameObject parentObject)
    {
        if (bgAudioInstance.isValid())
        {
            bgAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            bgAudioInstance.release();
        }

        UnityEngine.Debug.Log("bgmusic playing");
        bgAudioInstance = FMODUnity.RuntimeManager.CreateInstance(eventToLoad);

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(bgAudioInstance, parentObject.transform, parentObject.GetComponent<Rigidbody>());
        bgAudioInstance.setVolume(1f);
        bgAudioInstance.start();
    }

    public void PlayDialogSound(Guid eventToLoad, GameObject parentObject)
    {
        if (!dialogRunning)
        {
            UnityEngine.Debug.Log("dialog playing");
            bgAudioInstance.setVolume(0.1f);
            dialogRunning = true;

            var audioInstance = FMODUnity.RuntimeManager.CreateInstance(eventToLoad);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(audioInstance, parentObject.transform, parentObject.GetComponent<Rigidbody>());

            audioInstance.setCallback(audioFinishedCallback, EVENT_CALLBACK_TYPE.SOUND_STOPPED);
            audioInstance.start();
        }
        else
        {
            UnityEngine.Debug.Log("dialog already playing. new dialog sound skipped");
        }
    }

    private RESULT audioFinishedCallback(EVENT_CALLBACK_TYPE type, EventInstance eventInstance, IntPtr parameters)
    {
        dialogRunning = false;

        try
        {
            eventInstance.release();
            UnityEngine.Debug.Log("dialog finished");

            bgAudioInstance.setVolume(1f);
        }
        catch (Exception)
        {
            UnityEngine.Debug.Log("hit exception after audio finish");
        }

        return RESULT.OK;
    }

    public void Stop()
    {
        bgAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}