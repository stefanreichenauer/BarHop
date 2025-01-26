using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameState
{
    SIMULATION,
    PLACING_OBJECTS,
    CHOOSING_OBJECTS,
    DELETE_OBJECTS
}

public class GameStateController : MonoBehaviour
{
    [Header("Physic Elements")]
    [SerializeField] Rigidbody bubble;
    [SerializeField] Vector3 bubbleStartVelocity;
    private Vector3 bubbleOriginPos;
    private MeshRenderer bubbleRenderer;

    [Header("UI Elements")]
    [SerializeField] Button startButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button backToLevelSelectButton;
    [SerializeField] Button deleteObjectButton;
    [SerializeField] GameObject placeableObjectChooserPanel;
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI infoPanelText;

    [Header("Level References")]
    [SerializeField] UnityEditor.SceneAsset levelSelectReference;

    GameState currentGameState = GameState.CHOOSING_OBJECTS;

    GameObject activeButton;
    GameObject objectToPlace;
    bool canPlaceObject = false;
    Vector3 oldPosition;
    bool isMovingPlacedObject = false;
    RotationAxis currentRotationAxis = RotationAxis.Z_PLUS;

    [Header("Placement Settings")]
    [SerializeField] private Vector2 collisionCheckBoxSize = Vector2.one;
    [SerializeField] float rotationSpeed = 100f;

    private void Start()
    {
        bubbleRenderer = bubble.GetComponent<MeshRenderer>();
        bubbleOriginPos = bubble.transform.position;

        StopSimulation();

        startButton.onClick.AddListener(() => StartSimulation());
        stopButton.onClick.AddListener(() => StopSimulation());
        restartButton.onClick.AddListener(() => RestartLevel());
        backToLevelSelectButton.onClick.AddListener(() => LoadLevelSelect());
        deleteObjectButton.onClick.AddListener(() => ActivateDeleteMode());
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.SIMULATION:
                break;
            case GameState.PLACING_OBJECTS:
                HandlePlacingModeUpdate();
                break;
            case GameState.CHOOSING_OBJECTS:
                HandleChoosingModeUpdate();
                break;
            case GameState.DELETE_OBJECTS:
                HandleDeletingModeUpdate();
                break;
        }
    }

    private void HandlePlacingModeUpdate()
    {
        
        if (Input.GetButtonDown("Cancel"))
        {
            if (isMovingPlacedObject)
            {
                objectToPlace.transform.position = oldPosition;
                isMovingPlacedObject = false;
            }
            else
            {
                Destroy(objectToPlace);
            }

            objectToPlace = null;
            SetActiveGameState(GameState.CHOOSING_OBJECTS);
            return;
        }

        canPlaceObject = true;
        Collider[] colliders = Physics.OverlapBox(objectToPlace.transform.position, new Vector3(collisionCheckBoxSize.x, collisionCheckBoxSize.y, 1f), Quaternion.identity);

        foreach (var collider in colliders)
        {
            if (GetRootParent(collider.gameObject) == objectToPlace)
            {
                continue;
            }
            canPlaceObject = false;
            break;
        }

        if (canPlaceObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                objectToPlace = null;

                activeButton.SetActive(false);
                SetActiveGameState(GameState.CHOOSING_OBJECTS);
            }
        }

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheelInput != 0f)
        {
            float rotationAmount = mouseWheelInput * rotationSpeed;
            Vector3 rotation = Vector3.zero;

            switch(currentRotationAxis)
            {
                case RotationAxis.X_PLUS:
                    rotation.x = rotationAmount;
                    break;
                case RotationAxis.Y_PLUS:
                    rotation.y = rotationAmount;
                    break; 
                case RotationAxis.Z_PLUS:
                    rotation.z = rotationAmount;
                    break;
                case RotationAxis.X_MINUS:
                    rotation.x = -rotationAmount;
                    break;
                case RotationAxis.Y_MINUS:
                    rotation.y = -rotationAmount;
                    break;
                case RotationAxis.Z_MINUS:
                    rotation.z = -rotationAmount;
                    break;
            }

            objectToPlace.transform.Rotate(rotation);
        }

        if (objectToPlace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePosition.z = 0;
            objectToPlace.transform.position = mousePosition;
        }
    }

    private void HandleDeletingModeUpdate()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetActiveGameState(GameState.CHOOSING_OBJECTS);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitParent = GetRootParent(hit.collider.gameObject);
                PlaceableObjectMarkComponent markComponent = hitParent.GetComponent<PlaceableObjectMarkComponent>();
                if (markComponent != null)
                {
                    markComponent.buttonReference.SetActive(true);
                    Destroy(hitParent);
                    SetActiveGameState(GameState.CHOOSING_OBJECTS);
                }
            }
        }
    }

    private void HandleChoosingModeUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitParent = GetRootParent(hit.collider.gameObject);
                PlaceableObjectMarkComponent markComponent = hitParent.GetComponent<PlaceableObjectMarkComponent>();
                if (markComponent != null)
                {
                    oldPosition = hitParent.transform.position;
                    isMovingPlacedObject = true;
                    objectToPlace = hitParent;
                    SetActiveGameState(GameState.PLACING_OBJECTS);
                    currentRotationAxis = markComponent.data.RotationAxis;
                }
            }
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

    public void StartSimulation()
    {
        SetActiveGameState(GameState.SIMULATION);

        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.isKinematic = false;
        bubble.linearVelocity = bubbleStartVelocity;
    }

    public void StopSimulation()
    {
        SetActiveGameState(GameState.CHOOSING_OBJECTS);

        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.transform.position = bubbleOriginPos;
        bubble.isKinematic = true;
    }

    private void LoadLevelSelect()
    {
        SceneManager.LoadScene(levelSelectReference.name);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ActivateDeleteMode()
    {
        SetActiveGameState(GameState.DELETE_OBJECTS);
    }

    public void SetActiveGameState(GameState gameState)
    {
        currentGameState = gameState;

        switch (gameState)
        {
            case GameState.SIMULATION:
                startButton.gameObject.SetActive(false);
                stopButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                backToLevelSelectButton.gameObject.SetActive(false);
                placeableObjectChooserPanel.SetActive(false);
                break;
            case GameState.PLACING_OBJECTS:
                startButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                backToLevelSelectButton.gameObject.SetActive(false);
                placeableObjectChooserPanel.SetActive(false);
                infoPanel.gameObject.SetActive(true);
                infoPanelText.text = "Placing Object - Press <Esc> to cancel";
                break;
            case GameState.CHOOSING_OBJECTS:
                startButton.gameObject.SetActive(true);
                stopButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                backToLevelSelectButton.gameObject.SetActive(true);
                placeableObjectChooserPanel.SetActive(true);
                infoPanel.gameObject.SetActive(false);
                break;
            case GameState.DELETE_OBJECTS:
                startButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                backToLevelSelectButton.gameObject.SetActive(false);
                infoPanel.gameObject.SetActive(true);
                infoPanelText.text = "Deleting Object - Press <Esc> to cancel";
                placeableObjectChooserPanel.SetActive(false);
                break;
        }
    }

    public void StartPlacingObject(GameObject obj, GameObject buttonRef, PlaceableObjectData data)
    {
        SetActiveGameState(GameState.PLACING_OBJECTS);
        objectToPlace = obj;
        activeButton = buttonRef;
        currentRotationAxis = data.RotationAxis;

        PlaceableObjectMarkComponent markComponent = objectToPlace.AddComponent<PlaceableObjectMarkComponent>();
        markComponent.buttonReference = activeButton;
        markComponent.data = data;
    }

}
