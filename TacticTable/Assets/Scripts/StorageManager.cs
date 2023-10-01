using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StorageManager
{
    private char _seperator = '/';

    public string AppDirectoryPath { get; private set; }
    private string _savedAnimationsPath = "";

    public StorageManager()
    { 
        _seperator = Path.DirectorySeparatorChar;

        AppDirectoryPath = Application.persistentDataPath;
        _savedAnimationsPath = AppDirectoryPath + _seperator + "SavedAnimations";
    }

    public void SaveAnimation(string timelineAsJsonString, string animationName)
    {
        CheckPath(_savedAnimationsPath);
        animationName = animationName.Trim();

        string folderName = FilterDirectoryName(animationName);
        string numbering = "";
        int index = 2;
        while (Directory.Exists(folderName + numbering))
        {
            numbering = index.ToString();
            ++index;
        }
        Directory.CreateDirectory(Combine(_savedAnimationsPath, folderName + numbering));

        Dictionary<string, string> animationDictionary = new Dictionary<string, string>();
        animationDictionary.Add("name", animationName);
        animationDictionary.Add("timeline", timelineAsJsonString);
        string saveAsJsonString = JsonConvert.SerializeObject(animationDictionary);

        StreamWriter sw =
            new StreamWriter(Combine(_savedAnimationsPath, folderName + numbering, "animation.txt"));
        sw.Write(saveAsJsonString);
        sw.Close();
    }

    public void SaveAnimation(Timeline timeline, string animationName)
    {
        SaveAnimation(timeline.ToJSON(), animationName);
    }

    public void DeleteAnimation(string animationName)
    { 
        string path = Combine(_savedAnimationsPath, FilterDirectoryName(animationName));
        if (Directory.Exists(path))
        { 
            Directory.Delete(path, true);
        }
    }

    public string ReadFromSavedAnimations(string animationDirectoryName)
    {
        CheckPath(_savedAnimationsPath);
        string filePath = Combine(new string[] {
            _savedAnimationsPath, 
            animationDirectoryName, 
            "animation.txt"
        });
        return File.ReadAllText(filePath);
    }

    public string[] ListSavedAnimations()
    {
        CheckPath(_savedAnimationsPath);
        return Directory.GetDirectories(_savedAnimationsPath);
    }

    public Dictionary<string, string> GetSavedAnimations()
    {
        Dictionary<string, string> animationNames = new Dictionary<string, string>();
        
        string[] animationDirectoryList = ListSavedAnimations();
        foreach (string animationPath in animationDirectoryList)
        {
            string fileAsText = File.ReadAllText(Combine(animationPath, "animation.txt"));
            Dictionary<string, string> animDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileAsText);
            animationNames.Add(animDic["name"] ?? "Animáció", animDic["timeline"]);
        }

        return animationNames;
    }

    public string[] ListDirectories(string pathInAppDirectory)
    {
        string path = Combine(AppDirectoryPath, pathInAppDirectory);
        if (Directory.Exists(path))
            return Directory.GetDirectories(path);
        else
            return new string[0];
    }

    public string[] ListFiles(string pathInAppDirectory)
    {
        string path = Combine(AppDirectoryPath, pathInAppDirectory);
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

    private string Combine(params string[] paths)
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

    private string FilterDirectoryName(string dirName)
    {
        // https://stackoverflow.com/questions/907995/filter-a-string
        var allowedChars =
            Enumerable.Range('0', 10).Concat(
            Enumerable.Range('A', 26)).Concat(
            Enumerable.Range('a', 26));

        var goodChars = dirName.Where(c => allowedChars.Contains(c));
        return new string(goodChars.ToArray());
    }
}
