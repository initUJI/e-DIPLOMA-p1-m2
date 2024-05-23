using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MoodleAuth : MonoBehaviour
{
    private string middlewareUrl = "http://localhost:3000"; // URL del servidor intermediario

    public void Login(string username, string password)
    {
        StartCoroutine(LoginCoroutine(username, password));
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        // Crear la solicitud de autenticaci�n
        string loginUrl = $"{middlewareUrl}/login";
        UnityWebRequest webRequest = new UnityWebRequest(loginUrl, "POST");

        // Crear los datos del formulario
        string jsonData = JsonUtility.ToJson(new { username, password });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError("Error al realizar la solicitud de autenticaci�n: " + webRequest.error);
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;
            Debug.Log("Respuesta de autenticaci�n: " + responseText);

            var jsonResponse = JsonUtility.FromJson<LoginResponse>(responseText);
            if (jsonResponse.sessionCookie != null)
            {
                // Usar la cookie de sesi�n para obtener informaci�n del usuario
                GetUserInfo(jsonResponse.sessionCookie);
            }
            else
            {
                Debug.LogError("Autenticaci�n fallida");
            }
        }
    }

    private void GetUserInfo(string sessionCookie)
    {
        StartCoroutine(GetUserInfoCoroutine(sessionCookie));
    }

    private IEnumerator GetUserInfoCoroutine(string sessionCookie)
    {
        // Crear la solicitud para obtener informaci�n del usuario
        string userInfoUrl = $"{middlewareUrl}/get_user_info?sessionCookie={sessionCookie}";
        UnityWebRequest webRequest = UnityWebRequest.Get(userInfoUrl);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError("Error al obtener la informaci�n del usuario: " + webRequest.error);
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;
            Debug.Log("Informaci�n del usuario: " + responseText);

            // Procesar la respuesta JSON para obtener el ID del usuario
            var userInfoResponse = JsonUtility.FromJson<SiteInfoResponse>(responseText);
            Debug.Log("El ID del usuario actual es: " + userInfoResponse.userid);
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string sessionCookie;
    }

    [System.Serializable]
    public class SiteInfoResponse
    {
        public int userid;
        public string siteurl;
        public string sitename;
        public string fullname;
        public string username;
        public string lang;
        public string useridnumber;
        public string userpictureurl;
        public string useremail;
        // Agrega otros campos seg�n sea necesario
    }
}

