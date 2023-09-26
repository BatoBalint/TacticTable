using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.IO;
using Unity.VisualScripting;
using System;

public class TestMenuScript : MonoBehaviour
{
    // [SerializeField] private DiskScript _disk;
    [SerializeField] private Transform _diskHolder;
    [SerializeField] private Transform _blueDiskPrefab;
    [SerializeField] private Transform _redDiskPrefab;
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private Timeline _timeline;
    [SerializeField] private TimelineUI _timelineUI;

    private string _jsonString = "";

    private StorageManager _storageManager;

    private string _appPath;
    private string _testDirPath;

    void Start()
    {
        _appPath = Application.persistentDataPath;
        _testDirPath = _appPath + Path.DirectorySeparatorChar + "Tests";
        _storageManager = new StorageManager();
    }

    public void Func1()
    {
        LinearMovement move1 = new LinearMovement(_diskHolder.GetChild(0).gameObject, new Vector3(3, 3, 0), new Vector3(3, 0, 0));
        LinearMovement move2 = new LinearMovement(_diskHolder.GetChild(1).gameObject, new Vector3(-3, 3, 0), new Vector3(3, 3, 0));
        LinearMovement move3 = new LinearMovement(_diskHolder.GetChild(2).gameObject, new Vector3(-3, 0, 0), new Vector3(-3, 3, 0));

        _timeline.Add(move1, _diskHolder);
        _timeline.Add(move2, _diskHolder);
        _timeline.Add(move3, _diskHolder);

        _jsonString = _timeline.ToJSON();
        _debugText.text += _jsonString;

        _timelineUI.UpdateTimeline();
    }

    public void Func2()
    {
        _timeline.Play();
    }

    public void Func3()
    {
        _debugText.text = "";
    }

    public void Func4()
    {
        _timeline.ClearMoves();
        _timelineUI.UpdateTimeline();
    }

    public void Func5()
    {
        _timeline.LoadFromJSON(_jsonString, _diskHolder);
        _timelineUI.UpdateTimeline();
    }

    public void Test1()
    {
        List<Dictionary<string, dynamic>> dicList = new List<Dictionary<string, dynamic>>();

        Dictionary<string, dynamic> dic;
        for (int i = 0; i < 5; i++)
        {
            dic = new Dictionary<string, dynamic>();
            dic.Add("index", i);
            dic.Add("funfact", "You cant run from me");
            dicList.Add(dic);
        }

        Dictionary<string, dynamic> dic2 = new Dictionary<string, dynamic>();
        dic2.Add("Name", "Jhon");
        dic2.Add("Age", 19);
        dic2.Add("Hobbies", dicList);

        _debugText.text = JsonConvert.SerializeObject(dic2, Formatting.Indented);
    }
}
