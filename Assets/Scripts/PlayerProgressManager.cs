using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class PlayerProgressManager : MonoBehaviour
{
    public static PlayerProgressManager instance;
    private bool hasSaved = false;

    [Header("Supabase")]
    [SerializeField] private SupabaseManager supabaseManager;

    private string url;
    private string apiKey;
    private string uid;

    [Header("SESSION STATS")]
    public int enemiesKilled;
    public int livesLost;
    public int enemiesEscaped;
    public int goldEarned;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        url = supabaseManager.supabaseUrl;
        apiKey = supabaseManager.supabaseKey;
        uid = supabaseManager.getPlayerUID();

        ResetLevelStats();
    }

    // =========================
    // SAVE FLOW
    // =========================
    public IEnumerator SaveOnGameOver()
    {
        if (hasSaved) yield break;
        hasSaved = true;

        yield return SaveLevelStats();
        yield return UpdateGlobalStats();

        ResetLevelStats();
    }

    // =========================
    // LEVEL STATS (history per run)
    // =========================
    IEnumerator SaveLevelStats()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        string json =
            "{"
            + "\"uid\":\"" + uid + "\","
            + "\"level_name\":\"" + levelName + "\","
            + "\"enemies_killed\":" + enemiesKilled + ","
            + "\"lives_lost\":" + livesLost + ","
            + "\"enemies_escaped\":" + enemiesEscaped + ","
            + "\"gold_earned\":" + goldEarned +
            "}";

        UnityWebRequest request = new UnityWebRequest(url + "/rest/v1/level_stats", "POST");

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", apiKey);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("LevelStats error: " + request.error);
    }

    // =========================
    // GLOBAL STATS (RPC Supabase)
    // =========================
    IEnumerator UpdateGlobalStats()
    {
        string urlRpc = url + "/rest/v1/rpc/add_player_stats";

        string json =
            "{"
            + "\"p_uid\":\"" + uid + "\","
            + "\"p_kills\":" + enemiesKilled + ","
            + "\"p_lives\":" + livesLost + ","
            + "\"p_escaped\":" + enemiesEscaped + ","
            + "\"p_gold\":" + goldEarned +
            "}";

        UnityWebRequest request = new UnityWebRequest(urlRpc, "POST");

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", apiKey);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Prefer", "return=minimal");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("RPC error: " + request.error);
    }

    // =========================
    // RESET LEVEL
    // =========================
    public void ResetLevelStats()
    {
        enemiesKilled = 0;
        livesLost = 0;
        enemiesEscaped = 0;
        goldEarned = 0;
    }
    
    public IEnumerator UpdateTurretStats(
        string turretName,
        int purchases,
        int up1,
        int up2,
        int up3
    )
    {
        string urlRpc = url + "/rest/v1/rpc/update_turret_stats";

        string json =
            "{"
            + "\"p_uid\":\"" + uid + "\","
            + "\"p_turret\":\"" + turretName + "\","
            + "\"p_purchase\":" + purchases + ","
            + "\"p_up1\":" + up1 + ","
            + "\"p_up2\":" + up2 + ","
            + "\"p_up3\":" + up3 +
            "}";

        UnityWebRequest request = new UnityWebRequest(urlRpc, "POST");

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", apiKey);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
    }
}