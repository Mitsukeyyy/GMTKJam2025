using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button ExitButton;
    [SerializeField] private Button CreditsButton;
    
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject CreditsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        CreditsPanel.SetActive(false);
    }
    void Start()
    {
        MainMenuButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(ExitGame);
       // CreditsPanel.SetActive(false);
    }

    // Update is called once per frame  
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
    }
}
