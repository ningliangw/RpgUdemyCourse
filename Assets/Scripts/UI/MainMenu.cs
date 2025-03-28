using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] FadeScreen fadeScreen;

    private void Start()
    {
        if (SaveManager.Instance.HasSavedData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    IEnumerator LoadSceneWithFadeEffect(float delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
