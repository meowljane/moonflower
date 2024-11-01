using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject DialogueWindow;
    public GameObject TextWindow;
    public GameObject Button;

    public void DeactivateAllWindows()
    {
        DialogueWindow.SetActive(false);
        TextWindow.SetActive(false);
        Button.SetActive(false);
    }

    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateAllWindows();
        windowToOpen.SetActive(true);
    }

    public void CloseWindow()
    {
        Button.SetActive(true);
        DialogueWindow.SetActive(false);
        TextWindow.SetActive(false);
    }
}
