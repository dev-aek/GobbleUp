using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNew : MonoBehaviour
{

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        LevelManager.Instance.RestartLevel();
        //LevelManager.Instance.ui.UpdateLevelText();
    }

    public void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
    }
}