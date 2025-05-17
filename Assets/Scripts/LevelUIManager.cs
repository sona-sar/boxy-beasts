using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Button[] levelButtons;

    private int selectedCharacter = 0;

    private void Start()
    {
        UpdateCharacterDisplay();
         if (levelButtons != null && levelButtons.Length > 0)
        {
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
            
            for(int i = 0; i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                    levelButtons[i].interactable = false;
            }
            
            for(int i = 0; i < unlockedLevel && i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                    levelButtons[i].interactable = true;
            }
        }
    }

    private void UpdateCharacterDisplay()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == selectedCharacter);
        }
    }

    public void OnReplayButton()
    {
        Debug.Log("Replay Button Pressed");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextLevelButton()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(SceneTransition.instance.LoadAScene(nextIndex));
        else
            StartCoroutine(SceneTransition.instance.LoadAScene(0));;
    }

    public void OnStartButton(){
        CharacterManager.instance.SetSelectedCharacter(characters[selectedCharacter]);
        StartCoroutine(SceneTransition.instance.LoadAScene(1)); 
    }

    public void OnToggleMusic()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMusic();
        }
    }

    public void OnToggleSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleSound();
        }
    }

    public void OnLevelsButton()
    {
        menuPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void OnInfoButton(){
        menuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void OnBackButton()
    {
        menuPanel.SetActive(true);
        levelsPanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void OnLevelButton(int level){
        CharacterManager.instance.SetSelectedCharacter(characters[selectedCharacter]);
        StartCoroutine(SceneTransition.instance.LoadAScene(level));
    }

    public void OnHomeButton()
    {
        Debug.Log("Home Button Pressed");
        StartCoroutine(SceneTransition.instance.LoadAScene(0));
    }

    public void OnLeftArrowButton()
    {
        selectedCharacter = (selectedCharacter - 1 + characters.Length) % characters.Length;
        UpdateCharacterDisplay();
    }

    public void OnRightArrowButton()
    {
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        UpdateCharacterDisplay();
    }

    public void OnExitButton()
    {
        Debug.Log("Exit Button Pressed");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}