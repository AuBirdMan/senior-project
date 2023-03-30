using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject story;
    public GameObject options;
    public GameObject instructions;
    public GameObject credits;

    public void OpenStoryPanel()
    {
        story.SetActive(true);
    }

    public void OpenOptionsPanel()
    {
        options.SetActive(true);
    }

    public void OpenInstructionsPanel()
    {
        instructions.SetActive(true);
    }

    public void OpenCreditsPanel()
    {
        credits.SetActive(true);
    }
}
