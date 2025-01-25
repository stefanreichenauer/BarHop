using NUnit.Framework.Internal;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    GameObject buttonPanel;
    GameObject placementActivePanel;
    GameObject objectToPlace;
    bool isInPlacementMode = false;
    GameObject activeButton;

    void Start()
    {
        GameObject canvas = gameObject;
        buttonPanel = canvas.transform.Find("ButtonPanel").gameObject;
        placementActivePanel = canvas.transform.Find("PlacementActivePanel").gameObject;

    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel") && isInPlacementMode)
        {
            buttonPanel.SetActive(true);
            placementActivePanel.SetActive(false);
            Destroy(objectToPlace);
            objectToPlace = null;
            isInPlacementMode = false;
        }

        if (Input.GetMouseButtonDown(0) && isInPlacementMode)
        {
            buttonPanel.SetActive(true);
            placementActivePanel.SetActive(false);
            objectToPlace = null;
            isInPlacementMode = false;
            Destroy(activeButton);
        }

        if (objectToPlace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
            mousePosition.z = 0;
            objectToPlace.transform.position = mousePosition;
        }

    }

    public void OnButtonClicked(GameObject prefabToSpawn, GameObject buttonRef)
    {
        activeButton = buttonRef;
        isInPlacementMode = true;
        objectToPlace = Instantiate(prefabToSpawn);

        buttonPanel.SetActive(false);
        placementActivePanel.SetActive(true);
    }
}
