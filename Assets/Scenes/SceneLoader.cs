using UnityEngine;

public class SceneLoader
{
	void Load()
	{
		Loader.LoadWithNoLoadingScreen(Loader.Scene.SampleScene);
	}
}
