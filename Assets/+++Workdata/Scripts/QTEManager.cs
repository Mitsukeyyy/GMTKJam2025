using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] EggSpell eggspell;
    [SerializeField] EggSwitch eggSwitch;

    private string[] possibleKeys = Enumerable.Range('a', 26)
        .Select(c => ((char)c).ToString()).ToArray();

    private string correctKey;
    private bool QTEActive = false;
    private bool isEggActive = false;

    [SerializeField] TextMeshProUGUI QTEText;
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] GameObject EggKey1;
    [SerializeField] GameObject EggKey2;

    [SerializeField] GameObject QTEContainer;
    [SerializeField] GameObject QTECountdown;
    private int buttonCount = 0;
    public float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QTEContainer.SetActive(false);
        setEggKeysActive(false);
        StartCoroutine(QTE());
        StartCoroutine(Egg());
    }

    void Update()
    {
        updateCountdown();
    }

    void setEggKeysActive(bool state)
    {
        EggKey1.SetActive(state);
        EggKey2.SetActive(state);
        instructionText.gameObject.SetActive(state);
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
            eggspell.FlashSprite();
            string currentEggKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            string currentEggKey2 = possibleKeys[Random.Range(0, possibleKeys.Length)];

            while (currentEggKey2.Equals(currentEggKey))
            {
                currentEggKey2 = possibleKeys[Random.Range(0, possibleKeys.Length)];
            }

            EggKey1.GetComponentInChildren<TextMeshProUGUI>().text = currentEggKey.ToUpper();
            EggKey2.GetComponentInChildren<TextMeshProUGUI>().text = currentEggKey2.ToUpper();
            setEggKeysActive(true);

            yield return new WaitForSeconds(1.5f);

            eggSwitch.TriggerEggSpell();
            instructionText.gameObject.SetActive(false);
            EggKey2.SetActive(false);


            playerController.speed = 0f;
            int lastFrame = 0;

            while (buttonCount < 30)
            {
                if (Input.GetKeyDown(currentEggKey) && buttonCount % 2 == 0)
                {
                    buttonCount++;
                    EggKey1.SetActive(false);
                    EggKey2.SetActive(true);
                    int frameStep = buttonCount / 1;
                    if (frameStep > lastFrame)
                    {
                        eggSwitch.AdvanceEggFrame();
                        lastFrame = frameStep;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                else if (Input.GetKeyDown(currentEggKey2) && buttonCount % 2 == 1)
                {
                    buttonCount++;
                    EggKey2.SetActive(false);
                    EggKey1.SetActive(true);
                    
                    int frameStep = buttonCount / 1;
                    if (frameStep > lastFrame)
                    {
                        eggSwitch.AdvanceEggFrame();
                        lastFrame = frameStep;
                    }
                    yield return new WaitForSeconds(0.01f);
                }

                yield return null;
            }
            for (int i = 2; i < eggSwitch.eggFrames.Length; i++)
            {
                eggSwitch.SetEggFrame(i);
                yield return new WaitForSeconds(0.07f); 
            }
            eggSwitch.RevertToSalamander();
            
            playerController.speed = 2.5f;
            buttonCount = 0;
            setEggKeysActive(false);
            isEggActive = false;
        }

    }

    IEnumerator QTE()
    {
        int x = 0;
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
            QTEContainer.SetActive(true);

            timer = 0f;
            while (QTEActive)
            {
                if (isEggActive)
                {
                    QTEContainer.SetActive(false);
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
                    QTEContainer.SetActive(false);
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
                if (timer > 1.5f)
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
                    x = 0;
                    QTEContainer.SetActive(false);
                    QTEActive = false;
                    break;
                }
                else if (timer > 0.75f && x < 0 && playerController.speed > 0)
                {

                    playerController.speed -= 0.5f;
                    x += 1;
                
                }

                yield return null;
            }
        }
    }

    void updateCountdown() {
        if (QTEActive)
        {
            QTECountdown.SetActive(true);

            float normalizedValue = Mathf.Clamp(timer / 2f, 0.0f, 1.0f);
            
            Image circleImage = QTECountdown.GetComponent<Image>();
            circleImage.fillAmount = normalizedValue;
        }
    }

}