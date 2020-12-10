using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class LobbyServerClient : MonoBehaviour
{
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool socketReady = false;
    public bool IAmHost = false; // remember to change this to true on canvasgroup.
    public string host = "127.0.0.1";
    public int port = 6321;
    GameHost gh = null;
    
	// Update is called once per frame
	void Update ()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    public void ConnectToServer()
    {
        //if already connected, ignore this function
        if (socketReady)
            return;       

        // create the socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
            gh = GameObject.Find("GameHost").GetComponent<GameHost>();
            gh.StartServer();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private void OnIncomingData(string data)
    {
        Debug.Log("received: " + data);
        if (data == "%HOST")
        {
            if (IAmHost && gh != null)
            {                
                int port = gh.port;                
                Send("&HOST|" + port);
            }
            else
            {
                int lobbyNumber = 0; // get this from the client.
                Send("%LOBBY|" + lobbyNumber);
            }
            return;
        }
        if(data.Contains("&IP"))
        {
            gh.SetIP(data.Split('|')[1]);
        }
        else if(data.Contains("&ERROR"))
        {
            // tell client what error arrived.
        }
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;

        Debug.Log("Sent: " + data);
        writer.WriteLine(data);
        writer.Flush();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }
}
