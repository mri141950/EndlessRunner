using UnityEngine;

public static class InputManager
{
    public static bool JumpPressed => Input.GetButtonDown("Jump");
    public static bool SlidePressed => Input.GetButtonDown("Slide");
}
