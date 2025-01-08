using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    public Material defaultMaterial;
    public Material outlineMaterial;
    public string targetTag = "Interactable"; // 특정 태그로 필터링
    private GameObject targetObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 오브젝트가 특정 태그를 가지고 있을 때만 처리
        if (collision.CompareTag(targetTag))
        {
            // 항상 마지막으로 들어온 오브젝트를 타겟으로 설정
            if (targetObject != null)
            {
                ApplyOutline(false); // 기존 타겟의 아웃라인 제거
            }

            targetObject = collision.gameObject;
            ApplyOutline(true); // 새 타겟의 아웃라인 적용
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 나가는 오브젝트가 현재 타겟인 경우 처리
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
