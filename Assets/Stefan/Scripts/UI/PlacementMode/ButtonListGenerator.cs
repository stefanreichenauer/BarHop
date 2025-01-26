using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject buttonPrefab;

    [SerializeField]
    public PlaceableObjectsPerLevel objectData;

    [SerializeField]
    private GameStateController gameStateController;

    void Start()
    {
        for (int i = 0; i < objectData.objects.Length; i++)
        {
            CreateButton(objectData.objects[i]);
        }
    }

    void CreateButton(PlaceableObjectData data)
    {
        GameObject button = (GameObject)Instantiate(buttonPrefab);
        button.transform.SetParent(gameObject.transform);
        button.GetComponent<Button>().onClick.AddListener(() => OnClick(data.Prefab, button));
        button.transform.GetChild(0).GetComponent<Image>().sprite = data.Sprite;
    }

    void OnClick(GameObject prefabToSpawn, GameObject buttonRef)
    {
        GameObject obj = Instantiate(prefabToSpawn);

        gameStateController.StartPlacingObject(obj, buttonRef);
    }
}
