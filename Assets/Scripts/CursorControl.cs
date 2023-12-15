using System.Collections;
using UnityEngine;

public class CursorControl
{
    public static IEnumerator Control(float delay, CursorLockMode cursorLockMode, bool cursorVisible, float scaleTime)
    {
        yield return new WaitForSeconds(delay);

        Cursor.lockState = cursorLockMode;
        Cursor.visible = cursorVisible;

        Time.timeScale = scaleTime;
    }
}