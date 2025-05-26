using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;


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
            Debug.LogWarning("Passwords do not match!");
            return;
        }

        SignUpData data = new SignUpData(email, password);
        string json = data.ToJson();

        StartCoroutine(SendSignUpData(json));
    }

    IEnumerator SendSignUpData(string json)
    {
        string url = "https://website.david.drago.com/login";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sign-up successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Sign-up failed: " + request.error);
        }
    }
}

