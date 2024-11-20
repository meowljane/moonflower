using UnityEngine;
using UnityEngine.UI;

public class LastQuizMethod : MonoBehaviour
{
    public RectTransform rectTransform;

    public void CollectAndLogValues()
    {
        int index = 0;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
           
            var inputField = child.GetComponent<InputField>();
            if (inputField != null)
            {
                if (string.IsNullOrEmpty(inputField.text))
                {
                    Debug.Log("값이 없습니다.");
                    return; 
                }
                index++;
                
                continue;
            }
        }

        ShowAnswer();
    }

    public void ShowAnswer()
    {
        Debug.Log("다했네");
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            var inputField = child.GetComponent<InputField>();
            if (inputField != null)
            {
                inputField.interactable = false;
            }
        }
        foreach (Transform child in rectTransform)
        {
            if (child.CompareTag("AnswerPannel"))
            {
                child.gameObject.SetActive(true);
            }
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 2850f);
    }
}
