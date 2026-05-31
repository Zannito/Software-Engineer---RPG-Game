using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShopManager : MonoBehaviour
{
    [Header("--- Player Reference ---")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("--- Weapon Banner UI ---")]
    [Tooltip("Tarik Image dari objek Chest di Weapon Draw Parent")]
    [SerializeField] private Image weaponChestImage;

    [Header("--- Accessory Banner UI ---")]
    [Tooltip("Tarik Image dari objek Chest di Accessory Draw Parent")]
    [SerializeField] private Image accessoryChestImage;

    [Header("--- Sprite Resources ---")]
    [Tooltip("Gambar peti diam merem")]
    [SerializeField] private Sprite defaultMimicSprite;

    [Tooltip("Gambar frame mimic pas lagi jilat/melet (Kutukan)")]
    [SerializeField] private Sprite curseMimicSprite; // SAKTI: Slot baru buat gambar mimic melet

    [Space]
    [SerializeField] private Sprite rareWeaponSprite;
    [SerializeField] private Sprite epicWeaponSprite;
    [SerializeField] private Sprite mythicWeaponSprite;
    [Space]
    [SerializeField] private Sprite rareAccessorySprite;
    [SerializeField] private Sprite epicAccessorySprite;
    [SerializeField] private Sprite mythicAccessorySprite;

    private void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerStatus = playerObj.GetComponent<PlayerStatus>();
        }
    }

    // ========================================================
    // LOGIKA DRAW WEAPON (100 Diamond)
    // ========================================================
    public void DrawWeapon()
    {
        if (playerStatus == null) return;

        // Cek dan potong duit premium
        if (!playerStatus.SpendDiamond(100)) return;

        // Kalkulasi persentase (0.0 sampai 100.0)
        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll <= 20f)
        {
            // 20%: Kutukan Mimic (Ganti gambar ke mimic melet)
            playerStatus.ApplyMimicWeaponCurse();
            StartCoroutine(DisplayRewardFeedback(weaponChestImage, curseMimicSprite));
        }
        else if (randomRoll <= 80f)
        {
            // 60%: Rare Weapon (20.01 - 80.0)
            playerStatus.ownsRareWeapon = true; // Otomatis unlock di equipment
            StartCoroutine(DisplayRewardFeedback(weaponChestImage, rareWeaponSprite));
        }
        else if (randomRoll <= 99.5f)
        {
            // 19.5%: Epic Weapon (80.01 - 99.5)
            playerStatus.ownsEpicWeapon = true;
            StartCoroutine(DisplayRewardFeedback(weaponChestImage, epicWeaponSprite));
        }
        else
        {
            // 0.5%: Mythic Weapon (99.51 - 100.0)
            playerStatus.ownsMythicWeapon = true;
            StartCoroutine(DisplayRewardFeedback(weaponChestImage, mythicWeaponSprite));
        }
    }

    // ========================================================
    // LOGIKA DRAW ACCESSORY (100 Diamond)
    // ========================================================
    public void DrawAccessory()
    {
        if (playerStatus == null) return;

        if (!playerStatus.SpendDiamond(100)) return;

        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll <= 20f)
        {
            // 20%: Kutukan Mimic (Ganti gambar ke mimic melet)
            playerStatus.ApplyMimicAccessoryCurse();
            StartCoroutine(DisplayRewardFeedback(accessoryChestImage, curseMimicSprite));
        }
        else if (randomRoll <= 80f)
        {
            // 60%: Rare Accessory
            playerStatus.ownsRareAccessory = true;
            StartCoroutine(DisplayRewardFeedback(accessoryChestImage, rareAccessorySprite));
        }
        else if (randomRoll <= 99.5f)
        {
            // 19.5%: Epic Accessory
            playerStatus.ownsEpicAccessory = true;
            StartCoroutine(DisplayRewardFeedback(accessoryChestImage, epicAccessorySprite));
        }
        else
        {
            // 0.5%: Mythic Accessory
            playerStatus.ownsMythicAccessory = true;
            StartCoroutine(DisplayRewardFeedback(accessoryChestImage, mythicAccessorySprite));
        }
    }

    // SAKTI: Coroutine merubah gambar peti jadi item (atau mimic jilat), tahan 3 detik, lalu balik jadi peti lagi
    private IEnumerator DisplayRewardFeedback(Image chestImage, Sprite rewardSprite)
    {
        if (chestImage == null || rewardSprite == null) yield break;

        chestImage.sprite = rewardSprite;
        yield return new WaitForSeconds(3f);
        chestImage.sprite = defaultMimicSprite;
    }

    // ========================================================
    // SIMULASI BANNER TOP-UP
    // ========================================================
    public void BuyPackage1()
    {
        if (playerStatus == null) return;
        playerStatus.AddCoin(10000);
        playerStatus.AddDiamond(1000);
    }

    public void BuyPackage2()
    {
        if (playerStatus == null) return;
        playerStatus.AddCoin(100000);
        playerStatus.AddDiamond(10000);
    }
}