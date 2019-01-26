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

    public GameObject promptPanel;
    public Text promptText;

    private int currentRespawnPromptIndex = 0;
    private List<string> respawnPrompts;

    private int currentStoryPromptIndex = 0;
    private List<string> storyPrompts;

    private void Start()
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
        currentRespawnPromptIndex = Random.Range(0, respawnPrompts.Count - 1);
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

    public void ShowStoryPrompt()
    {
        Debug.Log("Show story prompt");
        promptPanel.SetActive(true);
        promptText.text = GetNextStoryPrompt();
    }

    public void ShowRespawnPrompt()
    {
        Debug.Log("Show respawn prompt");
        promptPanel.SetActive(true);
        promptText.text = GetNextRespawnPrompt();
    }

    public void HidePrompt()
    {
        Debug.Log("Hide prompt");
        promptPanel.SetActive(false);
    }
}