using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private string[] possibleKeys = Enumerable.Range('a', 26)
        .Select(c => ((char)c).ToString())
        .ToArray();

    private string correctKey;
    private string eggKey;
    private bool QTEActive = false;
    private bool isEggActive = false;

    [SerializeField] TextMeshProUGUI QTEText;
    [SerializeField] TextMeshProUGUI EggText;
    private int buttonCount = 0;
    public float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QTEText.gameObject.SetActive(false);
        EggText.gameObject.SetActive(false);
        StartCoroutine(QTE());
        StartCoroutine(Egg());
    }

    IEnumerator Egg()
    {
        while (true)
        {
           yield return new WaitForSeconds(10f);

           isEggActive = true; 
                   playerController.speed = 0f;
           
                   eggKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
                   EggText.text = "Press: " + eggKey.ToUpper();
                   EggText.gameObject.SetActive(true);
                   Debug.Log(buttonCount);
                   
                   while (buttonCount < 20)
                   {
                       if (Input.GetKeyDown(eggKey))
                       {
                           buttonCount++;
                           EggText.text = eggKey.ToUpper();
                           Debug.Log(buttonCount);
                           yield return new WaitForSeconds(0.01f);
                       }
           
                       yield return null;
                   }
                   playerController.speed = 3.5f;
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
            yield return new WaitForSeconds(3f);

            correctKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            QTEText.text = "Press: " + correctKey.ToUpper();
            QTEActive = true;
            QTEText.gameObject.SetActive(true);
            

            while (QTEActive)
            {
                if (isEggActive)
                {
                    QTEText.gameObject.SetActive(false);
                    QTEActive = false;
                    break;
                }

                if (Input.GetKeyDown(correctKey))
                {
                    if (playerController.speed < 6f && !Mathf.Approximately(playerController.speed, 5) )
                    {
                        playerController.speed += 2f;
                    }
                    if (Mathf.Approximately(playerController.speed, 5))
                    {
                        playerController.speed++;
                    }

                    QTEText.color = Color.green;
                    Debug.Log(correctKey);
                    QTEText.text = correctKey.ToUpper();
                    yield return new WaitForSeconds(0.5f);
                    QTEText.color = Color.white;
                    QTEText.gameObject.SetActive(false);
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
                
                timer  += Time.deltaTime;
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
                    QTEText.gameObject.SetActive(false);
                    QTEActive = false;
                    break;
                }

                yield return null;
            }
        }
    }
}