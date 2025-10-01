using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;

public class SubmitScoreApi : MonoBehaviour
{
    [Serializable]
    private class SubmitScoreRequest
    {
        public int session_id;
        public int score;
        public int clicks;
        public int gstrain_collected;
        public int fstrain_collected;
        public string perk_one;
        public string perk_two;
        public string perk_three;
    }

    [Serializable]
    private class SubmitScoreResponse
    {
        public bool ok;
        public int score_id;
        public string error;
    }

    private const string SubmitScoreUrl = "http://localhost:5173/api/submit-score";

    public IEnumerator SubmitScore(GameData gameData)
    {
        SubmitScoreRequest reqObj = new SubmitScoreRequest
        {
            session_id = 1,
            score = gameData.iq,
            clicks = gameData.brainClicks,
            gstrain_collected = gameData.goldenStrainsCollected,
            fstrain_collected = gameData.falseStrainsCollected,
            perk_one = gameData.perks[0].name,
            perk_two = gameData.perks[1].name,
            perk_three = gameData.perks[2].name,
        };

        string json = JsonUtility.ToJson(reqObj);

        UnityWebRequest www = new UnityWebRequest(SubmitScoreUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string text = www.downloadHandler.text;
            try
            {
                SubmitScoreResponse res = JsonUtility.FromJson<SubmitScoreResponse>(text);
                if (res != null && res.ok)
                {
                    Debug.Log($"Score saved. score_id = {res.score_id}");
                }
                else
                {
                    Debug.LogError($"Server did not return ok. Response: {text}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}\nResponse: {text}");
            }
        }
        else
        {
            Debug.LogError($"HTTP error: {www.responseCode} - {www.error}");
            try
            {
                string text = www.downloadHandler.text;
                SubmitScoreResponse res = JsonUtility.FromJson<SubmitScoreResponse>(text);
                if (res != null && !string.IsNullOrEmpty(res.error))
                {
                    Debug.LogError($"The server provided the following error message: {res.error}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}");
            }
        }
    }

    // private void Start()
    // {
    //     int sessionId = 3;
    //     int levelId = 1;
    //     StartCoroutine(SubmitScore(sessionId, levelId, 2552, 72, 64.2f));
    // }

    public void SubmitScoreToDB(GameData gameData)
    {
        StartCoroutine(SubmitScore(gameData));
    }
}

