using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnClickSceneMove : MonoBehaviour
{
    [SerializeField]
    private string moveScene;
    public void BtnClickSceneMv()
    {
        SceneManager.LoadScene(moveScene);
    }
}
