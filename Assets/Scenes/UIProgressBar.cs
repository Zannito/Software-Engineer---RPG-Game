using UnityEngine;
using UnityEngine.UI; // Wajib untuk mengontrol UI Slider

public class UIProgressBar : MonoBehaviour
{
    [Header("--- UI Elements ---")]
    [SerializeField] private Slider slider;

    /// <summary>
    /// Fungsi untuk mengupdate visual bar HP.
    /// </summary>
    /// <param name="currentValue">HP saat ini</param>
    /// <param name="maxValue">Max HP karakter/monster</param>
    public void UpdateValue(float currentValue, float maxValue)
    {
        if (slider == null) return;

        // JIKA LU PAKAI CARA B:
        // Kita paksa Max Value si Slider di Unity Editor mengikuti Max HP karakter secara real-time
        slider.maxValue = maxValue;

        // Isinya langsung dimasukkan angka HP saat ini (bukan hasil pembagian lagi)
        slider.value = currentValue;
    }
}