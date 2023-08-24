using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthbar : MonoBehaviour
{
	[SerializeField] private Health bossHealth;
	[SerializeField] private Slider slider;
	[SerializeField] private TMP_Text bossTextComponent;
	[SerializeField] private string bossName;
	[SerializeField] private RectTransform borderRect;

	private bool UpdatedBar = false;

	private void Start()
	{
		slider.maxValue = bossHealth.startingHealth;
		slider.value = bossHealth.currentHealth;
		bossTextComponent.text = bossName;
	}

	private void Update()
	{
		if (!UpdatedBar && bossHealth.currentHealth < bossHealth.startingHealth)
		{
			borderRect.offsetMax = new Vector2(-10, borderRect.offsetMax.y);
			UpdatedBar = true;
		}

		StartCoroutine(UpdateHealth());
	}

	private IEnumerator UpdateHealth()
	{
		float targetValue = bossHealth.currentHealth;

		float currentValue = slider.value;
		while (currentValue >= targetValue)
		{
			currentValue -= 2 * Time.deltaTime;
			slider.value = currentValue;
			yield return null;
		}

		slider.value = targetValue;
	}
}