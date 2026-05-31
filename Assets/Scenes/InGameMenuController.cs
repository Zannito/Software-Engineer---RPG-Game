using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    public static InGameMenuController Instance;

    [Header("--- Sub Menu Panels (Kanan) ---")]
    [SerializeField] private GameObject panelStatManage;
    [SerializeField] private GameObject panelInventory;
    [SerializeField] private GameObject panelSkill;
    [SerializeField] private GameObject panelShop;
    [SerializeField] private GameObject panelSetting;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Awal game, sembunyikan semua sub-menu kanan biar bersih
        CloseAllSubMenus();
    }

    // --- FUNGSI UNTUK BUTTON ---
    public void OnStatButtonClicked() { ToggleSubMenu(panelStatManage); }
    public void OnInventoryButtonClicked() { ToggleSubMenu(panelInventory); }
    public void OnSkillButtonClicked() { ToggleSubMenu(panelSkill); }
    public void OnShopButtonClicked() { ToggleSubMenu(panelShop); }
    public void OnSettingButtonClicked() { ToggleSubMenu(panelSetting); }

    private void ToggleSubMenu(GameObject targetPanel)
    {
        if (targetPanel == null) return;

        if (targetPanel.activeSelf)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            CloseAllSubMenus();
            targetPanel.SetActive(true);
        }
    }

    public void CloseAllSubMenus()
    {
        if (panelStatManage != null) panelStatManage.SetActive(false);
        if (panelInventory != null) panelInventory.SetActive(false);
        if (panelSkill != null) panelSkill.SetActive(false);
        if (panelShop != null) panelShop.SetActive(false);
        if (panelSetting != null) panelSetting.SetActive(false);
    }
}