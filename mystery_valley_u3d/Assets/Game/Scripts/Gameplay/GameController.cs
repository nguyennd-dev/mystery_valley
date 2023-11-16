using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Grid _gridController;

    [Header("Events")]
    [SerializeField] DragInputEvent _dragInput;

    public void Setup()
    {

    }
}
