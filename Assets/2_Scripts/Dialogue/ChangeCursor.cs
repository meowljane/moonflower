using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // �ٲ� Ŀ�� �̹���
    public Vector2 hotSpot = Vector2.zero; // Ŀ�� �̹������� Ŭ�� ����Ʈ ��ǥ
    public CursorMode cursorMode = CursorMode.Auto;

    //void Start()
    //{
    //    // Ư�� �̺�Ʈ�� ���� Ŀ�� �̹����� �����ϴ� ����
    //    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    //}

    // Ư�� �̺�Ʈ �߻� �� Ŀ�� �̹��� ����
    public void CursorChange()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // Ŀ���� �⺻ ���·� �ǵ����� ���
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}

