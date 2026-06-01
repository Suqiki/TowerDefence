using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Text;
using TMPro;

public class SupabaseManager : MonoBehaviour
{
    [Header("Supabase")]
    public string supabaseUrl = "https://jpljxngyucwnvbtxwahs.supabase.co";
    public string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpwbGp4bmd5dWN3bnZidHh3YWhzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Nzk3MTM2NDEsImV4cCI6MjA5NTI4OTY0MX0.l_yd57LlY8DS0TzF0LQ9JBRmSCXDF3nsV1NqdA4l9iA";

    private string playerUID;
    
    [Header("UI")]
    public TextMeshProUGUI uidText;

    public string getPlayerUID()
    {
        return  playerUID;
    }
    void Start()
    {
        Debug.Log("SUPABASE URL = >" + supabaseUrl + "<");
        
        CreateOrLoadUID();
        
        ShowUID();
        
        StartCoroutine(CheckPlayerExists());
    }
    
    void ShowUID()
    {
        if (uidText != null)
        {
            string shortUID = playerUID.Substring(0, 8);
            uidText.text = "UID: " + shortUID;
        }
    }

    void CreateOrLoadUID()
    {
        if (PlayerPrefs.HasKey("PLAYER_UID"))
        {
            playerUID = PlayerPrefs.GetString("PLAYER_UID");
            Debug.Log("Loaded UID: " + playerUID);
        }
        else
        {
            playerUID = Guid.NewGuid().ToString();

            PlayerPrefs.SetString("PLAYER_UID", playerUID);
            PlayerPrefs.Save();

            Debug.Log("Created UID: " + playerUID);
        }
    }

    IEnumerator CheckPlayerExists()
    {
        string url = $"{supabaseUrl}/rest/v1/players?uid=eq.{playerUID}";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", $"Bearer {supabaseKey}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("GET error: " + request.error);
            yield break;
        }

        string response = request.downloadHandler.text;
        Debug.Log("Response: " + response);

        if (response == "[]")
        {
            yield return StartCoroutine(CreatePlayer());
            yield return StartCoroutine(CreatePlayerStats());        }
        else
        {
            Debug.Log("Player already exists!");
        }
    }

    IEnumerator CreatePlayer()
    {
        string url = $"{supabaseUrl}/rest/v1/players";

        string json = JsonUtility.ToJson(new PlayerData(playerUID));

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", $"Bearer {supabaseKey}");
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("POST error: " + request.error);
        }
        else
        {
            Debug.Log("Player created successfully!");
        }
    }
    
    IEnumerator CreatePlayerStats()
    {
        string url = $"{supabaseUrl}/rest/v1/player_stats";

        string json =
            "{"
            + "\"uid\":\"" + playerUID + "\""
            + "}";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", $"Bearer {supabaseKey}");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("CreatePlayerStats ERROR: " + request.error);
        }
        else
        {
            Debug.Log("Player stats created!");
        }
    }
    
    public IEnumerator UpdateLevelReached(int level)
    {
        string url = $"{supabaseUrl}/rest/v1/players?uid=eq.{playerUID}";

        string json =
            "{"
            + "\"level_reached\":" + level +
            "}";

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("Level update error: " + request.error);
        else
            Debug.Log("Level reached updated in Supabase!");
    }

    [Serializable]
    public class PlayerData
    {
        public string uid;
        public bool tutorial_completed;
        public int level_reached;

        public PlayerData(string uid)
        {
            this.uid = uid;
            tutorial_completed = false;
            level_reached = 1;
        }
    }
    
    public IEnumerator SetTutorialCompleted(bool value)
    {
        string json = "{\"tutorial_completed\":" + value.ToString().ToLower() + "}";

        UnityWebRequest request = new UnityWebRequest(
            supabaseUrl + "/rest/v1/players?uid=eq." + playerUID,
            "PATCH"
        );

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError(request.error);
    }
    
    IEnumerator UpdateTutorial(bool value)
    {
        string url = $"{supabaseUrl}/rest/v1/players?uid=eq.{playerUID}";

        string json = "{\"tutorial_completed\":" + value.ToString().ToLower() + "}";

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", $"Bearer {supabaseKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Tutorial saved in Supabase!");
        }
        else
        {
            Debug.LogError("Failed updating tutorial: " + request.error);
        }
    }
    
    
}