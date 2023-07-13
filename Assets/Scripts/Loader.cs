using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    MainMenuScene,
    LoadingScene,
    GameScene
}

public static class Loader
{
    private static Scene targetScene;

    public static void LoadScene(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene((int)Scene.LoadingScene);
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene((int)targetScene);
    }
}
