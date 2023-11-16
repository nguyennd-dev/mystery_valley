using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Config")]
    [SerializeField] float _maxSpeedCamera;
    [SerializeField] float _maxRotationSpeedCamera;
    [SerializeField] float _zoomSpeedCamera;
    [SerializeField] float _accelerationCamera;
    [SerializeField] float _stepSize;
    [SerializeField] float _dampingCamera;
    [SerializeField] float _minZoomCamera;
    [SerializeField] float _maxZoomCamera;

    [SerializeField] Transform _tfCamera;

    GameInput _gameInput;

    Vector3 _horizontalVelocity = Vector3.zero;
    Vector3 _lastPosition;
    Vector3 _targetPosition;
    float _speed = 0f;
    float _zoomHeight = 0f;

    void Awake()
    {
        _gameInput = new GameInput();
    }

    void OnEnable()
    {
        _zoomHeight = _tfCamera.localPosition.y;
        _tfCamera.LookAt(this.transform);
        _lastPosition = this.transform.position;
        _gameInput.Camera.Rotation.performed += OnRotateCamera;
        _gameInput.Camera.Zoom.performed += OnZoomCamera;
        _gameInput.Enable();
    }

    void OnDisable()
    {
        _gameInput.Camera.Rotation.performed -= OnRotateCamera;
        _gameInput.Camera.Zoom.performed += OnZoomCamera;
        _gameInput.Disable();
    }

    void OnRotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * _maxRotationSpeedCamera + transform.rotation.eulerAngles.y, 0f);
    }

    void OnZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;
        if (Mathf.Abs(value) > 0.1f)
        {
            _zoomHeight = Mathf.Clamp(_tfCamera.localPosition.y + value * _stepSize, _minZoomCamera, _maxZoomCamera);
        }
    }

    void Update()
    {
        GetTargetMovement();
        UpdateVelocity();
        UpdateZoom();
        UpdatePosition();
    }

    void UpdateVelocity()
    {
        _horizontalVelocity = (this.transform.position - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0;
        _lastPosition = this.transform.position;
    }

    void UpdatePosition()
    {
        if (_targetPosition.sqrMagnitude > 0)
        {
            _speed = Mathf.Lerp(_speed, _maxSpeedCamera, Time.deltaTime * _accelerationCamera);
            transform.position += _targetPosition * _speed * Time.deltaTime;
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _dampingCamera);
            transform.position += _horizontalVelocity * Time.deltaTime;
        }
        _targetPosition = Vector3.zero;
    }

    void UpdateZoom()
    {
        var zoomTarget = new Vector3(_tfCamera.localPosition.x, _zoomHeight, _tfCamera.localPosition.z);
        zoomTarget -= _zoomSpeedCamera * (_zoomHeight - _tfCamera.localPosition.y) * Vector3.forward;
        _tfCamera.localPosition = Vector3.Lerp(_tfCamera.localPosition, zoomTarget, Time.deltaTime * _dampingCamera);
        _tfCamera.LookAt(this.transform);
    }

    void GetTargetMovement()
    {
        var moveInput = _gameInput.Camera.Move.ReadValue<Vector2>();
        Vector3 inputValue = (moveInput.x * GetCameraRight() + moveInput.y * GetCameraForward()).normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            _targetPosition += inputValue;
    }


    #region Utils
    Vector3 GetCameraRight()
    {
        var right = _tfCamera.right;
        right.y = 0;
        return right;
    }

    Vector3 GetCameraForward()
    {
        var right = _tfCamera.forward;
        right.y = 0;
        return right;
    }
    #endregion Utils
}
