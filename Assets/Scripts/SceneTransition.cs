using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    public Animator animator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator LoadAScene(int sceneIndex)
    {
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
