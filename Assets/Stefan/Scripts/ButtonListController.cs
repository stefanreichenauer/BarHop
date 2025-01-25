using NUnit.Framework.Internal;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{

    GameObject canvas;

    GameObject buttonPanel;
    GameObject placementActivePanel;

    [SerializeField]
    private GameObject prefabToPlace;

    GameObject objectToPlace;

    bool isInPlacementMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = gameObject;
        buttonPanel = canvas.transform.Find("ButtonPanel").gameObject;
        placementActivePanel = canvas.transform.Find("PlacementActivePanel").gameObject;

    }

    // Update is called once per frame
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
        }

        if (objectToPlace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
            mousePosition.z = 0;
            objectToPlace.transform.position = mousePosition;
        }

    }

    public void OnButtonClicked(int test)
    {
        isInPlacementMode = true;
        objectToPlace = Instantiate(prefabToPlace);

        buttonPanel.SetActive(false);
        placementActivePanel.SetActive(true);
    }
}
