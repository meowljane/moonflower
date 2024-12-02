using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    public Material defaultMaterial;
    public Material outlineMaterial;
    public string targetTag = "Interactable"; // Ư�� �±׷� ���͸�
    private GameObject targetObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������Ʈ�� Ư�� �±׸� ������ ���� ���� ó��
        if (collision.CompareTag(targetTag))
        {
            // �׻� ���������� ���� ������Ʈ�� Ÿ������ ����
            if (targetObject != null)
            {
                ApplyOutline(false); // ���� Ÿ���� �ƿ����� ����
            }

            targetObject = collision.gameObject;
            ApplyOutline(true); // �� Ÿ���� �ƿ����� ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ������ ������Ʈ�� ���� Ÿ���� ��� ó��
        if (targetObject == collision.gameObject)
        {
            ApplyOutline(false);
            targetObject = null;
        }
    }

    private void ApplyOutline(bool apply)
    {
        if (targetObject != null)
        {
            Renderer rend = targetObject.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = apply ? outlineMaterial : defaultMaterial;
            }
        }
    }
}
