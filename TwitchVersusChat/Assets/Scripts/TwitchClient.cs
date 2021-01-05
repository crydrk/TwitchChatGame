using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class TwitchClient : MonoBehaviour
{
    public static TwitchClient SharedInstance;

    public Client client;
    private string channel_name = "steakosaurus_rex";

    public UnityFollowerService FollowerService;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        Application.runInBackground = true;

        FollowerService = new UnityFollowerService(TwitchAPI.SharedInstance.api);
        FollowerService.SetChannelByChannelId(Secrets.host_id);
        StartCoroutine(StartFollowerServiceAfterApiInitialized());

        ConnectionCredentials credentials = new ConnectionCredentials("SteakosaurusChatGame", Secrets.bot_access_token);
        client = new Client();
        client.Initialize(credentials, channel_name);

        client.Connect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            client.SendMessage(client.JoinedChannels[0], "Welcome to the channel! Spawn a monster by typing '(mole, cactus" +
                " or steak) wearing a (any kind of) hat named (whatever name you want!)'. You can also spawn a boss monster" +
                " by following, or bring in an entire army of minions by raiding! You can also change the color of the lights" +
                " in my room by typing 'gryffindor', 'hufflepuff', 'slytherin', 'ravenclaw' or 'hunt'.");
        }
    }

    private IEnumerator StartFollowerServiceAfterApiInitialized()
    {
        yield return new WaitUntil(() => TwitchAPI.SharedInstance.api.Settings.ClientId != null);
        FollowerService.StartService();
        Debug.Log("Follower Service Started");
    }


}
