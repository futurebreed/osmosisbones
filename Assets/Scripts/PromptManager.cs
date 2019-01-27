using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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
    private List<string> respawnPrompts;

    private int currentStoryPromptIndex = 0;
    private List<string> storyPrompts;

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

    private void PopulatePromptData(string rawText, out List<string> output)
    {
        // Initialize output
        output = new List<string>();

        string[] rawTextLines = Regex.Split(rawText, "\n|\r\n?");

        foreach (string line in rawTextLines)
        {
            output.Add(line);
        }
    }

    public string GetNextStoryPrompt()
    {
        string prompt = storyPrompts[currentStoryPromptIndex];

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

    public string GetNextRespawnPrompt()
    {
        string prompt = respawnPrompts[currentRespawnPromptIndex];

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

    /// <summary>
    /// Retrieve a new respawn prompt and show it on the screen
    /// </summary>
    public void ShowRespawnPrompt()
    {
        ShowPrompt(GetNextRespawnPrompt);
    }

    private void ShowPrompt(Func<string> getPromptFunction)
    {
        // Make sure a previous prompt is already shut down
        HidePrompt();

        Debug.Log("Show prompt");
        promptPanel.SetActive(true);
        delayedTextRoutine = StartCoroutine(WaitAndPrintText(getPromptFunction()));
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
    private IEnumerator<WaitForSeconds> WaitAndPrintText(string text)
    {
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