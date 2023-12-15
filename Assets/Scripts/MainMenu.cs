using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject mainMenu;

    [SerializeField] 
    private GameObject splashScreen;

    [SerializeField] 
    private Text progressBarText;

    public void StartGame()
    {
        StartCoroutine(LoadAsync());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation AsyncLoad = SceneManager.LoadSceneAsync(1);//Асинхронная загрузка сцены

        AsyncLoad.allowSceneActivation = false;//Отключаем загрузку сцены, пока...

        while (!AsyncLoad.isDone)//Пока сцена не загружена
        {
            //LoadingBar.value = AsyncLoad.progress;//Передаем в слайдер - прогресс, того на сколько процентов загружена сцена
            mainMenu.SetActive(false);
            splashScreen.SetActive(true);
            progressBarText.text = Mathf.RoundToInt(AsyncLoad.progress * 100) + "%";//Передаем в текст - текст загрузки)

            if (AsyncLoad.progress >= .9f && !AsyncLoad.allowSceneActivation)//Если прогресс загрузки равен/больше 0,9 и сцена не активирована, то ...
            {
                if (Input.anyKeyDown)//Если нажали любую кнопку, то...
                {
                    AsyncLoad.allowSceneActivation = true;//Открываем сцену
                }
            }

            yield return null;//Возвращаем ничего
        }
    }
}