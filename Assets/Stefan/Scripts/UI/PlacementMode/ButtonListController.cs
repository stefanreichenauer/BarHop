using NUnit.Framework.Internal;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    GameObject buttonPanel;
    GameObject placementActivePanel;
    GameObject objectToPlace;
    bool isInPlacementMode = false;
    GameObject activeButton;

    [SerializeField]
    float rotationSpeed = 75f;

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
            if (Input.GetButtonDown("Cancel"))
            {
                buttonPanel.SetActive(true);
                placementActivePanel.SetActive(false);
                Destroy(objectToPlace);
                objectToPlace = null;
                isInPlacementMode = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                buttonPanel.SetActive(true);
                placementActivePanel.SetActive(false);
                objectToPlace = null;
                isInPlacementMode = false;
                Destroy(activeButton);
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

    public void OnButtonClicked(GameObject prefabToSpawn, GameObject buttonRef)
    {
        activeButton = buttonRef;
        isInPlacementMode = true;
        objectToPlace = Instantiate(prefabToSpawn);

        buttonPanel.SetActive(false);
        placementActivePanel.SetActive(true);
    }
}
