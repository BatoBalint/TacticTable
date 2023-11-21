using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionScalerScript : MonoBehaviour
{
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private Transform _redDisks;
    [SerializeField] private Transform _blueDisks;
    [SerializeField] private Transform _ball;
    [SerializeField] private bool _moveBall = true;

    public void ScalePositions()
    {
        Transform t = transform;

        float growthScale = Camera.main.aspect / (1920f / 1080);
        t.position = new Vector3(transform.position.x * growthScale, transform.position.y, transform.position.z);
        t.localScale = new Vector3(growthScale, 1, 1);
    }

    public void MoveDisksToDefaults()
    {
        PlayersToStartingPosition();
        AdditionalDisksToStartingPosition();
    }

    private void PlayersToStartingPosition()
    {
        if (_startingPosition.childCount < 2)
            return;

        Transform redPositions = _startingPosition.GetChild(0);
        Transform bluePositions = _startingPosition.GetChild(1);

        for (int i = 0; i < redPositions.childCount && i < _redDisks.childCount; i++)
        {
            _redDisks.GetChild(i).position = redPositions.GetChild(i).position;
        }

        for (int i = 0; i < bluePositions.childCount && i < _blueDisks.childCount; i++)
        {
            _blueDisks.GetChild(i).position = bluePositions.GetChild(i).position;
        }
    }

    private void AdditionalDisksToStartingPosition()
    {
        // the "magic" numbers are pre tested numbers from the editor (-3f, -4f, offset)
        float offset = 0.7f;

        // Subtract 200 because of margin center (400 / 2)
        int screenPos = Camera.main.pixelWidth - 200;
        Vector3 calculatedPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos, 0, 0));

        if (_redDisks.childCount > 6)
            _redDisks.GetChild(_redDisks.childCount - 1).position = new Vector3(calculatedPos.x - offset, -3f, 0);

        if (_blueDisks.childCount > 6)
            _blueDisks.GetChild(_blueDisks.childCount - 1).position = new Vector3(calculatedPos.x + offset, -3f, 0);

        if (_moveBall) _ball.position = new Vector3(calculatedPos.x, -4f, 0);
    }

    // Returns the growth scale or 1 if couldn't find a main camera
    public static float GetWidthGrowthScale()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
            return mainCamera.pixelWidth / 1920f;
        else
            return 1f;
    }
}