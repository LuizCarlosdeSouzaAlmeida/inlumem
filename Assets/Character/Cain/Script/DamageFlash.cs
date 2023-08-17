using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 1f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;


    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private Coroutine _damageFlashCoroutine;
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }
    public void CallDamageFlash(){
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private IEnumerator DamageFlasher(){
        SerFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < _flashTime){
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    private void SerFlashColor(){
        _material.SetColor("_FlashColor", _flashColor);
    }
    private void SetFlashAmount(float amount){
        _material.SetFloat("_FlashAmount", amount);
    }
}
