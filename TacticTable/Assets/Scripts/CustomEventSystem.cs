using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventSystem {

    private static CustomEventSystem _instance;
    public static CustomEventSystem Instance { 
        get { 
            if (_instance == null) {
                _instance = new CustomEventSystem();  
            } 
            return _instance;
        }
        private set { _instance = value; } 
    }

    public event Action onDarkModeChange;

    public void ChangeDarkMode()
    {
        if(onDarkModeChange != null)
        {
            onDarkModeChange();
        }
    }

    private CustomEventSystem(){
        

    }
}
