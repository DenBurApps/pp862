using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLoader
{ 
    public static int SelectedDifficulty { get; private set; } = 1;
    public static bool IsTutorialMode { get; private set; } = false;
    
    public static void SetGameSettings(int difficulty, bool tutorialMode)
    {
        SelectedDifficulty = Mathf.Clamp(difficulty, 1, 10);
        IsTutorialMode = tutorialMode;
    }

    public static void ResetTutorialMode()
    {
        IsTutorialMode = false;
    }
}
