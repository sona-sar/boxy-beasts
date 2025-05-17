using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ToggleType { Music, Sound }
    
    [SerializeField] private ToggleType toggleType;
    [SerializeField] private Sprite toggleOn;
    [SerializeField] private Sprite toggleOnHover;
    [SerializeField] private Sprite toggleOff;
    [SerializeField] private Sprite toggleOffHover;

    private Image buttonImage;
    private bool isHovered = false;
    private AudioManager audioManager;
    private bool isTransitioning = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        audioManager = GameObject.FindWithTag("AudioManager")?.GetComponent<AudioManager>();
        UpdateSprite();
        
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    void OnSceneUnloaded(Scene scene)
    {
        isTransitioning = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isTransitioning) return;
        
        if (audioManager != null)
        {
            audioManager.PlayButtonSound();
        }
        isHovered = true;
        UpdateSprite();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTransitioning) return;
        
        isHovered = false;
        UpdateSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTransitioning) return;
        
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isTransitioning || buttonImage == null || audioManager == null) return;
        
        bool isOn = (toggleType == ToggleType.Music) ? 
                   audioManager.IsMusicOn() : 
                   audioManager.IsSoundOn();
        
        if (isOn)
        {
            buttonImage.sprite = isHovered ? toggleOnHover : toggleOn;
        }
        else
        {
            buttonImage.sprite = isHovered ? toggleOffHover : toggleOff;
        }
    }
}