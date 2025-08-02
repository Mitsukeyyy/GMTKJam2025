using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public Button resumeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menu.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            menu.SetActive(!menu.activeSelf);
            Time.timeScale = menu.activeSelf ? 0 : 1;
        }
    }

    void ResumeGame()
    {
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = 1;
    }
}