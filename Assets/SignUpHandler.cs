using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class User
{
    public int id;
    public string email;
    public string password;
    public string created_at;
}

[Serializable]
public class ServerResponse
{
    public string status;
    public List<User> data;
}

public class SignUpHandler : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField reEnterPasswordInput;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string rePassword = reEnterPasswordInput.text;

        if (password != rePassword)
        {
            Debug.LogWarning("Passwords do not match");
            return;
        }

        StartCoroutine(SendSignUpData(email, password));
    }

    IEnumerator SendSignUpData(string email, string password)
    {
        string url = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Unity)");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Server responded: " + json);

            try
            {
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(json);
                if (response != null && response.status == "success")
                {
                    foreach (User user in response.data)
                    {
                        Debug.Log("ID: " + user.id + " | Email: " + user.email + " | Created At: " + user.created_at);
                    }
                }
                else
                {
                    Debug.LogWarning("Server response parsed, but no success status.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Sign-up failed: " + request.error + "\n" + request.downloadHandler.text);
        }
    }
}

