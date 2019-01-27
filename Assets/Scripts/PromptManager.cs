using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public struct AudioDialogue
{
    public string line;
    public string audioEvent;
}

public class PromptManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset respawnPromptData;

    [SerializeField]
    private TextAsset storyPromptData;

    [SerializeField]
    private float showLetterDelay;

    [SerializeField]
    private float closePromptDelay;

    private Coroutine delayedTextRoutine;

    public GameObject promptPanel;
    public Text promptText;

    private int currentRespawnPromptIndex = 0;
    private List<AudioDialogue> respawnPrompts;

    private int currentStoryPromptIndex = 0;
    private List<AudioDialogue> storyPrompts;

    private void Awake()
    {
        string respawnRawText = string.Empty;
        if (respawnPromptData != null)
        {
            respawnRawText = Encoding.Default.GetString(respawnPromptData.bytes);
        }

        string storyRawText = string.Empty;
        if (storyPromptData != null)
        {
            storyRawText = Encoding.Default.GetString(storyPromptData.bytes);
        }

        PopulatePromptData(respawnRawText, out respawnPrompts);
        PopulatePromptData(storyRawText, out storyPrompts);

        // Make sure we start back at the beginning of the game
        ResetPrompts();
    }

    private void PopulatePromptData(string rawText, out List<AudioDialogue> output)
    {
        // Initialize output
        output = new List<AudioDialogue>();

        string[] rawTextLines = Regex.Split(rawText, "\n|\r\n?");

        foreach (string line in rawTextLines)
        {
            var splitString = line.Split('\t');
            output.Add(new AudioDialogue
            {
                line = splitString[0],
                audioEvent = splitString[1]
            });
        }
    }

    public AudioDialogue GetNextStoryPrompt()
    {
        AudioDialogue prompt = storyPrompts[currentStoryPromptIndex];

        if (currentStoryPromptIndex + 1 < storyPrompts.Count)
        {
            currentStoryPromptIndex++;
        }

        return prompt;
    }

    // Possible TODO - Do we need a rollback thing if you can hit prompts but not cause a game save?

    public void ResetPrompts()
    {
        currentStoryPromptIndex = 0;
        currentRespawnPromptIndex = UnityEngine.Random.Range(0, respawnPrompts.Count - 1);
    }

    public AudioDialogue GetNextRespawnPrompt()
    {
        AudioDialogue prompt = respawnPrompts[currentRespawnPromptIndex];

        if (currentRespawnPromptIndex + 1 < respawnPrompts.Count)
        {
            currentRespawnPromptIndex++;
        }
        else
        {
            currentRespawnPromptIndex = 0;
        }

        return prompt;
    }

    /// <summary>
    /// Retrieve the next story prompt and show it on the screen
    /// </summary>
    public void ShowStoryPrompt()
    {
        ShowPrompt(GetNextStoryPrompt);
    }

    public void ShowFinalStoryPrompt()
    {
        // Make sure a previous prompt is already shut down
        HidePrompt();

        Debug.Log("Show final prompt");
        promptPanel.SetActive(true);
        delayedTextRoutine = StartCoroutine(PlayDialogue(storyPrompts[6]));
    }

    /// <summary>
    /// Retrieve a new respawn prompt and show it on the screen
    /// </summary>
    public void ShowRespawnPrompt()
    {
        ShowPrompt(GetNextRespawnPrompt);
    }

    private void ShowPrompt(Func<AudioDialogue> getPromptFunction)
    {
        // Make sure a previous prompt is already shut down
        HidePrompt();

        Debug.Log("Show prompt");
        promptPanel.SetActive(true);
        delayedTextRoutine = StartCoroutine(PlayDialogue(getPromptFunction()));
    }

    /// <summary>
    /// Hide the active text prompt
    /// </summary>
    public void HidePrompt()
    {
        Debug.Log("Hide prompt");
        promptPanel.SetActive(false);

        if (delayedTextRoutine != null)
        {
            StopCoroutine(delayedTextRoutine);
        }
    }

    /// <summary>
    /// Does a delayed text write effect
    /// </summary>
    private IEnumerator<WaitForSeconds> PlayDialogue(AudioDialogue dialogue)
    {
        var eventToLoad = dialogue.audioEvent;
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventToLoad, gameObject);

        var text = dialogue.line;

        int totalToShow = text.Length;
        int shownSoFarCount = 0;

        while (totalToShow > shownSoFarCount)
        {
            shownSoFarCount++;
            promptText.text = text.Substring(0, shownSoFarCount);

            // If the text is done rendering, wait for our prompt delay, then hide the prompt
            if (totalToShow == shownSoFarCount)
            {
                yield return new WaitForSeconds(closePromptDelay * Time.deltaTime);

                HidePrompt();
            }
            else
            {
                // Wait until we should show the next text character
                yield return new WaitForSeconds(showLetterDelay * Time.deltaTime);
            }
        }
    }
}