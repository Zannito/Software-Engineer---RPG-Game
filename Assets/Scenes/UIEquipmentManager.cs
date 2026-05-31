using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Weapon UI Images ---")]
    [SerializeField] private Image rareWeaponImage;
    [SerializeField] private Image epicWeaponImage;
    [SerializeField] private Image mythicWeaponImage;

    [Header("--- Accessory UI Images ---")]
    [SerializeField] private Image rareAccessoryImage;
    [SerializeField] private Image epicAccessoryImage;
    [SerializeField] private Image mythicAccessoryImage;

    // TIGA KONDISI WARNA SESUAI REQUEST LU
    private Color lockedColor = new Color(0.2f, 0.2f, 0.2f, 1f);     // 1. Gak punya = Abu-abu gelap banget
    private Color unlockedColor = Color.white;                        // 2. Punya tapi nganggur = Berwarna murni terang
    private Color equippedColor = new Color(0.5f, 0.5f, 0.5f, 1f);    // 3. Lagi di-Equip = Diitemin/diredupin dikit

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

        // 1. UPDATE KONDISI VISUAL WEAPON
        UpdateWeaponVisuals();

        // 2. UPDATE KONDISI VISUAL ACCESSORY
        UpdateAccessoryVisuals();
    }

    private void UpdateWeaponVisuals()
    {
        // Rare Weapon
        SetItemColor(rareWeaponImage, playerStatus.ownsRareWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Rare);

        // Epic Weapon
        SetItemColor(epicWeaponImage, playerStatus.ownsEpicWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Epic);

        // Mythic Weapon
        SetItemColor(mythicWeaponImage, playerStatus.ownsMythicWeapon, playerStatus.activeWeaponTier == PlayerStatus.EquipmentTier.Mythic);
    }

    private void UpdateAccessoryVisuals()
    {
        // Rare Accessory
        SetItemColor(rareAccessoryImage, playerStatus.ownsRareAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Rare);

        // Epic Accessory
        SetItemColor(epicAccessoryImage, playerStatus.ownsEpicAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Epic);

        // Mythic Accessory
        SetItemColor(mythicAccessoryImage, playerStatus.ownsMythicAccessory, playerStatus.activeAccessoryTier == PlayerStatus.EquipmentTier.Mythic);
    }

    /// <summary>
    /// Logika sakti penentu warna berdasarkan status kepemilikan dan status pemakaian
    /// </summary>
    private void SetItemColor(Image itemImage, bool isOwned, bool isEquipped)
    {
        if (itemImage == null) return;

        if (!isOwned)
        {
            itemImage.color = lockedColor; // Belum dapet gacha
        }
        else if (isEquipped)
        {
            itemImage.color = equippedColor; // SAKTI: Lagi dipakai, otomatis diredupin/diitemin dikit!
        }
        else
        {
            itemImage.color = unlockedColor; // Punya dan siap pakai
        }
    }

    // =======================================================
    // TOMBOL EQUIP WEAPON
    // =======================================================
    public void EquipRareWeapon()
    {
        if (playerStatus != null && playerStatus.ownsRareWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Rare;
            Debug.Log($"[EQUIP] Pakai RARE Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }

    public void EquipEpicWeapon()
    {
        if (playerStatus != null && playerStatus.ownsEpicWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Epic;
            Debug.Log($"[EQUIP] Pakai EPIC Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }

    public void EquipMythicWeapon()
    {
        if (playerStatus != null && playerStatus.ownsMythicWeapon)
        {
            playerStatus.activeWeaponTier = PlayerStatus.EquipmentTier.Mythic;
            Debug.Log($"[EQUIP] Pakai MYTHIC Weapon. ATK Real-time: {playerStatus.FinalAttack}");
        }
    }

    // =======================================================
    // TOMBOL EQUIP ACCESSORY
    // =======================================================
    public void EquipRareAccessory()
    {
        if (playerStatus != null && playerStatus.ownsRareAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Rare;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai RARE Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
        }
    }

    public void EquipEpicAccessory()
    {
        if (playerStatus != null && playerStatus.ownsEpicAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Epic;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai EPIC Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
        }
    }

    public void EquipMythicAccessory()
    {
        if (playerStatus != null && playerStatus.ownsMythicAccessory)
        {
            playerStatus.activeAccessoryTier = PlayerStatus.EquipmentTier.Mythic;
            playerStatus.ResetHpToMax();
            Debug.Log($"[EQUIP] Pakai MYTHIC Accessory. HP Real-time: {playerStatus.FinalMaxHp}");
        }
    }
}