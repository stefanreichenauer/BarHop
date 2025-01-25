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
    float rotationSpeed = 100f;

    [SerializeField]
    private GameStateController gameStateController;

    private GameObject previewObject; 
    private bool canPlace = true;

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
                gameStateController.HandleUIGameStateChange(GameState.CHOOSING_OBJECTS);
            }

            if (Input.GetMouseButtonDown(0))
            {
                buttonPanel.SetActive(true);
                placementActivePanel.SetActive(false);
                objectToPlace = null;
                isInPlacementMode = false;
                Destroy(activeButton);
                gameStateController.HandleUIGameStateChange(GameState.CHOOSING_OBJECTS);
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
        CreatePreviewObject(prefabToSpawn, new Vector2(0,0));
        buttonPanel.SetActive(false);
        placementActivePanel.SetActive(true);
        gameStateController.HandleUIGameStateChange(GameState.PLACING_OBJECTS);
    }


    private void CreatePreviewObject(GameObject fbxPrefab, Vector2 position)
    {
        previewObject = Instantiate(fbxPrefab, position, Quaternion.identity);
        previewObject.GetComponent<Collider>().isTrigger = true;
        SetPreviewMaterial(previewObject, Color.green);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (previewObject != null)
        {
            canPlace = false;
            SetPreviewMaterial(previewObject, Color.red);
        }
    }
    private void SetPreviewMaterial(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

}
