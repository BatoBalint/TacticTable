using System;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public bool timelinePlays = false;
    public float timeMultiplier = 1.0f;
    public float time = 0.0f;
    private float animationTime = 0.0f;
    public static List<Movement> moves = new List<Movement>();
    public int index = 0;

    private void Update()
    {
        if (!timelinePlays)
            return;

        if (index == moves.Count)
        {
            Stop();
            return;
        }

        IncreaseTime();

        bool animationEnded = false;
        if (time < 0f)
        {
            time = 0;
            animationEnded = true;
        }
        else if (time > 1f)
        {
            time = 1;
            animationEnded = true;
        }

        moves[index].Animate(animationTime);

        if (animationEnded)
            NextAnimation();
    }

    private void IncreaseTime()
    {
        time += Time.deltaTime * (timeMultiplier + Math.Abs(0.5f - time));
        animationTime = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(time);
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

    public static void Add(Movement move)
    {
        moves.Add(move);
    }

    public static void ClearMoves()
    {
        moves.Clear();
    }

    public static void Insert(int moveIndex, int newMoveIndex)
    {
        if (moveIndex == newMoveIndex || moveIndex < 0 || moveIndex >= moves.Count || newMoveIndex < 0 || newMoveIndex > moves.Count)
            return;

        Movement move = moves[moveIndex];
        moves.RemoveAt(moveIndex);

        if (newMoveIndex > moveIndex)
            newMoveIndex--;

        moves.Insert(newMoveIndex, move);
    }
}
