using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    GameObject buttonPanel;
    GameObject placementActivePanel;
    GameObject objectToPlace;
    bool isInPlacementMode = false;
    GameObject activeButton;
    bool canPlaceObject = false;

    [SerializeField]
    float rotationSpeed = 100f;

    [SerializeField]
    private GameStateController gameStateController;

    [SerializeField]
    private Vector2 collisionCheckBoxSize = Vector2.one;

    void Start()
    {
        GameObject canvas = gameObject;
        buttonPanel = canvas.transform.Find("ButtonPanel").gameObject;
        placementActivePanel = canvas.transform.Find("PlacementActivePanel").gameObject;

    }

    void Update()
    {
        if (isInPlacementMode)
        {
            canPlaceObject = true;
            Collider[] colliders = Physics.OverlapBox(objectToPlace.transform.position, new Vector3(collisionCheckBoxSize.x, collisionCheckBoxSize.y, 1), Quaternion.identity);

            foreach (var collider in colliders)
            {
                if(GetRootParent(collider.gameObject) == objectToPlace)
                {
                    continue;
                }
                canPlaceObject = false;
                break;
            }

            if (canPlaceObject)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    buttonPanel.SetActive(true);
                    placementActivePanel.SetActive(false);
                    Destroy(objectToPlace);
                    objectToPlace = null;
                    isInPlacementMode = false;
                    gameStateController.SetActiveGameState(GameState.CHOOSING_OBJECTS); 
                }

                if (Input.GetMouseButtonDown(0))
                {
                    PlaceableObjectMarkComponent markComponent = objectToPlace.AddComponent<PlaceableObjectMarkComponent>();
                    markComponent.buttonReference = activeButton;

                    buttonPanel.SetActive(true);
                    placementActivePanel.SetActive(false);
                    objectToPlace = null;
                    isInPlacementMode = false;
                    //Destroy(activeButton);
                    activeButton.SetActive(false);
                    gameStateController.SetActiveGameState(GameState.CHOOSING_OBJECTS);
                }
            }

            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheelInput != 0f)
            {
                objectToPlace.transform.Rotate(new Vector3(0, 0, mouseWheelInput * rotationSpeed));
            }

        }

        if (objectToPlace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
            mousePosition.z = 0;
            objectToPlace.transform.position = mousePosition;
        }

    }
    private GameObject GetRootParent(GameObject obj)
    {
        Transform current = obj.transform;

        while (current.parent != null)
        {
            current = current.parent;
        }

        return current.gameObject;
    }

    public void OnButtonClicked(GameObject prefabToSpawn, GameObject buttonRef)
    {
        activeButton = buttonRef;
        isInPlacementMode = true;
        objectToPlace = Instantiate(prefabToSpawn);

        buttonPanel.SetActive(false);
        placementActivePanel.SetActive(true);
        gameStateController.SetActiveGameState(GameState.PLACING_OBJECTS);
    }
}
