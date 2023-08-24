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
			// RectTransform border = (RectTransform)transform.Find("Filled").Find("FilledBorder");
			borderRect.offsetMax = new Vector2(-10, borderRect.offsetMax.y);
			UpdatedBar = true;
		}

		slider.value = bossHealth.currentHealth;
	}
}