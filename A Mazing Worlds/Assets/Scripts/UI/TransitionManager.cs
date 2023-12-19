using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private static readonly int _start = Animator.StringToHash("Start");

    public void LoadNextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene));
    }

    private IEnumerator LoadSceneCoroutine(int scene)
    {
        transition.SetTrigger(_start);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);
    }

    private IEnumerator LoadSceneCoroutine(string scene)
    {
        transition.SetTrigger(_start);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);
    }
}
