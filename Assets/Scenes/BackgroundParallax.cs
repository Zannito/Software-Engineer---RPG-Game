using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float backgroundWidth = 19.2f; // Sesuaikan dengan lebar sprite/gambar lu

    // SAKTI: Biar pas nge-loop gak kepanggil dobel gara-gara ada 2 background
    [Header("--- Loop Trigger Settings ---")]
    [Tooltip("Centang ini HANYA pada SATU background saja (misal objek lantai utama) biar gak manggil spawn dobel, cok!")]
    [SerializeField] private bool isLoopTriggerMaster = false;

    private Vector3 startPosition;
    private bool canMove = true;

    void Start()
    {
        // Menyimpan posisi awal koordinat saat game dimulai
        startPosition = transform.position;
    }

    void Update()
    {
        // Jika status canMove bernilai false (misal sedang bertarung), pergerakan berhenti
        if (!canMove) return;

        // Gerakkan background ke kiri
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Jika background sudah bergeser sejauh lebarnya, reset posisinya kembali ke awal
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            ResetPosition();

            // SAKTI: Cuma objek yang dicentang 'isLoopTriggerMaster' yang berhak ngebangunin monster!
            if (isLoopTriggerMaster && GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnMapLoopTriggered();
            }
        }
    }

    // Fungsi untuk mengontrol pergerakan dari script luar (Gameplay Manager)
    public void SetMovement(bool status)
    {
        canMove = status;
    }

    // Fungsi untuk mengembalikan posisi background ke titik semula saat player mati
    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}