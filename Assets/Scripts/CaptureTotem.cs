using System.Collections;
using UnityEngine;

public class CaptureTotem : MonoBehaviour
{
    public float captureTime = 5f;  // Time required to capture the totem
    public float respawnTime = 5f;  // Time after which the totem will reappear
    public float failCaptureTime = 5f;  // Time until capture failure
    public Vector3 spawnAreaCenter;  // Center of the spawn area
    public Vector3 spawnAreaSize;    // Size of the spawn area
    public float minDistanceFromLastPosition = 5f; // Minimum distance to avoid spawning too close
    public float groundLevelY = 0f; // Fixed ground level for y-coordinate

    private float timer = 0f;  // Timer to track player's time in the zone
    private bool playerInZone = false;
    private bool totemCaptured = false;
    private Vector3 lastPosition; // Last position of the totem
    private int lastLoggedSecond = 0;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (playerInZone && !totemCaptured)
        {
            timer += Time.deltaTime;
            int currentSecond = Mathf.FloorToInt(timer);
            if (currentSecond != lastLoggedSecond)
            {
                Debug.Log("Timer: " + currentSecond);
                lastLoggedSecond = currentSecond;
            }

            if (timer >= captureTime)
            {
                Capture();
            }
        }

        if (!playerInZone && timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                FailToCapture();
            }
        }
    }

    private void Capture()
    {
        totemCaptured = true;
        Debug.Log("Totem Captured!");
        PlayerBuffs playerBuffs = FindObjectOfType<PlayerBuffs>();
        if (playerBuffs != null)
        {
            playerBuffs.ApplyRandomBuff();
        }
        StartCoroutine(RespawnTotem());
    }

    private void FailToCapture()
    {
        totemCaptured = true;
        Debug.Log("Failed to Capture Totem!");
        PlayerBuffs playerBuffs = FindObjectOfType<PlayerBuffs>();
        if (playerBuffs != null)
        {
            playerBuffs.ApplyRandomDebuff();
        }
        StartCoroutine(RespawnTotem());
    }

    private IEnumerator RespawnTotem()
    {
        Debug.Log("Totem will respawn in " + respawnTime + " seconds");
        yield return new WaitForSeconds(respawnTime);

        Vector3 newPosition;
        do
        {
            newPosition = GetRandomPosition();
        } while (Vector3.Distance(newPosition, lastPosition) < minDistanceFromLastPosition);

        transform.position = newPosition;
        lastPosition = newPosition;

        totemCaptured = false;
        timer = 0f;
        lastLoggedSecond = 0;
        Debug.Log("Totem Respawned at position: " + transform.position);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            groundLevelY,
            Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
        );
        return randomPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!totemCaptured)
            {
                playerInZone = true;
                timer = 0f;  // Reset timer when player enters the zone
                Debug.Log("Player entered the totem zone.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!totemCaptured)
            {
                playerInZone = false;
                timer = failCaptureTime;  // Start countdown to failure
                Debug.Log("Player exited the totem zone, starting failure countdown.");
            }
        }
    }
}
