using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.IO;
using Unity.VisualScripting;
using System;

public class TestMenuScript : MonoBehaviour
{
    [SerializeField] private DiskScript _disk;
    [SerializeField] private Transform _blueDiskPrefab;
    [SerializeField] private Transform _redDiskPrefab;
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private Timeline _timeline;

    private Transform _diskHolder;

    private StorageManager _storageManager;

    private string _appPath;
    private string _testDirPath;

    void Start()
    {
        _appPath = Application.persistentDataPath;
        _testDirPath = _appPath + Path.DirectorySeparatorChar + "Tests";
        _diskHolder = _disk.transform.parent;
        _storageManager = new StorageManager();
    }

    public void Func1()
    {
        List<Dictionary<string, string>> diskExports = 
            new List<Dictionary<string, string>>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("color", _disk.IsBlue ? "blue" : "red");
        dic.Add("x", _disk.transform.position.x.ToString());
        dic.Add("y", _disk.transform.position.y.ToString());
        diskExports.Add(dic);
        for (int i = 0; i < 4; i++)
        {
            dic = new Dictionary<string, string>();
            
            string color = i % 2 == 0 ? "blue" : "red";
            float x = _disk.transform.position.x + (-4 + (i < 2 ? i : i + 1) * 2);
            float y = _disk.transform.position.y;
            dic.Add("color", color);
            dic.Add("x", x.ToString());
            dic.Add("y", y.ToString());

            diskExports.Add(dic);
        }
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings();

        string savedJson = JsonConvert.SerializeObject(diskExports, Formatting.Indented);

        WriteToTestDir("testFile.txt", savedJson);
    }

    public void Func2()
    {
        string filePath = _testDirPath + Path.DirectorySeparatorChar + "testFile.txt";

        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            List<Dictionary<string, string>> diskImports =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);

            SpawnDisks(diskImports);
        }
        else
        {
            _debugText.text = "Nem letezik ilyen fajl: " + filePath;
        }
    }

    private void SpawnDisks(List<Dictionary<string, string>> diskImports)
    {
        Transform newDisk;

        foreach (var diskData in diskImports)
        {
            if ((diskData["color"] ?? "") == "blue")
                newDisk = Instantiate(_blueDiskPrefab);
            else
                newDisk = Instantiate(_redDiskPrefab);

            float x, y;
            if (!float.TryParse(diskData["x"], out x)) x = 0f;
            if (!float.TryParse(diskData["y"], out y)) y = 0f;

            newDisk.position = new Vector3(x, y, 0);
            newDisk.parent = _diskHolder;
        }
    }

    public void Func3()
    {
        string filePath = _testDirPath + Path.DirectorySeparatorChar + "testFile.txt";
        if (File.Exists(filePath))
        {
            string fileAsString = File.ReadAllText(filePath);
            _debugText.text = fileAsString;
        }
        else
        {
            _debugText.text = "Nem letezik ilyen fajl: " + filePath;
        }
    }

    private void WriteToTestDir(string fileName, string data)
    {
        if (!Directory.Exists(_testDirPath))
            Directory.CreateDirectory(_testDirPath);

        try
        {
            StreamWriter sw = new StreamWriter(_testDirPath + Path.DirectorySeparatorChar + fileName, false);
            sw.Write(data);
            sw.Close();
        }
        catch (Exception ex)
        {
            _debugText.text += "Nem sikerult letrehozni/megnyitni a fajlt: " + fileName;
            _debugText.text += ex.Message;
        }
    }

    public void Func4()
    {
        string[] dirs = _storageManager.ListDirectories("Tests");
        string[] files = _storageManager.ListFiles("Tests");

        foreach (var item in dirs)
        {
            _debugText.text += item + "\n";
        }

        foreach (var item in files)
        {
            _debugText.text += item + "\n";
        }
    }

    public void Func5()
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
