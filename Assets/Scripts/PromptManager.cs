using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset respawnPromptData;

    [SerializeField]
    private TextAsset storyPromptData;

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
        ResetStoryPrompts();
    }

    private void PopulatePromptData(string rawText, out List<string> output)
    {
        // Initialize output
        output = new List<string>();

        string[] rawTextLines = Regex.Split(rawText, "\n|\r|\r\n");

        foreach (string line in rawTextLines)
        {
            output.Add(line);
        }
    }

    public string GetNextStoryPrompt()
    {
        currentStoryPromptIndex++;
        return storyPrompts[currentStoryPromptIndex];
    }

    // Possible TODO - Do we need a rollback thing if you can hit prompts but not cause a game save?

    public void ResetStoryPrompts()
    {
        currentStoryPromptIndex = 0;
    }

    public string GetRespawnPrompt()
    {
        return respawnPrompts[Random.Range(0, respawnPrompts.Count - 1)];
    }
}