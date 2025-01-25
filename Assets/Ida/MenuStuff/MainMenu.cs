using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource startGameSound;



    private IEnumerator startGameAfterSound()
    {
        if (startGameSound != null && startGameSound.isActiveAndEnabled)
        {
            yield return new WaitUntil(() => startGameSound.time > 0); //When Closing Sound started playing
            yield return new WaitUntil(() => startGameSound.time == 0); //Closing Sound stopped playing
        }
        
    }
    public void LoadLevel(string levelName)
    {
        StartCoroutine(startGameAfterSound());
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
