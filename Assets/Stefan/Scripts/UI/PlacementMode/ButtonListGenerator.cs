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
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.DisplayName;
    }

    void OnClick(GameObject prefabToSpawn, GameObject buttonRef)
    {
        ButtonListController buttonListController = gameObject.GetComponentInParent<ButtonListController>();
        buttonListController.OnButtonClicked(prefabToSpawn, buttonRef);
    }
}
