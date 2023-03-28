using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform arrowRect;

    private AsyncOperation asyncOperation;
    // Start is called before the first frame update
    void Start()
    {
        LoadGameScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            arrowRect.anchoredPosition = new Vector2(arrowRect.anchoredPosition.x, -arrowRect.anchoredPosition.y);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (arrowRect.anchoredPosition.y)
            {
                case 50:
                    asyncOperation.allowSceneActivation = true;
                    break;
                case -50:
                    Application.Quit();
                    break;
            }
        }
    }
    private void LoadGameScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
    }   
}
