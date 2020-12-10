using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    public int port = 6321;
    public GameObject lobbyIDGroup;
    public GameObject lobbyStatusGroup;
    public GameObject lobbyIPGroup;
    public GameObject lobbyPortGroup;
    public GameObject lobbyIDPrefab;
    public GameObject lobbyStatusPrefab;
    public GameObject lobbyIPPrefab;
    public GameObject lobbyPortPrefab;
    public Text debugText;
    private TcpListener server;
    private bool serverStarted;

    private void Start()
    {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on port " + port.ToString());
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
        foreach (ServerClient client in clients)
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

    private void OnIncomingData(ServerClient client, string data)
    {
        Debug.Log("Received data: " + data);
        if (data.Contains("&HOST"))
        {
            client.lobbyIP = ((IPEndPoint)client.tcp.Client.RemoteEndPoint).Address.ToString();
            debugText.text = "Clients IP should be: " + client.lobbyIP;
            //client.lobbyIP = data.Split('|')[1];
            client.lobbyIPField = Instantiate(lobbyIPPrefab, lobbyIPGroup.transform);
            int.TryParse(data.Split('|')[1], out client.lobbyPort);
            client.lobbyPortField = Instantiate(lobbyPortPrefab, lobbyPortGroup.transform);
            AssignID(client);
            client.lobbyIDField = Instantiate(lobbyIDPrefab, lobbyIDGroup.transform);
            UpdateFields(client);
            Broadcast("&IP|" + client.lobbyIP, new List<ServerClient>() { client });
            return;

        }
        else if(data.Contains("%LOBBY"))
        {
            int askedLobbyNumber;
            int.TryParse(data.Split('|')[1], out askedLobbyNumber);
            FindLobby(client,askedLobbyNumber);
            return;
        }
        else if (data.Contains("%STATUS"))
        {
            client.lobbyStatus = data.Split('|')[1];
        }
    }

    private void UpdateFields(ServerClient client)
    {
        client.lobbyIDField.GetComponent<Text>().text = client.lobbyID.ToString();
        client.lobbyIPField.GetComponent<Text>().text = client.lobbyIP.ToString();
        client.lobbyPortField.GetComponent<Text>().text = client.lobbyPort.ToString();
        client.lobbyStatusField.GetComponent<Text>().text = client.lobbyStatus.ToString();
    }

    private void FindLobby(ServerClient client, int lobbyID)
    {
        foreach (ServerClient c in clients)
        {
            if (lobbyID == c.lobbyID)
            {
                Broadcast("&IP|" + c.lobbyIP + "|"+c.lobbyPort, new List<ServerClient>() { client });
                return;
            }
        }
        Debug.Log("Sending Lobby not found");
        Broadcast("&ERROR|Lobby Not Found", new List<ServerClient>() { client });
    }

    private void AssignID(ServerClient client)
    {
        int ID = UnityEngine.Random.Range(10000, 100000);
        bool found = false;
        foreach (ServerClient c in clients)
        {
            if(ID==c.lobbyID)
            {
                found = true;
            }            
        }
        if (!found)
        {
            client.lobbyID = ID;
            Broadcast("&ID|"+ID, new List<ServerClient>() { client });
        }
        else
        {
            AssignID(client);
        }
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Client.Connected) // if client does not exist, he cannot be connected. If the client does not have a socket assigned then he cannot be connected. If that socket exists and is connected, we can check if everything else works.
            {
                if (client.Client.Poll(0, SelectMode.SelectRead)) // if the socket can be read from, or the socket has Listen active(it's listening for inc. messages), or even if the connection is closed. basically tests whether or not any connection info exists on socket?
                {
                    return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0); // We try and receive 1 byte info(because we check 1 flag) from the socket that tells us which socketflags are active on the socket, specifically if the socket is peeking. Not sure why though.
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

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening(); // start listening for new connections again.
        // ask if the connection game from a gamehost.
        Broadcast("%HOST", new List<ServerClient>() { clients[clients.Count - 1] }); // i debated changing this into ServerClient instead of list, and just keeping the hosts, but this probably helps with keeping up multiple connection attempts. 
    }

    private void Broadcast(string data, List<ServerClient> clients)
    {
        Debug.Log("Sending: " + data);
        foreach (ServerClient client in clients)
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
}

public class ServerClient
{
    public TcpClient tcp;
    public int lobbyID;
    public string lobbyIP;
    public string lobbyStatus;
    public int lobbyPort;
    public GameObject lobbyIDField;
    public GameObject lobbyIPField;
    public GameObject lobbyPortField;
    public GameObject lobbyStatusField;

    public ServerClient(TcpClient clientSocket)
    {
        tcp = clientSocket;
        lobbyID = 0;
        lobbyPort = 0;
        lobbyStatus = "Initializing";
    }    
}
