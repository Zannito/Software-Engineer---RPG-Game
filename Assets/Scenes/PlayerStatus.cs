using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [System.Serializable]
    public enum EquipmentTier { None, Rare, Epic, Mythic }

    [System.Serializable]
    public enum SkillType { None, IronWill, DancingWaves, IronBody, IronEye }

    [Header("--- Leveling & Points System ---")]
    public int currentLevel = 1;
    public int currentXp = 0;
    public int maxXp = 10;
    public int availableStatPoints = 0;

    [Header("--- Currency System (Sistem Baru) ---")]
    public int currentCoin = 0;
    public int currentDiamond = 0; // SAKTI: Variabel Diamond ditambahkan

    [Header("--- Allocated Points Tracker (Display Kanan) ---")]
    public int allocatedHpPoints = 0;
    public int allocatedAttackPoints = 0;
    public int allocatedDefensePoints = 0;
    public int allocatedAccuracyPoints = 0;
    public int allocatedHpRecoveryPoints = 0;
    public int allocatedEvasionPoints = 0;
    public int allocatedCritDamagePoints = 0;
    public int allocatedCritRatePoints = 0;

    [Header("--- Player Default Stats ---")]
    [SerializeField] private float baseHp = 100f;
    [SerializeField] private float baseAttack = 100f;
    [SerializeField] private float baseDefense = 100f;
    [SerializeField] private float baseHpRecovery = 10f;
    [SerializeField] private float baseEvasion = 100f;
    [SerializeField] private float baseCritDamage = 0f;
    [SerializeField] private float baseCritRate = 0f;
    [SerializeField] private float baseAccuracy = 100f;
    [SerializeField] private float baseAttackSpeed = 1f;

    [Header("--- Owned Equipments (Hasil Gacha) ---")]
    public bool ownsRareWeapon = false;
    public bool ownsEpicWeapon = false;
    public bool ownsMythicWeapon = false;

    public bool ownsRareAccessory = false;
    public bool ownsEpicAccessory = false;
    public bool ownsMythicAccessory = false;

    [Header("--- Equipment Slots (Current) ---")]
    public EquipmentTier activeWeaponTier = EquipmentTier.None;
    public EquipmentTier activeAccessoryTier = EquipmentTier.None;

    [Header("--- Skill Slots ---")]
    public SkillType[] equippedSkills = new SkillType[4];

    [Header("--- UI References (Sistem Baru) ---")]
    [Tooltip("Tarik objek Slider HP Player ke sini")]
    [SerializeField] private UIProgressBar playerHpBar;

    public float currentHp { get; private set; }

    void Awake()
    {
        ResetHpToMax();
    }

    public void GainXp(int amount)
    {
        currentXp += amount;
        while (currentXp >= maxXp)
        {
            currentXp -= maxXp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        availableStatPoints += 3;
        maxXp = maxXp + maxXp;
        Debug.Log($"LEVEL UP! Sekarang Level {currentLevel}. Poin Tersedia: {availableStatPoints}. Target XP Berikutnya: {maxXp}");
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        Debug.Log($"[PLAYER STATUS] Koin bertambah +{amount}! Total Koin Sekarang: {currentCoin}");
    }

    // ========================================================
    // SAKTI: FUNGSI MANAJEMEN DIAMOND
    // ========================================================
    public void AddDiamond(int amount)
    {
        currentDiamond += amount;
        Debug.Log($"[CURRENCY] Diamond bertambah +{amount}! Total: {currentDiamond}");
    }

    public bool SpendDiamond(int amount)
    {
        if (currentDiamond >= amount)
        {
            currentDiamond -= amount;
            Debug.Log($"[CURRENCY] Diamond terpakai {amount}. Sisa: {currentDiamond}");
            return true;
        }
        Debug.LogWarning("[CURRENCY] Diamond gak cukup cok!");
        return false;
    }

    // ========================================================
    // SAKTI: FUNGSI KUTUKAN PERMANEN MIMIC GACHA SHOP
    // ========================================================
    public void ApplyMimicWeaponCurse()
    {
        baseAttack = Mathf.Max(1f, baseAttack - 1f); // Ditahan di angka 1 biar gak 0/minus
        Debug.LogWarning($"[MIMIC CURSE] Apes! Attack dasar turun permanen menjadi: {baseAttack}");
    }

    public void ApplyMimicAccessoryCurse()
    {
        baseHp = Mathf.Max(1f, baseHp - 1f); // Ditahan di angka 1 biar gak 0/minus
        ResetHpToMax();
        Debug.LogWarning($"[MIMIC CURSE] Apes! HP dasar turun permanen menjadi: {baseHp}");
    }

    #region STAT POINT ALLOCATION
    public void AllocatePointToHp() { if (availableStatPoints <= 0) return; allocatedHpPoints++; baseHp += 100f; availableStatPoints--; ResetHpToMax(); }
    public void AllocatePointToAttack() { if (availableStatPoints <= 0) return; allocatedAttackPoints++; baseAttack += 100f; availableStatPoints--; }
    public void AllocatePointToDefense() { if (availableStatPoints <= 0) return; allocatedDefensePoints++; baseDefense += 100f; availableStatPoints--; }
    public void AllocatePointToAccuracy() { if (availableStatPoints <= 0) return; allocatedAccuracyPoints++; baseAccuracy += 10f; availableStatPoints--; }
    public void AllocatePointToHpRecovery() { if (availableStatPoints <= 0) return; allocatedHpRecoveryPoints++; baseHpRecovery += 10f; availableStatPoints--; }
    public void AllocatePointToEvasion() { if (availableStatPoints <= 0) return; allocatedEvasionPoints++; baseEvasion += 100f; availableStatPoints--; }
    public void AllocatePointToCritDamage() { if (availableStatPoints <= 0) return; allocatedCritDamagePoints++; baseCritDamage += 5f; availableStatPoints--; }
    public void AllocatePointToCritRate() { if (availableStatPoints <= 0) return; if (baseCritRate >= 100f) { Debug.LogWarning("Crit Rate Maksimal!"); return; } allocatedCritRatePoints++; baseCritRate += 0.5f; if (baseCritRate > 100f) baseCritRate = 100f; availableStatPoints--; }
    #endregion

    public bool HasSkill(SkillType skillToCheck)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] == skillToCheck) return true;
        }
        return false;
    }

    #region GETTERS FOR FINAL STATS
    public float FinalAttack
    {
        get
        {
            int multiplier = 1;
            if (activeWeaponTier == EquipmentTier.Rare) multiplier = 2;
            else if (activeWeaponTier == EquipmentTier.Epic) multiplier = 3;
            else if (activeWeaponTier == EquipmentTier.Mythic) multiplier = 4;

            float stat = baseAttack * multiplier;
            if (HasSkill(SkillType.IronWill)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalMaxHp
    {
        get
        {
            int multiplier = 1;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4;
            return baseHp * multiplier;
        }
    }

    public float FinalDefense
    {
        get
        {
            int multiplier = 1;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4;

            float stat = baseDefense * multiplier;
            if (HasSkill(SkillType.IronBody)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalHpRecovery
    {
        get
        {
            int multiplier = 1;
            if (activeAccessoryTier == EquipmentTier.Rare) multiplier = 2;
            else if (activeAccessoryTier == EquipmentTier.Epic) multiplier = 3;
            else if (activeAccessoryTier == EquipmentTier.Mythic) multiplier = 4;
            return baseHpRecovery * multiplier;
        }
    }

    public float FinalAttackSpeed
    {
        get
        {
            float multiplier = 1f;
            if (activeWeaponTier == EquipmentTier.Rare) multiplier = 2f;
            else if (activeWeaponTier == EquipmentTier.Epic) multiplier = 2f;
            else if (activeWeaponTier == EquipmentTier.Mythic) multiplier = 4f;
            return baseAttackSpeed * multiplier;
        }
    }

    public float FinalEvasion
    {
        get
        {
            float stat = baseEvasion;
            if (HasSkill(SkillType.DancingWaves)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalAccuracy
    {
        get
        {
            float stat = baseAccuracy;
            if (HasSkill(SkillType.IronEye)) stat *= 1.1f;
            return stat;
        }
    }

    public float FinalCritDamage => baseCritDamage;
    public float FinalCritRate => baseCritRate;
    #endregion

    public void ResetHpToMax()
    {
        currentHp = FinalMaxHp;
        if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        Debug.Log($"[PLAYER] HP Berkurang! Sisa HP Player: {currentHp}/{FinalMaxHp}");

        if (playerHpBar != null) playerHpBar.UpdateValue(currentHp, FinalMaxHp);

        if (currentHp <= 0)
        {
            currentHp = 0;
            ResetHpToMax();
            GameplayManager.Instance.OnPlayerDeath();
        }
    }
}