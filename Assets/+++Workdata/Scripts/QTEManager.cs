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
    private bool QTEActive = false;

    [SerializeField] TextMeshProUGUI QTEText;
    private int buttonCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QTEText.gameObject.SetActive(false);
        StartCoroutine(QTE());
     //   StartCoroutine(Egg());
    }

    IEnumerator Egg()
    {
        while (true)
        {
           yield return new WaitForSeconds(10f);
                   playerController.speed = 0f;
           
                   correctKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
                   QTEText.text = "Press: " + correctKey.ToUpper();
                   QTEText.gameObject.SetActive(true);
                   Debug.Log(buttonCount);
                   
                   while (buttonCount < 20)
                   {
                       if (Input.GetKeyDown(correctKey))
                       {
                           buttonCount++;
                           QTEText.text = correctKey.ToUpper();
                           Debug.Log(buttonCount);
                           yield return new WaitForSeconds(0.01f);
                       }
           
                       yield return null;
                   }
                   playerController.speed = 3.5f;
                   buttonCount = 0;
                   QTEText.gameObject.SetActive(false); 
        }
        
    }

    IEnumerator QTE()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            correctKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            QTEText.text = "Press: " + correctKey.ToUpper();
            QTEActive = true;
            QTEText.gameObject.SetActive(true);

            while (QTEActive)
            {
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

                yield return null;
            }
        }
    }
}