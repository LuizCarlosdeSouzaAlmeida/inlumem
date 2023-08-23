using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
	// Must match the exactly Scene file name
	public enum Scene
	{
		MainMenuScene,
		InitialCutscene,
		SampleScene,
		LoadingScene,
		SceneBoss
	}

	public static Scene targetScene;

	public static void Load(Scene targetScene)
	{
		Loader.targetScene = targetScene;

		SceneManager.LoadScene(Scene.LoadingScene.ToString());
	}

	public static void LoadWithNoLoadingScreen(Scene targetScene)
	{
		Loader.targetScene = targetScene;
		LoaderCallback();
	}

	// Load the target scene at least 1 frame after the loading scene
	public static void LoaderCallback()
	{
		SceneManager.LoadScene(targetScene.ToString());
	}
}
