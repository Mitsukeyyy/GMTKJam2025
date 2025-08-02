using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button ExitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenuButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void ExitGame()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
