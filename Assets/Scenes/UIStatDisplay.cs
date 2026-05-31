using UnityEngine;
using TMPro;

public class UIStatDisplay : MonoBehaviour
{
    [Header("--- Player Status Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Top Left Profile Header References ---")]
    [Tooltip("Tarik objek 'Border Coin' ke sini")]
    [SerializeField] private Transform borderCoin;

    // SAKTI: Tambahan Slot Diamond
    [Tooltip("Tarik objek 'Border Diamond' ke sini")]
    [SerializeField] private Transform borderDiamond;

    [Header("--- Main Profile Panel Parent (Kotak Besar Final Stats) ---")]
    [SerializeField] private Transform panelProfileStats;

    private Transform finalHpParent;
    private Transform finalAttackParent;
    private Transform finalDefenseParent;
    private Transform finalAccuracyParent;
    private Transform finalHpRecoveryParent;
    private Transform finalEvasionParent;
    private Transform finalCritDamageParent;
    private Transform finalCritChanceParent;
    private Transform finalAttackSpeedParent;

    void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerStatus = playerObj.GetComponent<PlayerStatus>();
        }

        if (panelProfileStats != null)
        {
            finalHpParent = panelProfileStats.Find("Final HP Parent");
            finalAttackParent = panelProfileStats.Find("Final Attack Parent");
            finalDefenseParent = panelProfileStats.Find("Final Defense Parent");
            finalAccuracyParent = panelProfileStats.Find("Final Accuracy Parent");
            finalHpRecoveryParent = panelProfileStats.Find("Final HP Recovery Parent");
            finalEvasionParent = panelProfileStats.Find("Final Evasion Parent");
            finalCritDamageParent = panelProfileStats.Find("Final Crit Damage Parent");
            finalCritChanceParent = panelProfileStats.Find("Final Crit Rate Parent");
            finalAttackSpeedParent = panelProfileStats.Find("Final Attack Speed Parent");
        }
    }

    void Update()
    {
        if (playerStatus == null) return;

        // 1. UPDATE KOTAK KOIN & DIAMOND
        if (borderCoin != null) UpdateValueText(borderCoin, playerStatus.currentCoin.ToString());
        if (borderDiamond != null) UpdateValueText(borderDiamond, playerStatus.currentDiamond.ToString());

        // 2. UPDATE ISI ANGKA REAL-TIME DI PANEL BESAR PROFILE
        if (panelProfileStats != null && panelProfileStats.gameObject.activeSelf)
        {
            UpdateValueText(finalHpParent, Mathf.RoundToInt(playerStatus.currentHp) + " / " + Mathf.RoundToInt(playerStatus.FinalMaxHp));
            UpdateValueText(finalAttackParent, Mathf.RoundToInt(playerStatus.FinalAttack).ToString());
            UpdateValueText(finalDefenseParent, Mathf.RoundToInt(playerStatus.FinalDefense).ToString());
            UpdateValueText(finalAccuracyParent, Mathf.RoundToInt(playerStatus.FinalAccuracy).ToString());
            UpdateValueText(finalHpRecoveryParent, Mathf.RoundToInt(playerStatus.FinalHpRecovery).ToString());
            UpdateValueText(finalEvasionParent, Mathf.RoundToInt(playerStatus.FinalEvasion).ToString());
            UpdateValueText(finalCritDamageParent, playerStatus.FinalCritDamage + "%");
            UpdateValueText(finalCritChanceParent, playerStatus.FinalCritRate + "%");
            UpdateValueText(finalAttackSpeedParent, playerStatus.FinalAttackSpeed.ToString("F1") + "/s");
        }
    }

    private void UpdateValueText(Transform parentTransform, string newValue)
    {
        if (parentTransform == null) return;

        Transform valueChild = parentTransform.Find("isi");
        if (valueChild != null)
        {
            TextMeshProUGUI tmp = valueChild.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = newValue;
        }
    }
}