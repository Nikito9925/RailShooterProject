using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _instructionsMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _creditsMenu;

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _instructionsMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
    }

    public void ShowInstructionsMenu()
    {
        _mainMenu.SetActive(false);
        _instructionsMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        _mainMenu.SetActive(false);
        _instructionsMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _creditsMenu.SetActive(false);
    }

    public void ShowCreditsMenu()
    {
        _mainMenu.SetActive(false);
        _instructionsMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }

    public void CloseGame()
    {

    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
