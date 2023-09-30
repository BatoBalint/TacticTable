using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskSpawnerScript : MonoBehaviour
{
    [SerializeField] private Transform _blueDisk;
    [SerializeField] private Transform _redDisk;
    [SerializeField] private Transform _startPos;
    [SerializeField] private int _diskAmount;
    [SerializeField] private bool _isBlue;

    void Start()
    {
        for (int i = 0; i < _diskAmount; i++)
        {
            Transform newDisk = Instantiate(_isBlue ? _blueDisk : _redDisk, transform);
            newDisk.position = new Vector3(-6 + (i * 2), _isBlue ? 2 : 0, 0);
        }

        if (_startPos.childCount < 2) return;
        Transform positions = _isBlue ? _startPos.GetChild(1) : _startPos.GetChild(0);

        for (int i = 0; i < positions.childCount && i < transform.childCount; i++)
        {
            transform.GetChild(i).position = positions.GetChild(i).position;
        }
    }
}
