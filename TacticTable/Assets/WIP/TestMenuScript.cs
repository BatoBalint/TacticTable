using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TestMenuScript : MonoBehaviour
{
    struct MovementStruct
    {
        public string MoveType;
        public List<DiskStruct> Disks;
        public List<Vector3> Positions;
    }

    struct DiskStruct
    {
        public bool IsBlue;
        public Vector3 Pos;
    }

    [SerializeField] private DiskScript _disk;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Func1()
    {
        DiskStruct diskStruct = new DiskStruct();
        diskStruct.IsBlue = true;
        diskStruct.Pos = Vector3.up;

        MovementStruct moveStruct = new MovementStruct();
        moveStruct.Disks = new List<DiskStruct>();
        moveStruct.Disks.Add(diskStruct);
        moveStruct.Positions = new List<Vector3>();
        moveStruct.Positions.Add(Vector3.up);
        moveStruct.Positions.Add(Vector3.right);
        moveStruct.MoveType = "LinearMovement";

        Dictionary<string, dynamic> dir = new Dictionary<string, dynamic>();
        dir.Add("alma", 3);
        dir.Add("disks", new List<int>() { 1, 2, 3 });

        string json = JsonConvert.SerializeObject(moveStruct);

        //string json = JsonConvert.SerializeObject(dir);

        Debug.Log(json);
    }

    public void Func2()
    {
        Debug.Log("Func2");
    }

    public void Func3()
    {
        Debug.Log("Func3");
    }
}
