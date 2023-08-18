using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour, IPointerEnterHandler
{
	private AudioSource AudioSource;
	[SerializeField] private AudioClip MenuHoverSound;
	[SerializeField] private AudioClip MenuClickSound;
	[SerializeField] private Button newGameButton;
	[SerializeField] private Button quitButton;
	private List<Button> Buttons;

	private void Awake()
	{
		Buttons = new List<Button>() { newGameButton, quitButton };
		AudioSource = GetComponent<AudioSource>();

		newGameButton.onClick.AddListener(() =>
		{
			AudioSource.PlayOneShot(MenuClickSound);
			StartCoroutine(WaitForAudioToLoad());
		});

		quitButton.onClick.AddListener(() =>
		{
			Application.Quit();
		});
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		var targetGraphics = Buttons.Select(b => b.targetGraphic.gameObject).ToList();
		if (targetGraphics.Contains(eventData.pointerEnter))
		{
			AudioSource.PlayOneShot(MenuHoverSound);
		}
	}

	private IEnumerator WaitForAudioToLoad()
	{
		yield return new WaitWhile(() => AudioSource.isPlaying);
		Loader.Load(Loader.Scene.InitialCutscene);
	}
}
