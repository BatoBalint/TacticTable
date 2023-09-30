using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public bool timelinePlays = false;
    public float timeMultiplier = 1.0f;
    public float time = 0.0f;
    private float _animationTime = 0.0f;
    public List<Movement> Moves = new List<Movement>();
    public List<DisksState> DiskStates = new List<DisksState>();
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
        if (index == Moves.Count)
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

        bool animationFinished = Moves[index].Animate(_animationTime);

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
            if (index < Moves.Count - 1)
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
        if (index == 0 && DiskStates.Count > 0) DiskStates[0].ResetDisksToSavedPosition();

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
        DisksState disksState;
        if (Moves.Count == 0)
        { 
            disksState = new DisksState(diskHolder);
            DiskStates.Add(disksState);
        }

        Moves.Add(move);
        disksState = new DisksState(diskHolder);
        foreach (var disk in move.GetEndPositions())
        {
            disksState.SetDiskPosition(disk.Key, disk.Value);
        }
        DiskStates.Add(disksState);
    }

    public void ClearMoves()
    {
        Moves.Clear();
        DiskStates.Clear();
    }

    public void RemoveAt(int removeIndex)
    {
        if (removeIndex < 0 || removeIndex > Moves.Count - 1)
            return;

        Moves.RemoveAt(removeIndex);
        if (removeIndex == 0)
            DiskStates.RemoveAt(0);
        else
            DiskStates.RemoveAt(removeIndex + 1);
    }

    public void Insert(int moveIndex, int newMoveIndex)
    {
        if (moveIndex == newMoveIndex || moveIndex < 0 || moveIndex >= Moves.Count || newMoveIndex < 0 || newMoveIndex >= Moves.Count)
            return;

        Movement move = Moves[moveIndex];
        Moves.RemoveAt(moveIndex);

        if (newMoveIndex > moveIndex)
            newMoveIndex--;

        Moves.Insert(newMoveIndex, move);
    }

    public string ToJSON()
    {
        Dictionary<string, string> jsonDictionary = new Dictionary<string, string>();
        
        // Save moves
        List<string> movesAsJsonString = new List<string>();
        foreach (var move in Moves)
        {
            movesAsJsonString.Add(move.ToJSON());
        }
        jsonDictionary.Add("moves", JsonConvert.SerializeObject(movesAsJsonString));

        // Save diskstates
        List<string> diskStatesAsJsonString = new List<string>();
        foreach (var diskState in DiskStates)
        {
            diskStatesAsJsonString.Add(diskState.ToJSON());
        }
        jsonDictionary.Add("disksStates", JsonConvert.SerializeObject(diskStatesAsJsonString));

        return JsonConvert.SerializeObject(jsonDictionary);
    }

    public void LoadFromJSON(string jsonString, Transform diskHolder)
    {
        // Terminate if there are no disks in the scene
        if (DiskScript.DiskScripts.Count == 0)
            return;

        ClearMoves();

        Dictionary<string, string> timelineDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

        // Load movements if the list is in the dictionary
        if (timelineDic["moves"] != null)
        {
            List<string> movesList = JsonConvert.DeserializeObject<List<string>>(timelineDic["moves"]);
            foreach (var moveString in movesList)
            {
                Movement move = MovementFactory.LoadMovement(moveString);
                if (move != null)
                { 
                    Moves.Add(move);
                } 
            }
        }

        // Load diskstates if the list is in the dictionary
        if (timelineDic["disksStates"] != null)
        {
            List<string> diskStatesList = JsonConvert.DeserializeObject<List<string>>(timelineDic["disksStates"]);
            foreach (var stateString in diskStatesList)
            {
                DiskStates.Add(DisksStateFactory.LoadDisksState(stateString));
            }
        }
    }
}
