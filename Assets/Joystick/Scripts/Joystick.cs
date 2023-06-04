using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Joystick : MonoBehaviour
{
    public enum JoystickType
    {
        Static,
        Movable,
    }

    public enum JoystickVisibility
    {
        Visible,
        Invisible
    }

#region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(Joystick))]
    public class JoystickEditor : Editor {
        private SerializedObject _serializedObject;

        private void OnEnable() {
            _serializedObject = new SerializedObject(target);

        }

        public override void OnInspectorGUI()
        {
            Joystick joystick = (Joystick)target;           

            EditorGUILayout.LabelField("Settings");

            HandleJoystickType(joystick);

            joystick.handle.sprite = (Sprite)EditorGUILayout.ObjectField("Handle Image",joystick.handle.sprite,typeof(Sprite), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            joystick.background.sprite = (Sprite)EditorGUILayout.ObjectField("Background Image",joystick.background.sprite,typeof(Sprite), true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            
            joystick.showReferences = EditorGUILayout.Foldout(joystick.showReferences, "References", true);
            if (joystick.showReferences)
                base.OnInspectorGUI();

            EditorUtility.SetDirty(target);
        }

        private void HandleJoystickType(Joystick joystick)
        {
            joystick.joystickVisibility = (JoystickVisibility)EditorGUILayout.EnumPopup("Visibility", joystick.joystickVisibility);

            if (joystick.joystickVisibility == JoystickVisibility.Visible){
                joystick.joystickType = (JoystickType)EditorGUILayout.EnumPopup("Type", joystick.joystickType);
                Color handleColor = joystick.handle.color;
                handleColor.a = 1;
                joystick.handle.color = handleColor;
                joystick.background.enabled = true;
            }else{
                joystick.joystickType = JoystickType.Movable;
                Color handleColor = joystick.handle.color;
                handleColor.a = 0;
                joystick.handle.color = handleColor;
                joystick.background.enabled = false;
            }

            if (joystick.joystickType == JoystickType.Static){
                joystick.touchArea.gameObject.SetActive(false);
            }else{
                joystick.touchArea.gameObject.SetActive(true);
            }
        }
    }
#endif
#endregion

    [SerializeField]private JoystickType joystickType;
    [SerializeField]private JoystickVisibility joystickVisibility;
    [SerializeField]private Canvas canvas;
    [SerializeField]private Image handle;
    [SerializeField]private Image background;
    [SerializeField]private RectTransform touchArea;
    [SerializeField]private RectTransform stickTransform;
    private bool showReferences = false;

    private void Start() {
        GameInput.Instance.OnTouch += GameInput_OnTouch;
        SetJoystickVisibility();
        touchArea.GetComponent<Image>().enabled = false;
    }

    private void SetJoystickVisibility()
    {
        if (joystickVisibility == JoystickVisibility.Invisible)
        {
            Color handleColor = handle.color;
            handleColor.a = 0;
            handle.color = handleColor;
            background.enabled = false;
        }
    }

    private void GameInput_OnTouch(object sender, EventArgs e){
        
        switch (joystickType)
        {
            case JoystickType.Static:
                return;
            case JoystickType.Movable:
                HandleJoystickMovable();
                break;
        }
        

        
    }

    private void HandleJoystickMovable()
    {
        Vector2 touchPositionOnScreen = GameInput.Instance.GetTouchPotitionVector();
        float canvasScale = canvas.transform.localScale.x;
        Vector2 touchPositionInCanvasSpace = touchPositionOnScreen / canvasScale;

        if (RectTransformUtility.RectangleContainsScreenPoint(touchArea, touchPositionOnScreen))
        {
            stickTransform.anchoredPosition = touchPositionInCanvasSpace;
        }
    }
}
