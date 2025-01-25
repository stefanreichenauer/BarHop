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

    [SerializeField] private bool is_pause_menu = false;

    public static SubMenu currentOpenMenu = null;
    private SubMenu myPreviousSubMenu;
    private PlayerInput playerInput;

    private EventSystem eventSystem;
    private GameObject selectedBeforeOpened;

    public void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        eventSystem = EventSystem.current;
    }

    public void OpenSubMenu()
    {
        subMenuCanvas.SetActive(true);
        myPreviousSubMenu = currentOpenMenu;
        currentOpenMenu = this;
        if (selectAfterClose == null)
        {
            selectedBeforeOpened = eventSystem.currentSelectedGameObject;
        }
        eventSystem.SetSelectedGameObject(firstSelectedButton);
        if (is_pause_menu)
        {
            Time.timeScale = 0f;
        }
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
        currentOpenMenu = myPreviousSubMenu;
        subMenuCanvas.SetActive(false);
        if (selectAfterClose == null)
        {
            eventSystem.SetSelectedGameObject(selectedBeforeOpened);
        }
        else
        {
            eventSystem.SetSelectedGameObject(selectAfterClose);
        }
        if (is_pause_menu)
        {
            Time.timeScale = 1f;
        }
    }

    public void ToggleMenu()
    {
        if (currentOpenMenu == this)
        {
            CloseSubMenu();
        }
        else if (currentOpenMenu == null)
        {
            OpenSubMenu();
        }

    }

    private void Update()
    {

        if (playerInput.actions["Back"].triggered
            || (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)))
        {

            if (currentOpenMenu == this)
            {
                CloseSubMenu();
            }
        }
    }

}
