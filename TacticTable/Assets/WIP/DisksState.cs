using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DisksState : Movement
{
    private List<DiskScript> _disks;
    private Dictionary<DiskScript, Vector3> _positions;

    public DisksState(List<DiskScript> disks)
    {
        InitDisksState(disks);
    }

    public DisksState(Transform diskContainer)
    {
        List<DiskScript> disks = GetDisks(diskContainer);

        InitDisksState(disks);
    }

    // Moved to this function for future upgrades
    private List<DiskScript> GetDisks(Transform diskContainer)
    {
        List<DiskScript> disks = new List<DiskScript>();
        foreach (Transform t in diskContainer)
        {
            DiskScript diskScript = t.GetComponent<DiskScript>();
            if (diskScript != null) disks.Add(diskScript);
        }
        return disks;
    }

    private void InitDisksState(List<DiskScript> disks)
    {
        _positions = new Dictionary<DiskScript, Vector3>();
        _disks = new List<DiskScript>();
        foreach (DiskScript disk in disks)
        {
            _disks.Add(disk);
        }
        SavePositions();
    }

    public void SavePositions()
    {
        if (_positions == null || _disks == null)
        { 
            _disks = new List<DiskScript>();
            _positions = new Dictionary<DiskScript, Vector3>();
            return;
        }

        _positions.Clear();
        foreach (DiskScript disk in _disks)
        {
            _positions.Add(disk, disk.transform.position);
        }
    }

    public void SetDiskPosition(DiskScript disk, Vector3 pos)
    {
        _positions[disk] = pos;
    }

    public override bool Animate(float time)
    {
        foreach (var pos in _positions)
        {
            pos.Key.transform.position = pos.Value;
        }

        return true;
    }
}
