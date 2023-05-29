using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    
    [SerializeField]private Canvas canvas;

    private void Start() {
        GameInput.Instance.OnTouch += GameInput_OnTouch;
    }

    private void GameInput_OnTouch(object sender, EventArgs e){
        Vector2 touchPositionOnScreen = GameInput.Instance.GetTouchPotitionVector();
        float canvasScale = canvas.transform.localScale.x;
        Vector2 touchPositionInCanvasSpace = (touchPositionOnScreen / canvasScale);
        float screenCenterPositionX = canvas.GetComponent<RectTransform>().rect.width / 2;
        if(touchPositionInCanvasSpace.x < screenCenterPositionX){
            GetComponent<RectTransform>().anchoredPosition = touchPositionInCanvasSpace;
        }
    }
}
