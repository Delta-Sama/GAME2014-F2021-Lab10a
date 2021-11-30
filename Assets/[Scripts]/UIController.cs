using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject OnScreenControls;

    [Header("Button Events")]
    public static bool jump = false;

    private void Start()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
            case RuntimePlatform.WindowsEditor:
                {
                    OnScreenControls.SetActive(true);
                    break;
                }
            default:
                {
                    OnScreenControls.SetActive(false);
                    break;
                }
        }
    }

    public void OnJumpButton_Down()
    {
        jump = true;

    }

    public void OnJumpButton_Up()
    {
        jump = false;
    }
}
