using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite buttonNormal;
    public Sprite buttonHovered; 

    AudioManager audioManager;
    
    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = buttonNormal;
        audioManager = GameObject.FindWithTag("AudioManager")?.GetComponent<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayButtonSound();
        }
        buttonImage.sprite = buttonHovered;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button Clicked");
        if (audioManager != null)
        {
            audioManager.PlayClickSound();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = buttonNormal;
    }
}