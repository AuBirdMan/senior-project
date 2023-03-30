using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCloser : MonoBehaviour
{
    public GameObject story;
    public GameObject options;
    public GameObject instructions;
    public GameObject credits;

    public void CloseStoryPanel()
    {
        story.SetActive(false);
    }

    public void CloseOptionsPanel()
    {
        options.SetActive(false);
    }

    public void CloseInstructionsPanel()
    {
        instructions.SetActive(false);
    }

    public void CloseCreditsPanel()
    {
        credits.SetActive(false);
    }
}
