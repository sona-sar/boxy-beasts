using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Material normalBoxMaterial;
    [SerializeField] private Material goalBoxMaterial;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject exitButton;

    private List<BoxController> boxesInScene = new List<BoxController>();
    private bool isExitButtonVisible = false;

    private void Awake()
    {
        if(instance == null)
        {
            // DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isExitButtonVisible = !isExitButtonVisible;
            if (exitButton != null)
                exitButton.SetActive(isExitButtonVisible);
            if (levelCompleteUI != null){
                if(exitButton.activeSelf)
                    levelCompleteUI.SetActive(false);
                else
                    CheckWinCondition();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        boxesInScene.Clear();
        
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
        if (inGameUI != null)
            inGameUI.SetActive(true);
    }

    public void RegisterBox(BoxController box)
    {
        if (!boxesInScene.Contains(box))
        {
            boxesInScene.Add(box);
        }
    }

    public void UnregisterBox(BoxController box)
    {
        boxesInScene.Remove(box);
    }

    public void UpdateBoxMaterial(BoxController boxController, bool isOnGoal)
    {
        Renderer boxRenderer = boxController.GetComponentInChildren<Renderer>();
        if (boxRenderer != null)
        {
            boxRenderer.material = isOnGoal ? goalBoxMaterial : normalBoxMaterial;
        }
    }

    public void CheckWinCondition()
    {
        int count = 0;
        foreach (var box in boxesInScene)
        {
            if (!box.OnGoal) count++;
        }

        if (count == 0)
        {
            levelCompleteUI.SetActive(true);
            inGameUI.SetActive(false);
            PlayerPrefs.SetInt("UnlockedLevel", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.Save();
        }
    }

}
