using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            if (Input.GetButtonDown("Cancel"))
            {
                Destroy(objectToPlace);
                objectToPlace = null;
                SetActiveGameState(GameState.CHOOSING_OBJECTS);
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceableObjectMarkComponent markComponent = objectToPlace.AddComponent<PlaceableObjectMarkComponent>();
                markComponent.buttonReference = activeButton;

                objectToPlace = null;

                activeButton.SetActive(false);
                SetActiveGameState(GameState.CHOOSING_OBJECTS);
            }
        }

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheelInput != 0f)
        {
            objectToPlace.transform.Rotate(new Vector3(0, 0, mouseWheelInput * rotationSpeed));
        }

        if (objectToPlace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitParent = GetRootParent(hit.collider.gameObject);
                PlaceableObjectMarkComponent markComponent = hitParent.GetComponent<PlaceableObjectMarkComponent>();
                if (markComponent != null)
                {
                    objectToPlace = hitParent;
                    SetActiveGameState(GameState.PLACING_OBJECTS);
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

    public void StartPlacingObject(GameObject obj, GameObject buttonRef)
    {
        SetActiveGameState(GameState.PLACING_OBJECTS);
        objectToPlace = obj;
        activeButton = buttonRef;
    }

}
