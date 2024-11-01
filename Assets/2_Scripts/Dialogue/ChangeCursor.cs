using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // 바꿀 커서 이미지
    public Vector2 hotSpot = Vector2.zero; // 커서 이미지에서 클릭 포인트 좌표
    public CursorMode cursorMode = CursorMode.Auto;

    //void Start()
    //{
    //    // 특정 이벤트에 따라 커서 이미지를 변경하는 예시
    //    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    //}

    // 특정 이벤트 발생 시 커서 이미지 변경
    public void CursorChange()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // 커서를 기본 상태로 되돌리는 방법
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}

