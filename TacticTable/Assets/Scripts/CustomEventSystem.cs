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

    public event Action<DarkMode> onDarkModeChange;

    public void ChangeDarkMode(DarkMode dm)
    {
        if(onDarkModeChange != null)
        {
            onDarkModeChange(dm);
        }
    }

    private CustomEventSystem(){
        

    }
}
