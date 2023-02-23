using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeonButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Battle");
    }

    public void OnClick2()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
