using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class OauthTest : MonoBehaviour
{
    private static string loopbackURL = "http://localhost/7000";

    public static string LoopBackURL
    {
        get
        {
            return loopbackURL;
        }
    }
 


    public Dictionary<string, string> contents = new Dictionary<string, string>();
    public List<string> redirect_uris = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        contents.Add("client_id", "370245614603-llrcfe8rukg2c8q8hm6r8tcpeh508sp7.apps.googleusercontent.com");
        contents.Add("project_id", "united-aura-310007");
        contents.Add("auth_uri", "https://accounts.google.com/o/oauth2/auth");
        contents.Add("token_uri", "https://oauth2.googleapis.com/token");
        contents.Add("auth_provider_x509_cert_url", "https://www.googleapis.com/oauth2/v1/certs");

        redirect_uris.Add("urn:ietf:wg:oauth:2.0:oob");
        redirect_uris.Add("http://localhost/7000");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static bool ConnectOauthToken()
    {
        

        return true;
    }
}
