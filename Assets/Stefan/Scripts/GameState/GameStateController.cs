using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    SIMULATION,
    PLACING_OBJECTS,
    CHOOSING_OBJECTS
}

public class GameStateController : MonoBehaviour
{
    [SerializeField] Rigidbody bubble;
    [SerializeField] Vector3 bubbleStartVelocity;
    private Vector3 bubbleOriginPos;
    private MeshRenderer bubbleRenderer;

    [SerializeField] Button startButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button backToLevelSelectButton;
    [SerializeField] GameObject placeableObjectChooserPanel;

    private void Start()
    {
        StopSimulation();

        bubbleRenderer = bubble.GetComponent<MeshRenderer>();
        bubbleOriginPos = bubble.transform.position;

        startButton.onClick.AddListener(() => StartSimulation());
        stopButton.onClick.AddListener(() => StopSimulation());
        restartButton.onClick.AddListener(() => RestartLevel());
        backToLevelSelectButton.onClick.AddListener(() => LoadLevelSelect());
    }
    public void StartSimulation()
    {
        HandleUIGameStateChange(GameState.SIMULATION);

        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.isKinematic = false;
        bubble.linearVelocity = bubbleStartVelocity;
    }

    public void StopSimulation()
    {
        HandleUIGameStateChange(GameState.CHOOSING_OBJECTS);

        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.linearVelocity = new Vector3(0, 0, 0);
        bubble.transform.position = bubbleOriginPos;
        bubble.isKinematic = true;
    }

    private void LoadLevelSelect()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleUIGameStateChange(GameState gameState)
    {
        switch(gameState)
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
                break;
            case GameState.CHOOSING_OBJECTS:
                startButton.gameObject.SetActive(true);
                stopButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
                backToLevelSelectButton.gameObject.SetActive(true);
                placeableObjectChooserPanel.SetActive(true);
                break;
        }
    }
}
