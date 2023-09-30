using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60;
    void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}
