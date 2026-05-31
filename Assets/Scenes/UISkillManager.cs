using UnityEngine;
using UnityEngine.UI;

public class UISkillManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Slot UI References ---")]
    [Tooltip("Masukkan Button Slot 1 sampai 4 berurutan")]
    [SerializeField] private Button[] slotButtons = new Button[4];
    [SerializeField] private Image[] slotImages = new Image[4];

    [Header("--- Sprite Resources ---")]
    [Tooltip("Gambar default saat slot kosong (belum dipasang skill)")]
    [SerializeField] private Sprite emptySlotSprite;

    [Tooltip("Gambar masing-masing skill")]
    [SerializeField] private Sprite ironWillSprite;
    [SerializeField] private Sprite dancingWavesSprite;
    [SerializeField] private Sprite ironBodySprite;
    [SerializeField] private Sprite ironEyeSprite;

    private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Gelap
    private Color unlockedColor = Color.white; // Terang murni

    // Level requirement untuk masing-masing slot (1, 5, 10, 15)
    private int[] slotUnlockLevels = new int[] { 1, 5, 10, 15 };

    void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerStatus = playerObj.GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (playerStatus == null) return;
        RefreshSlotUI();
    }

    /// <summary>
    /// Update tampilan 4 slot (terkunci, kosong, atau ada isinya)
    /// </summary>
    private void RefreshSlotUI()
    {
        int pLevel = playerStatus.currentLevel;

        for (int i = 0; i < 4; i++)
        {
            bool isUnlocked = pLevel >= slotUnlockLevels[i];

            // Atur status bisa diklik (interactable)
            slotButtons[i].interactable = isUnlocked;

            if (!isUnlocked)
            {
                // Kalau kekunci: warnanya gelap dan gambarnya dipaksa jadi empty
                slotImages[i].color = lockedColor;
                slotImages[i].sprite = emptySlotSprite;
            }
            else
            {
                // Kalau terbuka: warnanya terang, lalu cek isinya
                slotImages[i].color = unlockedColor;

                PlayerStatus.SkillType currentSkill = playerStatus.equippedSkills[i];
                if (currentSkill == PlayerStatus.SkillType.None)
                {
                    slotImages[i].sprite = emptySlotSprite; // Kosong
                }
                else
                {
                    slotImages[i].sprite = GetSpriteForSkill(currentSkill); // Ada isinya
                }
            }
        }
    }

    private Sprite GetSpriteForSkill(PlayerStatus.SkillType skill)
    {
        switch (skill)
        {
            case PlayerStatus.SkillType.IronWill: return ironWillSprite;
            case PlayerStatus.SkillType.DancingWaves: return dancingWavesSprite;
            case PlayerStatus.SkillType.IronBody: return ironBodySprite;
            case PlayerStatus.SkillType.IronEye: return ironEyeSprite;
            default: return emptySlotSprite;
        }
    }

    // ========================================================
    // FUNGSI UNTUK BUTTON "ADD" DI MASING-MASING PARENT SKILL
    // ========================================================
    public void EquipSkill(int skillTypeIndex)
    {
        PlayerStatus.SkillType skillToEquip = (PlayerStatus.SkillType)skillTypeIndex;

        // 1. Cek apakah skill ini udah dipasang di slot lain (Anti-Duplikat)
        if (playerStatus.HasSkill(skillToEquip))
        {
            Debug.LogWarning("Lu udah pasang skill ini cok! Gak bisa dipasang dobel!");
            return;
        }

        // 2. Cari slot PERTAMA yang TERBUKA dan KOSONG
        for (int i = 0; i < 4; i++)
        {
            bool isUnlocked = playerStatus.currentLevel >= slotUnlockLevels[i];

            if (isUnlocked && playerStatus.equippedSkills[i] == PlayerStatus.SkillType.None)
            {
                // Pasang skill ke slot ini
                playerStatus.equippedSkills[i] = skillToEquip;
                Debug.Log($"Skill {skillToEquip} berhasil dipasang ke Slot {i + 1}");
                return; // Sukses, langsung keluar dari fungsi
            }
        }

        Debug.LogWarning("Semua slot lu udah penuh atau belum kebuka cok!");
    }

    // ========================================================
    // FUNGSI UNTUK MENGHAPUS SKILL (UNEQUIP) SAAT SLOT DIKLIK
    // ========================================================
    public void UnequipSlot(int slotIndex)
    {
        // Pastikan slotnya emang kebuka dan ada isinya
        if (playerStatus.currentLevel >= slotUnlockLevels[slotIndex])
        {
            if (playerStatus.equippedSkills[slotIndex] != PlayerStatus.SkillType.None)
            {
                playerStatus.equippedSkills[slotIndex] = PlayerStatus.SkillType.None;
                Debug.Log($"Slot {slotIndex + 1} berhasil dicopot!");
            }
        }
    }

    // Helper functions biar gampang dicolok di Unity Editor Button OnClick()
    public void ClickAddIronWill() { EquipSkill((int)PlayerStatus.SkillType.IronWill); }
    public void ClickAddDancingWaves() { EquipSkill((int)PlayerStatus.SkillType.DancingWaves); }
    public void ClickAddIronBody() { EquipSkill((int)PlayerStatus.SkillType.IronBody); }
    public void ClickAddIronEye() { EquipSkill((int)PlayerStatus.SkillType.IronEye); }

    public void ClickSlot1() { UnequipSlot(0); }
    public void ClickSlot2() { UnequipSlot(1); }
    public void ClickSlot3() { UnequipSlot(2); }
    public void ClickSlot4() { UnequipSlot(3); }
}