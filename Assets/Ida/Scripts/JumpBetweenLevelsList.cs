using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpBetweenLevelsList : MonoBehaviour
{
    [Header("List of levels including main menu in the end!")]
    [SerializeField] private List<string> levels;
    

    public void LoadNextLevel()
    {
        string current_scene_name = SceneManager.GetActiveScene().name;
        int current_index = levels.IndexOf(current_scene_name);
        if (current_index != -1)
        {
            SceneManager.LoadScene(levels[current_index + 1]);
        }
        else
        {
            SceneManager.LoadScene(levels[-1]);
        }
    }
}
