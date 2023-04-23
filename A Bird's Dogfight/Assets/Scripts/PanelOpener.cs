using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject story;
    public GameObject options;
    public GameObject instructions;
    public GameObject credits;
    public GameObject almond;
    public GameObject pina;

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

    public void OpenAlmondPanel()
    {
        almond.SetActive(true);
    }

    public void OpenPinaPanel()
    {
        pina.SetActive(true);
    }
}
