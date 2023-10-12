using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Application.Quit();
    }
}
