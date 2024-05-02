using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // ef teljarinn er st�rri en 0 mun hann telja ni�ur
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            // h�tta a� birta glugga �egar teljarinn er kominn undir 0
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}
