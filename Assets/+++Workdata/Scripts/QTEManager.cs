using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private string[] possibleKeys = Enumerable.Range('a', 26)
        .Select(c => ((char)c).ToString()).ToArray();

    private string correctKey;
    private string eggKey;
    private string eggKey2;
    private bool QTEActive = false;
    private bool isEggActive = false;

    [SerializeField] TextMeshProUGUI QTEText;
    [SerializeField] TextMeshProUGUI EggText;
    [SerializeField] Image QTEContainer;
    [SerializeField] GameObject circleTimer;
    private int buttonCount = 0;
    public float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setQTEActive(false);
        EggText.gameObject.SetActive(false);
        StartCoroutine(QTE());
        StartCoroutine(Egg());
    }

    void Update()
    {
        updateCountdown();
    }

    void setQTEActive(bool state)
    {
        QTEText.gameObject.SetActive(state);
        QTEContainer.gameObject.SetActive(state);
        circleTimer.SetActive(false);
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
            eggKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            eggKey2 = possibleKeys[Random.Range(0, possibleKeys.Length)];
            while (eggKey2.Equals(eggKey))
            {
                eggKey2 = possibleKeys[Random.Range(0, possibleKeys.Length)];
            }

            EggText.text = $"Press: {eggKey.ToUpper()} & {eggKey2.ToUpper()}";
            EggText.gameObject.SetActive(true);
            // Debug.Log(buttonCount);

            while (buttonCount < 35)
            {
                if (Input.GetKeyDown(eggKey) && buttonCount % 2 == 0)
                {
                    buttonCount++;
                    EggText.text = eggKey2.ToUpper();
                    Debug.Log(buttonCount);
                    yield return new WaitForSeconds(0.01f);
                }
                else if (Input.GetKeyDown(eggKey2) && buttonCount % 2 == 1)
                {
                    buttonCount++;
                    EggText.text = eggKey.ToUpper();
                    Debug.Log(buttonCount);
                    yield return new WaitForSeconds(0.01f);
                }

                yield return null;
            }
            playerController.speed = 2.5f;
            buttonCount = 0;
            EggText.gameObject.SetActive(false);
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

            correctKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            QTEText.text = correctKey.ToUpper();
            QTEActive = true;
            setQTEActive(true);

            timer = 0f;
            while (QTEActive)
            {
                if (isEggActive)
                {
                    setQTEActive(false);
                    QTEActive = false;
                    break;
                }

                if (Input.GetKeyDown(correctKey))
                {
                    playerController.speed++;
                    QTEText.color = Color.green;
                    Debug.Log(correctKey);
                    QTEText.text = correctKey.ToUpper();
                    yield return new WaitForSeconds(0.5f);
                    QTEText.color = Color.white;
                    setQTEActive(false);
                    QTEActive = false;
                }

                foreach (string key in possibleKeys)
                {
                    if (key != correctKey && Input.GetKeyDown(key) && playerController.speed > 0)
                    {
                        playerController.speed -= 1f;
                        QTEText.color = Color.red;
                        yield return new WaitForSeconds(0.2f);
                        QTEText.color = Color.white;
                        Debug.Log(key);
                    }
                }

                timer += Time.deltaTime;
                if (timer >= 3f)
                {
                    if (playerController.speed > 0)
                    {
                        playerController.speed -= 1f;
                        Debug.Log("player didnt click");
                    }

                    timer = 0f;

                    QTEText.color = Color.red;
                    yield return new WaitForSeconds(0.2f);
                    QTEText.color = Color.white;
                    setQTEActive(false);
                    QTEActive = false;
                    break;
                }

                yield return null;
            }
        }
    }

    void updateCountdown() {
        if (QTEActive)
        {
            circleTimer.SetActive(true);

            float normalizedValue = Mathf.Clamp(timer / 3f, 0.0f, 1.0f);
            
            Image circleImage = circleTimer.GetComponent<Image>();
            circleImage.fillAmount = normalizedValue;
        }
    }

}