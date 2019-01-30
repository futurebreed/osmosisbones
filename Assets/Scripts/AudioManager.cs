using FMOD;
using FMOD.Studio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private static bool dialogRunning = false;

    [SerializeField]
    private float maxBackgroundAudioLevel;

    [SerializeField]
    private float maxDialogueAudioLevel;

    [SerializeField]
    private float maxSoundEffectAudioLevel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Mandatory for later dynamic audio spawning
        lowlevelSystem = FMODUnity.RuntimeManager.LowlevelSystem;
        uint version;
        lowlevelSystem.getVersion(out version);

        FMODUnity.RuntimeManager.LoadBank("Master Bank");
    }

    private FMOD.System lowlevelSystem;

    private EventInstance bgAudioInstance;

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
        bgAudioInstance.setVolume(maxBackgroundAudioLevel);
        bgAudioInstance.start();
    }

    public void PlayDialogSound(Guid eventToLoad, GameObject parentObject)
    {
        if (!dialogRunning)
        {
            UnityEngine.Debug.Log("dialog playing");
            dialogRunning = true;

            var audioInstance = FMODUnity.RuntimeManager.CreateInstance(eventToLoad);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(audioInstance, parentObject.transform, parentObject.GetComponent<Rigidbody>());

            audioInstance.setCallback(audioFinishedCallback, EVENT_CALLBACK_TYPE.SOUND_STOPPED);
            audioInstance.setVolume(maxDialogueAudioLevel);
            audioInstance.start();
            audioInstance.release();
        }
        else
        {
            UnityEngine.Debug.Log("dialog already playing. new dialog sound skipped");
        }
    }

    private RESULT audioFinishedCallback(EVENT_CALLBACK_TYPE type, EventInstance eventInstance, IntPtr parameters)
    {
        dialogRunning = false;
        UnityEngine.Debug.Log("dialog finished");

        return RESULT.OK;
    }

    public void Stop()
    {
        bgAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}