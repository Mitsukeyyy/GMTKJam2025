using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject winPanel;
    public GameObject losePanel;
    public Button resumeButton;
    public Button exitButton;
    public Button restartButton1;
    public Button exitButton1;
    public Button restartButton2;
    public Button exitButton2;
    public GameObject QTE;
    public GameObject EggQTE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitGame);
        exitButton1.onClick.AddListener(ExitGame);
        exitButton2.onClick.AddListener(ExitGame);
        restartButton1.onClick.AddListener(RestartLevel);
        restartButton2.onClick.AddListener(RestartLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            menu.SetActive(!menu.activeSelf);
            QTE.SetActive(!menu.activeSelf);
            EggQTE.SetActive(!menu.activeSelf);
            Time.timeScale = menu.activeSelf ? 0 : 1;
        }
    }

    void ResumeGame()
    {
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = 1;
    }

    void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ShowWinPanel()
    {
        QTE.SetActive(false);
        EggQTE.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowLosePanel()
    {
        QTE.SetActive(false);
        EggQTE.SetActive(false);
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }
}