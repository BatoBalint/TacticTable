using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    public bool timelinePlays = false;
    public float timeMultiplier = 1.0f;
    public float time = 0.0f;
    public static List<Movement> moves = new List<Movement>();
    public int index = 0;

    private void Update()
    {
        if (!timelinePlays)
            return;

        time += Time.deltaTime * timeMultiplier;

        bool animationEnded = false;
        if (time < 0)
        {
            time = 0;
            animationEnded = true;
        }
        else if (time > 1)
        {
            time = 1;
            animationEnded = true;
        }

        moves[index].Animate(time);

        if (animationEnded)
            NextAnimation(time);
    }

    private void NextAnimation(float time)
    {
        if (time <= 0)
        {
            if (index > 0)
                index--;
            else
                Pause();
        }
        else if (time >= 1)
        {
            if (index < moves.Count - 1)
                index++;
            else
                Pause();
        }
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

    public void Add(Movement move)
    {
        moves.Add(move);
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
}
