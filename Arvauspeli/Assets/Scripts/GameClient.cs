using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class GameClient
{
    public TcpClient tcp;
    public string name;

    public GameClient(TcpClient clientSocket)
    {
        tcp = clientSocket;
        name = "Unidentified";
    }
}


