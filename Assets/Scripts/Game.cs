using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    private int unlockedLevels = 0;
    
    void Start()
    {
        instance = this;
        unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 0);
    }

    // Update is called once per frame
    public void SaveUnlockedLevel(int newLevel)
    {
        if (newLevel > unlockedLevels)
        {
            unlockedLevels = newLevel;
            PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        }
    }

    public int GetUnlockedLevel()
    {
        return unlockedLevels;
    }
}
