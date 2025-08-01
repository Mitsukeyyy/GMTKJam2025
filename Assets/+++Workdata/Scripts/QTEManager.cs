using System.Collections;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    // private string[] possibleKeys = Enumerable.Range('a', 26)
    //     .Select(c => ((char)c).ToString()).ToArray();

    private string word;
    private string eggWord;
    private string eggWord2;
    private bool QTEActive = false;
    private bool isEggActive = false;
    private WebClient client = new WebClient();
    private string greenColor = "#1fa842";
    private Color redColor = new Color(186, 12, 12);
    private bool eggWordDone = false;
    private bool eggWord2Done = false;

    [SerializeField] TextMeshProUGUI QTEText;
    [SerializeField] TextMeshProUGUI EggText;
    [SerializeField] Image QTEContainer;
    // private int buttonCount = 0;
    public float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setQTEActive(false);
        EggText.gameObject.SetActive(false);
        StartCoroutine(QTE());
        StartCoroutine(Egg());
    }

    string getWord(int max)
    {
        return client.DownloadString($"https://random-word-api.herokuapp.com/word?length={Random.Range(3, max)}").Split('"')[1];
    }
    void setQTEActive(bool state)
    {
        QTEText.gameObject.SetActive(state);
        QTEContainer.gameObject.SetActive(state);
    }

    IEnumerator Egg()
    {
        while (true)
        {
            while (QTEActive)
            {
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(10f, 30f));

            isEggActive = true; 
            playerController.speed = 0f;

            eggWord = getWord(9);
            string tempWord = eggWord;

            eggWord2 = getWord(9);
            string tempWord2 = eggWord2;

            while (eggWord2.Equals(eggWord))
            {
                eggWord2 = getWord(9);
            }

            EggText.text = $"Type: {eggWord} & {eggWord2}";
            EggText.gameObject.SetActive(true);
            // Debug.Log(buttonCount);
            int i = 0;
            int j = 0;
            int k = 6;
            while (!eggWordDone && !eggWord2Done)
            {
                if (Input.GetKeyDown(eggWord[i].ToString()) && !eggWordDone)
                {
                    EggText.text = $"<color={greenColor}>Type: {tempWord.Substring(0, i + 1)}</color>{tempWord.Substring(i+1, tempWord.Length - (i + 1))} & {eggWord2}";
                    i++;
                    k++;

                    if (i == eggWord.Length - 1)
                    {
                        i = 0;
                        k += 3;
                        eggWordDone = true;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                else if (Input.GetKeyDown(eggWord2[j].ToString()) && eggWordDone)
                {
                    EggText.text = $"<color={greenColor}>Type: {eggWord} & {tempWord2.Substring(0, j + 1)}</color>{tempWord2.Substring(j+1, tempWord2.Length - (j+1))}";
                    j++;
                    k++;

                    if (j == eggWord2.Length - 1)
                    {
                        eggWord2Done = true;
                    }
                    yield return new WaitForSeconds(0.01f);

                }

                yield return new WaitForSeconds(0.01f);
            }
            playerController.speed = 3.5f;
            EggText.gameObject.SetActive(false);
            EggText.color = redColor;
            eggWordDone = false;
            eggWord2Done = false;
            isEggActive = false;
        }
        
    }

    IEnumerator QTE()
    {
        while (true)
        {
            while (isEggActive)
            {
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            word = getWord(8);
            string tempWord = word;
            QTEText.text = word;
            QTEActive = true;
            setQTEActive(true);

            timer = 0f;
            int i = 0;
            while (QTEActive)
            {
                if (isEggActive)
                {
                    setQTEActive(false);
                    QTEActive = false;
                    break;
                }

                if (Input.GetKeyDown(word[i].ToString()) && i == word.Length - 1)
                {
                    QTEText.text = $"<color={greenColor}>{word}</color>";
                    Debug.Log("Right");
                    if (playerController.speed < 6f && !Mathf.Approximately(playerController.speed, 5))
                    {
                        playerController.speed = 6f;
                    }
                    if (Mathf.Approximately(playerController.speed, 5))
                    {
                        playerController.speed++;
                    }

                    yield return new WaitForSeconds(0.5f);
                    QTEText.color = redColor;
                    setQTEActive(false);
                    QTEActive = false;

                }
                else if (Input.GetKeyDown(word[i].ToString()))
                {
                    QTEText.text = $"<color={greenColor}>{tempWord.Substring(0, i + 1)}</color>{tempWord.Substring(i+1, tempWord.Length - (i+1))}";
                    i++;
                }

                if (!Input.GetKeyDown(word[i].ToString()) && playerController.speed > 0 && !Input.GetKeyDown(KeyCode.Space))
                {
                    playerController.speed -= 1f;
                    QTEText.color = Color.red;
                    yield return new WaitForSeconds(0.2f);
                }

                timer += Time.deltaTime;
                if (timer >= 5f)
                {
                    if (playerController.speed > 0)
                    {
                        playerController.speed -= 1f;
                        Debug.Log("player didnt click");
                    }

                    timer = 0f;

                    QTEText.color = Color.red;
                    yield return new WaitForSeconds(0.2f);
                    setQTEActive(false);
                    QTEActive = false;
                    break;
                }

                yield return null;
            }
        }
    }
}