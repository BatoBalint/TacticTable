using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public List<Movement> Moves = new List<Movement>();
    public List<DisksState> DiskStates = new List<DisksState>();

    private TimelineUI _timelineUI;
    
    public int Index = 0;
    public bool TimelinePlays = false;
    public float Time = 0.0f;
    public float TimeMultiplier = 1.0f;
    private float _animationTime = 0.0f;
    
    private int _partialAnimationEndIndex = 0;

    public int SelectedIndex = -1;

    public void Awake()
    {
        _timelineUI = GetComponent<TimelineUI>();
    }

    private void Update()
    {
        if (_partialAnimationEndIndex != 0 && Index == _partialAnimationEndIndex)
        {
            Stop();
            _partialAnimationEndIndex = 0;
        }

        if (TimelinePlays) Animate();
    }

    public void AnimateAtIndex(int moveIndex)
    { 
        _partialAnimationEndIndex = moveIndex + 1;
        Index = moveIndex;
        Play();
    }

    private void Animate()
    {
        if (Index == Moves.Count)
        {
            Stop();
            return;
        }

        IncreaseTime();

        bool timelineEnded = false;
        if (Time < 0f)
        {
            Time = 0;
            timelineEnded = true;
        }
        else if (Time > 1f)
        {
            Time = 1;
            timelineEnded = true;
        }

        bool animationFinished = Moves[Index].Animate(_animationTime);

        if (timelineEnded || animationFinished)
            NextAnimation();
    }

    private void IncreaseTime()
    {
        Time += UnityEngine.Time.deltaTime * (TimeMultiplier + Math.Abs(0.5f - Time));
        _animationTime = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(Time);
    }

    private void NextAnimation()
    {

        if (Time <= 0f)
        {
            if (Index > 0)
            { 
                Index--;
                Time = 1f;
            }    
            else
                Stop();
        }
        else if (Time >= 1f)
        {
            if (Index < Moves.Count - 1)
            { 
                Index++;
                Time = 0f;
            }
            else
                Stop();
        }
        _timelineUI.ReDrawUI();
    }

    private void Stop()
    {
        Pause();
        Time = 0f;
        TimeMultiplier = 1;
        Index = 0;
        _partialAnimationEndIndex = 0;

        _timelineUI.ReDrawUI();
    }

    private void Pause()
    {
        TimelinePlays = false;
    }

    public void Play()
    {
        if (Index == 0 && DiskStates.Count > 0) DiskStates[0].ResetDisksToSavedPosition();

        TimelinePlays = true;
        if (TimeMultiplier == 0)
            TimeMultiplier = 1;

        _timelineUI.ReDrawUI();
    }

    public void SetTimeSpeed(float timeSpeed)
    {
        if (timeSpeed != 0)
        {
            TimeMultiplier = timeSpeed;
            if (timeSpeed < 0)
                Time = 1;
            else if (timeSpeed > 0)
                Time = 0;
        }
        else
        {
            Pause();
        }
    }

    public void Add(Movement move)
    {
        DisksState disksState;
        if (Moves.Count == 0)
        { 
            disksState = new DisksState(DiskScript.DiskScripts);
            DiskStates.Add(disksState);
        }

        Moves.Add(move);
        disksState = new DisksState(DiskScript.DiskScripts);
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

    public void RemoveAtSelection()
    {
        RemoveAt(SelectedIndex);
    }

    public void RemoveAt(int removeIndex)
    {
        if (removeIndex < 0 || removeIndex >= Moves.Count)
            return;

        Moves.RemoveAt(removeIndex);
        if (removeIndex == 0)
            DiskStates.RemoveAt(0);
        else
            DiskStates.RemoveAt(removeIndex + 1);

        if (Moves.Count == 0)
            DiskStates.Clear();

        _timelineUI.ReDrawUI();
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

    public void SelectMovement(int index)
    {
        if (index < 0 || index >= Moves.Count)
            return;

        if (index == SelectedIndex)
        { 
            SelectedIndex = -1;
            return;
        }

        SelectedIndex = index;
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

    public void LoadFromJSON(string jsonString)
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
                DisksState newDiskState = DisksStateFactory.LoadDisksState(stateString);
                DiskStates.Add(newDiskState);
            }
        }

        _timelineUI.ReDrawUI();
    }
}
