using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectableSounds : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private Selectable selectable;
    [SerializeField] private AudioSource UIhoverSound;
    [SerializeField] private AudioSource UIconfirmSound;
    private void Awake()
    {
        if (UIhoverSound == null)
        {
            Debug.LogWarning("Audio source on UI selectable is null!");
        }
        selectable = GetComponent<Selectable>();
        if (selectable != null && selectable.GetType()==typeof(Button))
        {
            if (UIconfirmSound == null)
            {
                Debug.LogWarning("Audio source on UI selectable is null!");
            }
            Button button = (Button)selectable;
            button.onClick.AddListener(delegate { ClickSound(); });
        }
        

    }
    public void OnSelect(BaseEventData eventData)
    {
        if (selectable != null)
        {
            if (selectable.interactable)
            {

                UIhoverSound.Play();
                
            }
        }
        else
        {
            UIhoverSound.Play();
        }
    }

    public void ClickSound()
    {
        if (selectable != null)
        {
            if (selectable.interactable)
            {
                UIconfirmSound.Play();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectable != null)
        {
            if (selectable.interactable)
            {
                UIhoverSound.Play();
            }
        }
        else
        {
            UIhoverSound.Play();
        }
    }
}
