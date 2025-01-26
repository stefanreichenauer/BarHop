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
    private BubblePopper bubblePopper;

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
    [SerializeField] float rotationSpeed = 15f;

    private InputAction leftMouseClick;
    private InputAction rightMouseClick;

    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.Enable();

        rightMouseClick = new InputAction(binding: "<Mouse>/rightButton");
        rightMouseClick.performed += ctx => RightMouseClicked();
        rightMouseClick.Enable();
    }

    private void OnEnable()
    {
        leftMouseClick.Enable();
        rightMouseClick.Enable();
    }

    private void OnDisable()
    {
        leftMouseClick.Disable();
        rightMouseClick.Disable();
    }

    private void LeftMouseClicked()
    {
        if (currentGameState == GameState.PLACING_OBJECTS)
        {
            if (canPlaceObject)
            {
                objectToPlace = null;

                if (activeButton != null)
                {
                    activeButton.SetActive(false);
                }
                SetActiveGameState(GameState.CHOOSING_OBJECTS);
            }

            return;
        }

        if (currentGameState == GameState.DELETE_OBJECTS)
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

            return;
        }

        if (currentGameState == GameState.CHOOSING_OBJECTS)
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

            return;
        }
    }

    private void RightMouseClicked()
    {
        if(currentGameState == GameState.PLACING_OBJECTS)
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
            activeButton = null;
            SetActiveGameState(GameState.CHOOSING_OBJECTS);
            return;
        } 
        
        if(currentGameState == GameState.DELETE_OBJECTS)
        {
            SetActiveGameState(GameState.CHOOSING_OBJECTS);
            return;
        }
    }

    private void Start()
    {
        bubbleRenderer = bubble.GetComponent<MeshRenderer>();
        bubblePopper = bubble.GetComponent<BubblePopper>();
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
        if (currentGameState == GameState.PLACING_OBJECTS)
        {
            HandlePlacingModeUpdate();
        }
    }

    private void HandlePlacingModeUpdate()
    {
        // Check if object is colliding with anything
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

        // Rotate object
        float mouseWheelInput = Mouse.current.scroll.ReadValue().y; 

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

        // Object follows the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        objectToPlace.transform.position = mousePosition;
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
        if (bubblePopper != null)
        {
            bubblePopper.is_popped = false;
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
        if (bubblePopper != null)
        {
            bubblePopper.is_popped = false;
        }
        bubble.transform.position = bubbleOriginPos;
        bubble.isKinematic = true;
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(levelSelectReference.name);
    }

    public void RestartLevel()
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
                infoPanelText.text = "Placing Object";
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
                infoPanelText.text = "Deleting Object";
                placeableObjectChooserPanel.SetActive(false);
                break;
        }
    }

    public void StartPlacingObject(GameObject obj, GameObject buttonRef, PlaceableObjectData data)
    {
        objectToPlace = obj;
        activeButton = buttonRef;
        currentRotationAxis = data.RotationAxis;

        PlaceableObjectMarkComponent markComponent = objectToPlace.AddComponent<PlaceableObjectMarkComponent>();
        markComponent.buttonReference = activeButton;
        markComponent.data = data;

        SetActiveGameState(GameState.PLACING_OBJECTS);
    }

}
