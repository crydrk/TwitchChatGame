using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.Video;



public class UDP_Listen : MonoBehaviour
{
    
    Thread receiveThread;
    UdpClient client;

    

    public string IP = "127.0.0.1";
    public int port = 8052;
    
    public string lastReceivedUDPPacket = "";
    
    private static void Main()
    {
        UDP_Listen receiveObj = new UDP_Listen();
        receiveObj.init();

        string text = "";
        do
        {
            text = Console.ReadLine();
        }
        while (!text.Equals("exit"));
    }
    
    public void Start()
    {
        init();
    }

    public void Update()
    {

    }
    
    private void init()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (true)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                
                string text = Encoding.UTF8.GetString(data);

                Debug.Log(text);

                //SpawnManager.SharedInstance.ParseData(text);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    
}