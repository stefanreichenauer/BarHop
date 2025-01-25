using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SubMenu : MonoBehaviour
{
    [SerializeField] GameObject subMenuCanvas;
    [SerializeField] GameObject firstSelectedButton;
    [SerializeField] GameObject selectAfterClose;
    [SerializeField] AudioSource closingSound;


    private EventSystem eventSystem;
    private GameObject selectedBeforeOpened;

    public void Awake()
    {
        eventSystem = EventSystem.current;
    }

    public void OpenSubMenu()
    {
        subMenuCanvas.SetActive(true);

        if (selectAfterClose == null)
        {
            selectedBeforeOpened = eventSystem.currentSelectedGameObject;
        }
        eventSystem.SetSelectedGameObject(firstSelectedButton);

    }

    public void CloseSubMenu()
    {
        StartCoroutine(closeMenuAfterCloseSound());
    }

    private IEnumerator closeMenuAfterCloseSound()
    {
        if (closingSound != null && closingSound.isActiveAndEnabled)
        {
            yield return new WaitUntil(() => closingSound.time > 0); //When Closing Sound started playing
            yield return new WaitUntil(() => closingSound.time == 0); //Closing Sound stopped playing
        }

        subMenuCanvas.SetActive(false);
        if (selectAfterClose == null)
        {
            eventSystem.SetSelectedGameObject(selectedBeforeOpened);
        }
        else
        {
            eventSystem.SetSelectedGameObject(selectAfterClose);
        }

    }


}
