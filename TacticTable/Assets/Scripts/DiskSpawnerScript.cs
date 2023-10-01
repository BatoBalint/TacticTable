using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskSpawnerScript : MonoBehaviour
{
    [SerializeField] private Transform _blueDisk;
    [SerializeField] private Transform _redDisk;
    [SerializeField] private int _diskAmount;
    [SerializeField] private bool _isBlue;

    public void SpawnDisks()
    {
        for (int i = 0; i < _diskAmount; i++)
        {
            Transform newDisk = Instantiate(_isBlue ? _blueDisk : _redDisk, transform);
            newDisk.position = new Vector3(-6 + (i * 2), _isBlue ? 2 : 0, 0);
        }
    }
}
