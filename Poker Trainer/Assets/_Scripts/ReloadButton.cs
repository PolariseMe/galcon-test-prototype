using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadButton : MonoBehaviour
{
    public void ReloadGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
