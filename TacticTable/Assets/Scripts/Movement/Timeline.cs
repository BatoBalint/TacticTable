using System;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public bool timelinePlays = false;
    public float timeMultiplier = 1.0f;
    public float time = 0.0f;
    private float _animationTime = 0.0f;
    public List<Movement> moves = new List<Movement>();
    public List<DisksState> disksStates = new List<DisksState>();
    public int index = 0;
    private int _partialAnimationEndIndex = 0;

    private void Update()
    {
        if (_partialAnimationEndIndex != 0 && index == _partialAnimationEndIndex)
        {
            Stop();
            _partialAnimationEndIndex = 0;
        }

        if (timelinePlays) Animate();
    }

    public void AnimateAtIndex(int moveIndex)
    { 
        _partialAnimationEndIndex = moveIndex + 1;
        index = moveIndex;
        Play();
    }

    private void Animate()
    {
        if (index == moves.Count)
        {
            Stop();
            return;
        }

        IncreaseTime();

        bool timelineEnded = false;
        if (time < 0f)
        {
            time = 0;
            timelineEnded = true;
        }
        else if (time > 1f)
        {
            time = 1;
            timelineEnded = true;
        }

        bool animationFinished = moves[index].Animate(_animationTime);

        if (timelineEnded || animationFinished)
            NextAnimation();
    }

    private void IncreaseTime()
    {
        time += Time.deltaTime * (timeMultiplier + Math.Abs(0.5f - time));
        _animationTime = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(time);
    }

    private void NextAnimation()
    {
        if (time <= 0f)
        {
            if (index > 0)
            { 
                index--;
                time = 1f;
            }    
            else
                Stop();
        }
        else if (time >= 1f)
        {
            if (index < moves.Count - 1)
            { 
                index++;
                time = 0f;
            }
            else
                Stop();
        }
    }

    private void Stop()
    {
        Pause();
        time = 0f;
        timeMultiplier = 1;
        index = 0;
    }

    private void Pause()
    {
        timelinePlays = false;
    }

    public void Play()
    {
        if (index == 0) disksStates[0].ResetDisksToSavedPosition();

        timelinePlays = true;
        if (timeMultiplier == 0)
            timeMultiplier = 1;
    }

    public void SetTimeSpeed(float timeSpeed)
    {
        if (timeSpeed != 0)
        {
            timeMultiplier = timeSpeed;
            if (timeSpeed < 0)
                time = 1;
            else if (timeSpeed > 0)
                time = 0;
        }
        else
        {
            Pause();
        }
    }

    public void Add(Movement move, Transform diskHolder)
    {
        moves.Add(move);
        DisksState disksState = new DisksState(diskHolder);
        foreach (var disk in move.GetEndPositions())
        {
            disksState.SetDiskPosition(disk.Key, disk.Value);
        }
        disksStates.Add(disksState);
    }

    public void SaveDiskPositions(Transform diskHolder)
    {
        disksStates.Add(new DisksState(diskHolder));
    }

    public void ClearMoves()
    {
        while (moves.Count > 0)
        {
            moves.RemoveAt(0);
        }
    }

    public void RemoveAt(int removeIndex)
    {
        if (removeIndex < 1 || removeIndex > moves.Count - 1)
            return;

        moves.RemoveAt(removeIndex);
        disksStates.RemoveAt(removeIndex + 1);
    }

    public void Insert(int moveIndex, int newMoveIndex)
    {
        if (moveIndex == newMoveIndex || moveIndex < 0 || moveIndex >= moves.Count || newMoveIndex < 0 || newMoveIndex > moves.Count)
            return;

        Movement move = moves[moveIndex];
        moves.RemoveAt(moveIndex);

        if (newMoveIndex > moveIndex)
            newMoveIndex--;

        moves.Insert(newMoveIndex, move);
    }

    public Dictionary<string, dynamic> ToJSON()
    {
        Dictionary<string, dynamic> jsonDictionary = new Dictionary<string, dynamic>();
        

        return jsonDictionary;
    }
}
