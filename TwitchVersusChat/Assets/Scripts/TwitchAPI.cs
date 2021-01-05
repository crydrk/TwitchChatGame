using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;

public class TwitchAPI : MonoBehaviour
{
    public static TwitchAPI SharedInstance;
    public Api api;

    private void Awake()
    {
        SharedInstance = this;

        api = new Api();
        api.Settings.AccessToken = Secrets.bot_access_token;
        api.Settings.ClientId = Secrets.client_id;
    }
    
    void Start()
    {
        Application.runInBackground = true;
        
    }
    
    void Update()
    {

    }
}
