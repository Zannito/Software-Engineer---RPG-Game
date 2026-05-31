using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public enum ItemType { Coin, Xp }

    [Header("--- Item Settings ---")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private int valueAmount = 5; // Gua set default 5 biar kerasa pas nambah

    [Header("--- Magnet Mechanics ---")]
    [SerializeField] private float magnetRadius = 5f; // Jarak aman mulai kesedot
    [SerializeField] private float moveSpeed = 10f;    // Gua naikin ke 10 biar sedotannya makin kencang

    private Transform playerTransform;
    private PlayerStatus playerStatus;
    private bool isBeingAttracted = false;

    void Start()
    {
        // Cari objek Player secara otomatis di dalam Scene berdasarkan Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStatus = playerObj.GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Hitung jarak real-time antara item ini dengan posisi Player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Jika player sudah masuk radius atau item sudah dalam status kesedot
        if (distanceToPlayer <= magnetRadius || isBeingAttracted)
        {
            isBeingAttracted = true;

            // Gerakkan item menuju posisi tengah Player (efek magnet)
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // SOLUSI NGSTUCK: Jika jaraknya udah super dekat (hampir nempel pusat player), langsung sedot masuk sistem!
            if (distanceToPlayer <= 0.2f)
            {
                CollectItem();
            }
        }
    }

    // Fungsi alternatif cadangan jika koin kebetulan menabrak Collider Player duluan
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    /// <summary>
    /// Logika eksekusi penambahan koin/XP ke script PlayerStatus dan menghancurkan objek di layar
    /// </summary>
    private void CollectItem()
    {
        if (playerStatus != null)
        {
            if (itemType == ItemType.Coin)
            {
                playerStatus.AddCoin(valueAmount);
            }
            else if (itemType == ItemType.Xp)
            {
                // Ini bakal nambah XP dan otomatis mentrigger fungsi LevelUp() di PlayerStatus lu kalau tembus maxXp
                playerStatus.GainXp(valueAmount);
            }
        }

        // Langsung lenyap dari layar, gak bakal ngestuck jadi hantu di belakang player lagi!
        Destroy(gameObject);
    }
}