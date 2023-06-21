using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadScene : MonoBehaviour
{
    public GameObject loadScenePanel;
    public Slider slider;
    public int sceneNextId;
    public void OnLoadNextScene()
    {
        
        loadScenePanel.SetActive(true);
        slider.gameObject.SetActive(true);
        loadScenePanel.transform.SetAsLastSibling();
        Screen.SetResolution(1920, 1080, false);
        StartCoroutine("LoadLeve");
    }
    IEnumerator LoadLeve()
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + sceneNextId);
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            if (sceneNextId < 0)
            {
                UIManager.Instance.ClearData();
            }
            
            yield return null;
        }
    }
}
