using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("--- Daftarkan Semua Panel Menu Lu Di Sini ---")]
    [Tooltip("Masukkan Panel_Status, Panel_Equipment, Panel_Skill, dll")]
    [SerializeField] private GameObject[] allPanels;

    [Header("--- Universal Blocker ---")]
    [Tooltip("Tarik objek BG_Blocker raksasa lu ke sini")]
    [SerializeField] private GameObject bgBlocker; // Blocker tunggal untuk semua panel

    /// <summary>
    /// SAKTI: Buka satu panel, tutup yang lain, dan nyalakan Blocker
    /// </summary>
    public void OpenExclusivePanel(GameObject panelToOpen)
    {
        bool anyPanelOpened = false;

        // 1. Sapu bersih: Tutup semua panel dulu
        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        // 2. Buka khusus panel yang lu klik
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            anyPanelOpened = true; // Tandai kalau ada panel yang lagi kebuka
        }

        // 3. Nyalakan blocker raksasa HANYA jika ada panel yang terbuka
        if (bgBlocker != null)
        {
            bgBlocker.SetActive(anyPanelOpened);
        }
    }

    /// <summary>
    /// SAKTI: Fungsi buat nutup semua panel dan matiin blocker (Dicolok ke BG_Blocker)
    /// </summary>
    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        // Matiin juga blockernya karena layar udah bersih
        if (bgBlocker != null) bgBlocker.SetActive(false);
    }
}