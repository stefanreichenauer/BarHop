using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class LevelBuilderListController
{
    ListView m_PlacableObjectList;

    List<PlacableObjectData> m_allObjects;
    VisualTreeAsset m_ListEntryTemplate;

    public void InitAllObjects(VisualElement root, VisualTreeAsset listElementTemplate, PlacableObjectData m_PlacableObject)
    {

        Debug.Log("LevelBuilderListController: start: " + m_PlacableObject);
        m_PlacableObjectList = root.Q<ListView>("placable-object-list");

        m_ListEntryTemplate = listElementTemplate;

        m_PlacableObjectList.makeItem = () =>
        {
            var newListEntry = m_ListEntryTemplate.Instantiate();
            var newListEntryLogic = new LevelBuilderButtonController();

            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };

        m_PlacableObjectList.bindItem = (item, index) =>
        {
            (item.userData as LevelBuilderButtonController)?.SetObjectData(m_PlacableObject);
        };

        m_PlacableObjectList.fixedItemHeight = 200;

        m_PlacableObjectList.itemsSource = new List<PlacableObjectData> { m_PlacableObject };

    }
}
