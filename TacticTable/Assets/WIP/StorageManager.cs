using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StorageManager
{
    private char _seperator = '/';

    public string AppDirectoryPath { get; private set; }
    private string _savedAnimationsPath = "";

    // Test
    private string _testDirPath = "";

    public StorageManager()
    { 
        _seperator = Path.DirectorySeparatorChar;

        AppDirectoryPath = Application.persistentDataPath;
        _savedAnimationsPath = AppDirectoryPath + _seperator + "SavedAnimations";


        //Test
        _testDirPath = AppDirectoryPath + _seperator + "Tests";
    }

    public string ReadFromSavedAnimations(string animationName)
    {
        CheckPath(_savedAnimationsPath);
        string filePath = Combine(new string[] {
            _savedAnimationsPath, 
            animationName, 
            "animation.txt"
        });
        return File.ReadAllText(filePath);
    }

    public string[] ListSavedAnimations()
    { 
        return Directory.GetDirectories(_savedAnimationsPath);
    }

    public string[] ListDirectories(string pathInAppDirectory)
    {
        string path = Combine(new string[] { AppDirectoryPath, pathInAppDirectory });
        if (Directory.Exists(path))
            return Directory.GetDirectories(path);
        else
            return new string[0];
    }

    public string[] ListFiles(string pathInAppDirectory)
    {
        string path = Combine(new string[] { AppDirectoryPath, pathInAppDirectory });
        if (Directory.Exists(path))
            return Directory.GetFiles(path);
        else
            return new string[0];
    }

    private void CheckPath(string path)
    { 
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private string Combine(string[] paths)
    {
        string seperator = "";
        string resultPath = "";
        foreach (var path in paths)
        {
            resultPath += seperator + path;
            seperator = _seperator + "";
        }
        return resultPath;
    }

    //Test
    public string ReadFromTest(string filename)
    {
        CheckPath(_testDirPath);
        string filePath = _testDirPath + _seperator + filename;

        return File.ReadAllText(filePath);
    }
}
