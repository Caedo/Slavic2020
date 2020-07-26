using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject panel;

    bool isOpened;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            TogglePanel();
        }
    }

    public void TogglePanel() {
        isOpened = !isOpened;

        panel.SetActive(isOpened);
        Time.timeScale = isOpened ? 0 : 1;
    }

    public void QuitGame() {
        Application.Quit();
    }

}
