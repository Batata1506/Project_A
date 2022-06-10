using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePause = false;
    private bool inOptionMenu = false;
    public GameObject pauseMenuUI;
    public GameObject OptionMenuUI;

    private void Start()
    {

        pauseMenuUI.SetActive(false);
    }
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape) )
        {
         
            if (gamePause && OptionMenuUI.activeInHierarchy == false)
            {
                Resume();
                
            }
         
            else
            {
                Pause();
            }
        }


    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePause = false;
    }

   void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePause = true;
           if (gamePause && OptionMenuUI.activeInHierarchy == true)
        {
            pauseMenuUI.SetActive(true);
            OptionMenuUI.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}
