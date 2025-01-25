using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelBuilderButtonController
{
    Label m_NameLabel;
    Button m_Button;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("placable-object-name");
        m_Button = visualElement.Q<Button>("placable-object-button");

        m_Button.RegisterCallback<ClickEvent>(OnButtonClick);
    }

    public void SetObjectData(PlacableObjectData placableObjectData)
    {
       // m_NameLabel.text = placableObjectData.DisplayName;
        m_Button.text = placableObjectData.DisplayName;
    }

    private void OnButtonClick(ClickEvent evt)
    {
        Debug.Log("LevelBuilderButtonController: OnButtonClick" + evt);

    }
}
