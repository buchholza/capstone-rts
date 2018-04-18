using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject StartButton, SettingButton, CreditButton, ExitButton;
    public GameObject SettingText, CreditText, Back;

    void Start() {
        SettingText.SetActive(false);
        CreditText.SetActive(false);
        Back.SetActive(false);
    }
    public void StartGame(string name) {
        SceneManager.LoadScene(name);
    }
    public void Settings() {
        StartButton.SetActive(false);
        SettingButton.SetActive(false);
        CreditButton.SetActive(false);
        ExitButton.SetActive(false);
        SettingText.SetActive(true);
        Back.SetActive(true);
    }
    public void Credits() {
        StartButton.SetActive(false);
        SettingButton.SetActive(false);
        CreditButton.SetActive(false);
        ExitButton.SetActive(false);
        CreditText.SetActive(true);
        Back.SetActive(true);
    }
    public void QuitGame() {
        Application.Quit();
    }

    public void BackButton() {
        StartButton.SetActive(true);
        SettingButton.SetActive(true);
        CreditButton.SetActive(true);
        ExitButton.SetActive(true);
        CreditText.SetActive(false);
        SettingText.SetActive(false);
        Back.SetActive(false);
    }
}
