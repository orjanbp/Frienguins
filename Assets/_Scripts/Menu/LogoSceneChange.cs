using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LogoSceneChange : MonoBehaviour
{

    public Animator animator;
    public Animator logoFadein;

    public int levelToLoad;

    private void Start()
    {
        StartCoroutine(GoToNextLevel());
    }

    IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("InsideMenu");
    }
}
