using UnityEngine;
using System;

[Serializable]
public class SignUpData
{
    public string email;
    public string password;
    public string createDate;

    public SignUpData(string email, string password)
    {
        this.email = email;
        this.password = password;
        this.createDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

