using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] DragInputEvent _dragInput;

    GameInput _gameInput;

    Vector2 _screenPosition = Vector2.zero;
    UnityEngine.InputSystem.TouchPhase _touchPhase = UnityEngine.InputSystem.TouchPhase.Canceled;

    void Awake()
    {
        _gameInput = new GameInput();
        _touchPhase = UnityEngine.InputSystem.TouchPhase.Canceled;
    }

    void OnEnable()
    {
        _gameInput.Drag.Position.performed += OnMouseMove;
        _gameInput.Enable();
    }

    void OnDisable()
    {
        _gameInput.Drag.Position.performed -= OnMouseMove;
        _gameInput.Disable();
    }

    public void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            switch (_touchPhase)
            {
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    _touchPhase = UnityEngine.InputSystem.TouchPhase.Began;
                    _dragInput?.RaiseBeginDrag(_screenPosition);
                    break;

                case UnityEngine.InputSystem.TouchPhase.Began:
                case UnityEngine.InputSystem.TouchPhase.Moved:
                    _touchPhase = UnityEngine.InputSystem.TouchPhase.Moved;
                    _dragInput?.RaiseDrag(_screenPosition);
                    break;
            }
        }
        else
        {
            switch (_touchPhase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                case UnityEngine.InputSystem.TouchPhase.Moved:
                    _touchPhase = UnityEngine.InputSystem.TouchPhase.Ended;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                    _dragInput?.RaiseEndDrag(_screenPosition);
                    _touchPhase = UnityEngine.InputSystem.TouchPhase.Canceled;
                    break;
            }
        }
    }

    private void OnMouseMove(InputAction.CallbackContext inputValue)
    {
        _screenPosition = inputValue.ReadValue<Vector2>();
    }
}
