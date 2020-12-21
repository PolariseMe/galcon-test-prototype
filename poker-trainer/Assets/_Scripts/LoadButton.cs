using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
