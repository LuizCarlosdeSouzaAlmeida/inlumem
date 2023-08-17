using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Fator para reduzir a escala de tempo (1.0 é a velocidade normal, 0.5 é metade da velocidade, 0.1 é uma velocidade muito lenta, etc.)
    public float timeScale = 0.5f;

    private void Update()
    {
        // Defina o timescale para o valor especificado
        Time.timeScale = timeScale;
    }
}
