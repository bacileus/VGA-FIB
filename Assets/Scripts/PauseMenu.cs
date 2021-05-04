using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject health, mana, menu;
    bool isOpen = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
            {
                Time.timeScale = 0;
                health.SetActive(false);
                mana.SetActive(false);
                menu.SetActive(true);
                isOpen = true;
            } else Continue();
        }
    }

    public void Continue()
    {
        Time.timeScale = 1;
        health.SetActive(true);
        mana.SetActive(true);
        menu.SetActive(false);
        isOpen = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
