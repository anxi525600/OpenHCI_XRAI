using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

public class MultiPageFormManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;

    [Header("Input Fields")]
    public TMP_InputField nameField;
    public TMP_InputField experienceField;
    public TMP_InputField questionsField;

    private string nameText = "";
    private string experienceText = "";
    private List<string> questionsList = new List<string>();

    // 分頁切換
    public void GoToPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
    }

    public void GoToPage2()
    {
        nameText = nameField.text;
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
    }

    public void BackToPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        nameField.text = ""; // 清空也可以設為 nameText 讓使用者修改
    }

    public void GoToPage3()
    {
        experienceText = experienceField.text;
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(true);
    }

    public void BackToPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
        experienceField.text = ""; // 同上
    }

    public void SubmitForm()
    {
        string questionsText = questionsField.text;
        questionsList = new List<string>(questionsText.Split('\n'));

        // 建立 JSON 結構
        var promptData = new
        {
            name = nameText,
            experience = experienceText,
            questions = questionsList
        };

        string json = JsonConvert.SerializeObject(promptData, Formatting.Indented);
        Debug.Log("Send JSON:\n" + json);

        // 送出 HTTP Request
        StartCoroutine(SendToServer(json));
    }

    IEnumerator SendToServer(string json)
    {
        string url = "http://localhost:5000/api/prompt"; // 模擬 API endpoint
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("HTTP Error: " + request.error);
        else
            Debug.Log("Server response:\n" + request.downloadHandler.text);
    }
}
