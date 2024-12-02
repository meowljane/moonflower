using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    //Ȱ��ȭ, ��Ȱ��ȭ�� ������Ʈ��
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
