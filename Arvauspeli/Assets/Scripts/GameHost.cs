using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class GameHost : MonoBehaviour
{
    private TcpListener server;
    private List<GameClient> clients;
    private List<GameClient> disconnectList;
    public int port = 1234;
    private bool serverStarted = false;
    private string IP = null;
    
    public void SetIP(string IP)
    {
        this.IP = IP;
        Debug.Log("Server started on: " + this.IP);
    }
    public void StartServer()
    {
        clients = new List<GameClient>();
        disconnectList = new List<GameClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListening();
            serverStarted = true;
            /*IPHostEntry host = Dns.GetHostEntry("localhost");
            IP = host.AddressList[0].ToString(); // not entirely sure if this works, the examples i found are now part of deprecated code.
            foreach(IPAddress IPaddr in host.AddressList)
            {
                Debug.Log("IPAddr: " + IPaddr.ToString());
            }*/
            
            
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
        {
            return;
        }        
        foreach (GameClient client in clients)
        {
            if (!IsConnected(client.tcp))
            {
                client.tcp.Close();
                disconnectList.Add(client);
                continue;
            }
            else
            {
                NetworkStream s = client.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(client, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
            //remove all the UI elements for that lobby.
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new GameClient(listener.EndAcceptTcpClient(ar)));
        StartListening(); // start listening for new connections again.

        Broadcast("%HOST", new List<GameClient>() { clients[clients.Count - 1] }); 
    }

    private void Broadcast(string data, List<GameClient> clients)
    {
        Debug.Log("Sending: " + data);
        foreach (GameClient client in clients)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error: " + e.Message);
            }
        }
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Client.Connected) 
            {
                if (client.Client.Poll(0, SelectMode.SelectRead)) 
                {
                    return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0); 
                }

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    private void OnIncomingData(GameClient client, string data)
    {
        /*Debug.Log("Received data: " + data);
        if (data.Contains("&HOST"))
        {
            client.lobbyIP = data.Split('|')[1];
            client.lobbyIPField = Instantiate(lobbyIPPrefab, lobbyIPGroup.transform);
            int.TryParse(data.Split('|')[2], out client.lobbyPort);
            client.lobbyPortField = Instantiate(lobbyPortPrefab, lobbyPortGroup.transform);
            AssignID(client);
            client.lobbyIDField = Instantiate(lobbyIDPrefab, lobbyIDGroup.transform);
            UpdateFields(client);
            return;
        }
        else if (data.Contains("%LOBBY"))
        {
            int askedLobbyNumber;
            int.TryParse(data.Split('|')[1], out askedLobbyNumber);
            FindLobby(client, askedLobbyNumber);
            return;
        }
        else if (data.Contains("%STATUS"))
        {
            client.lobbyStatus = data.Split('|')[1];
        }*/
    }
}
