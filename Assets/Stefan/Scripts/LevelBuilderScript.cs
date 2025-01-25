using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelBuilderScript : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset m_ListEntryTemplate;

    [SerializeField]
    PlacableObjectData m_PlacableObject;

    [SerializeField]
    private GameObject currentPlaceableObject;

    GameObject objectToPlace;

    void OnEnable()
    {
        Debug.Log("LevelBuilderScript: start");

        var uiDocument = GetComponent<UIDocument>();

        var levelBuilderUI = new LevelBuilderListController();
        levelBuilderUI.InitAllObjects(uiDocument.rootVisualElement, m_ListEntryTemplate, m_PlacableObject);

        objectToPlace = Instantiate(currentPlaceableObject);
    }

    private void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10f));
        mousePosition.z = 0; 
        objectToPlace.transform.position = mousePosition;



        Debug.Log("LevelBuilderScript: hit: 1: " + mousePosition);
        Debug.Log("LevelBuilderScript: hit: 2: " + objectToPlace.transform.position);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            Debug.Log("LevelBuilderScript: hit: " + hitInfo);
        }

    }

}
