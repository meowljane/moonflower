using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    private void OnEnable()
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}