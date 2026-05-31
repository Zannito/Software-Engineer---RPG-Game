using UnityEngine;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("--- References ---")]
    [Tooltip("Tarik objek Player lu ke sini")]
    [SerializeField] private Transform playerTransform;
    [Tooltip("Tarik objek Player lu yang punya komponen Animator ke sini")]
    [SerializeField] private Animator playerAnimator;

    [Header("--- Backgrounds to Stop (Bisa Banyak Objek) ---")]
    [Tooltip("Tarik SEMUA objek background/floor yang bergerak ke sini (Size: 2)")]
    [SerializeField] private BackgroundParallax[] movingBackgrounds;

    [Header("--- Settings ---")]
    public float MapSpeed = 3.5f;
    public bool IsMapMoving { get; private set; } = true;

    [Tooltip("Jarak minimal antara Player dan Monster untuk memulai tarung (Cocok di angka 1 sampai 1.5)")]
    [SerializeField] private float attackDistanceThreshold = 1.2f;

    [Header("--- Queue List (Isi 5 Monster Berurutan) ---")]
    [SerializeField] private List<Monster> monsterQueue = new List<Monster>();

    // List internal sakti buat nyatet susunan 5 monster asli lu pas awal game
    private List<Monster> masterMonsterList = new List<Monster>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Catat dan kunci susunan 5 monster yang lu susun manual di Hierarchy
        masterMonsterList = new List<Monster>(monsterQueue);
        SetMapMovement(true);
    }

    void Update()
    {
        // SISTEM DETEKSI ASLI LU: Menggunakan Jarak Riil (Distance), bukan koordinat X mentah
        if (monsterQueue.Count > 0 && IsMapMoving)
        {
            // Pastikan objek monster pertama tidak kosong/null dan sedang aktif
            if (monsterQueue[0] != null && playerTransform != null && monsterQueue[0].gameObject.activeSelf)
            {
                // Hitung jarak asli antara koordinat Player dan Monster pertama
                float realDistance = Vector3.Distance(playerTransform.position, monsterQueue[0].transform.position);

                // Game HANYA BOLEH berhenti dan attack jika jarak mereka sudah benar-benar dekat!
                if (realDistance <= attackDistanceThreshold)
                {
                    TriggerBattleState();
                }
            }
        }
    }

    private void TriggerBattleState()
    {
        SetMapMovement(false); // Semua background otomatis berhenti
        if (playerAnimator != null) playerAnimator.SetBool("isAttacking", true); // Player memukul

        if (monsterQueue.Count > 0 && monsterQueue[0] != null)
        {
            monsterQueue[0].StartFighting();

            // AMBIL SCRIPT PLAYERSTATUS YANG ADA DI BADAN PLAYER
            PlayerStatus playerStatus = playerTransform.GetComponent<PlayerStatus>();

            // PICU SISTEM PERTEMPURAN REAL-TIME OTOMATIS DI SINI!
            if (BattleSystem.Instance != null && playerStatus != null)
            {
                BattleSystem.Instance.StartBattle(playerStatus, monsterQueue[0]);
            }
        }
    }

    public void OnMonsterDefeated()
    {
        // MATIKAN BATTLE SYSTEM KARENA MUSUHNYA SUDAH MATI
        if (BattleSystem.Instance != null) BattleSystem.Instance.EndBattle();

        if (monsterQueue.Count > 0) monsterQueue.RemoveAt(0);

        if (monsterQueue.Count > 0)
        {
            SetMapMovement(true); // Jalan lagi nyamperin monster ke-2
            if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);
        }
        else
        {
            // Pas monster ke-5 mampus, map jalan kosong nungguin background map nge-loop
            Debug.Log("Semua kloter monster mampus! Jalan terus nunggu map looping.");
            SetMapMovement(true);
            if (playerAnimator != null) playerAnimator.SetBool("isAttacking", false);
        }
    }

    // ========================================================
    // SAKTI: FUNGSI BARU UNTUK SPAWN LOOPING MONSTER HIERARCHY
    // ========================================================
    /// <summary>
    /// PANGGIL FUNGSI INI DI SCRIPT BACKGROUND LU PAS GAMBARNYA RESET NGE-LOOP!
    /// </summary>
    public void OnMapLoopTriggered()
    {
        Debug.Log("[LOOPING] Map nge-reset! Bangkitkan kembali 5 monster di posisi semula!");

        // 1. Kosongkan sisa antrean lama biar steril
        monsterQueue.Clear();

        // 2. Loop list master cadangan, bangkitkan kodenya, dan masukin antrean lagi berurutan
        foreach (Monster m in masterMonsterList)
        {
            if (m != null)
            {
                m.ResetMonster(); // Teleport balik, isi darah penuh, set aktif true!
                monsterQueue.Add(m); // Antre lagi buat digebuk player
            }
        }

        SetMapMovement(true);
    }

    public void OnPlayerDeath()
    {
        if (BattleSystem.Instance != null) BattleSystem.Instance.EndBattle();
        SetMapMovement(false);

        // Reset semua background yang didaftarkan
        foreach (BackgroundParallax bp in movingBackgrounds)
        {
            if (bp != null) bp.ResetPosition();
        }

        // Kalau player mati, sekalian reset monsternya biar seger lagi dari awal
        OnMapLoopTriggered();
    }

    private void SetMapMovement(bool status)
    {
        IsMapMoving = status;
        // Mengontrol semua background secara serempak lewat loop
        foreach (BackgroundParallax bp in movingBackgrounds)
        {
            if (bp != null) bp.SetMovement(status);
        }
    }
}