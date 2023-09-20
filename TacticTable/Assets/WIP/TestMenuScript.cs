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
    [SerializeField] private TextMeshProUGUI _debugText;

    private string _appPath;
    private string _testDirPath;

    void Start()
    {
        _appPath = Application.persistentDataPath;
        _testDirPath = _appPath + Path.DirectorySeparatorChar + "Tests";
    }

    void Update()
    {
        
    }

    public void Func1()
    {
        string fileName = "testFile.txt";
        try
        {
            StreamWriter sw = new StreamWriter(_testDirPath + Path.DirectorySeparatorChar + fileName, false);
            sw.WriteLine("This is a test file.");
            sw.WriteLine("Lorem ipsum I guess...");
            sw.WriteLine(DateTime.Now.ToString());
            sw.Close();
            _debugText.text = "Sikeres fajlba iras.";
        }
        catch (System.Exception)
        {
            _debugText.text = "Nem sikerult megnyitni a fajlt.";
        }
    }

    public void Func2()
    {
        if (!Directory.Exists(_testDirPath))
        { 
            Directory.CreateDirectory(_testDirPath);
        }

        string[] dirs = Directory.GetDirectories(_appPath);
        string[] files = Directory.GetFiles(_testDirPath);

        string msg = "Directories:\n";
        foreach (var dir in dirs)
        {
            msg += "\t" + dir + "\n";
        }
        msg += "Files in test directory:\n";
        foreach (var file in files)
        {
            msg += "\t" + file + "\n";
        }
        _debugText.text = msg;
    }

    public void Func3()
    {
        string filePaht = _testDirPath + Path.DirectorySeparatorChar + "testFile.txt";
        if (File.Exists(filePaht))
        {
            string fileAsString = File.ReadAllText(filePaht);
            _debugText.text = fileAsString;
        }
        else
        {
            _debugText.text = "Nem letezik ilyen fajl. " + filePaht;
        }
    }
}
