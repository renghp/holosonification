﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
using System.Threading.Tasks;
#endif

public class clientDualSystem : MonoBehaviour
{
    public Text feedback;
    public Text inputHost;

    //public USTrackingManager TrackingManager;
    //public USStatusTextManager StatusTextManager;

#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    System.Net.Sockets.TcpClient client;
    System.Net.Sockets.NetworkStream stream;
    private Thread exchangeThread;
#endif

    private Byte[] bytes = new Byte[256];
    private StreamWriter writer;
    private StreamReader reader;


    public void OnSelect()
    {

        string host = "192.168.43.1";
        int port = 6321;

        if (inputHost.text != "" && inputHost.text != null)
            host = inputHost.text;

        feedback.text = "Connecting to " + host + " on port " + port.ToString();


        Connect(host, port.ToString());

    }

    public void Connect(string host, string port)
    {
        if (_useUWP)
        {
            ConnectUWP(host, port);
        }
        else
        {
            ConnectUnity(host, port);
        }
    }



#if UNITY_EDITOR
    private void ConnectUWP(string host, string port)
#else
    private async void ConnectUWP(string host, string port)
#endif
    {
#if UNITY_EDITOR
        feedback.text = "UWP TCP client used in Unity!";
#else
        try
        {
            if (exchangeTask != null) StopExchange();
        
            socket = new Windows.Networking.Sockets.StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
            await socket.ConnectAsync(serverHost, port);
        
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };
        
            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new StreamReader(streamIn);

            //feedback.text = "Connected!";

            RestartExchange();
        }
        catch (Exception e)
        {
            feedback.text = "Error " + e.ToString();
        }
#endif
    }

    private void ConnectUnity(string host, string port)
    {
#if !UNITY_EDITOR
        errorStatus = "Unity TCP client used in UWP!";
#else
        try
        {
            if (exchangeThread != null) StopExchange();

            client = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            feedback.text = "Connected!";

            RestartExchange();
            
        }
        catch (Exception e)
        {
            feedback.text = "Error " + e.ToString();
        }
#endif
    }

    private bool exchanging = false;
    private bool exchangeStopRequested = false;
    private string lastPacket = null;

    private string errorStatus = null;
    private string warningStatus = null;
    private string successStatus = null;
    private string unknownStatus = null;

    public void RestartExchange()
    {
#if UNITY_EDITOR
        if (exchangeThread != null)
            StopExchange();

        exchangeStopRequested = false;
        exchangeThread = new System.Threading.Thread(ExchangePackets);
        exchangeThread.Start();
#else
        if (exchangeTask != null) 
            StopExchange();

        exchangeStopRequested = false;

        Debug.Log ("Vai chamar a task exchangepackets");
        print("print funciona?");



       /* exchangeTask = new Task(
            async() =>
            {
                ExchangePackets();
            }
            );
        exchangeTask.Start();*/





        //exchangeThread = new System.Threading.Thread(ExchangePackets);

        exchangeTask = Task.Run(() => ExchangePackets());
#endif
    }

    public void Update()
    {



        if (lastPacket != null)
        {
            feedback.text = "Server says: " + lastPacket;       //só escrever aqui no update

        }

      /*  if (errorStatus != null)
        {
            StatusTextManager.SetError(errorStatus);
            errorStatus = null;
        }
        if (warningStatus != null)
        {
            StatusTextManager.SetWarning(warningStatus);
            warningStatus = null;
        }
        if (successStatus != null)
        {
            StatusTextManager.SetSuccess(successStatus);
            successStatus = null;
        }
        if (unknownStatus != null)
        {
            StatusTextManager.SetUnknown(unknownStatus);
            unknownStatus = null;
        }*/
    }

    public void ExchangePackets()
    {
        //Debug.Log("vai entrar no while");

        while (!exchangeStopRequested)   
         {
           // Debug.Log("entrou no while");

            if (writer == null || reader == null) continue;
             exchanging = true;

             writer.Write("X\n");
             //Debug.Log( "Sent data!");
             string received = null;

         #if UNITY_EDITOR
             byte[] bytes = new byte[client.SendBufferSize];
             int recv = 0;
             while (true)
             {
                 recv = stream.Read(bytes, 0, client.SendBufferSize);
                 received += Encoding.UTF8.GetString(bytes, 0, recv);
                 if (received.EndsWith("\n")) break;
             }
         #else
             received = reader.ReadLine();
         #endif

             lastPacket = received;
            // Debug.Log("Received data: " + received);

             exchanging = false;
         }

   
    }





public void StopExchange()
{
 exchangeStopRequested = true;

#if UNITY_EDITOR
 if (exchangeThread != null)
 {
     exchangeThread.Abort();
     stream.Close();
     client.Close();
     writer.Close();
     reader.Close();

     stream = null;
     exchangeThread = null;
 }
#else
 if (exchangeTask != null) {
     exchangeTask.Wait();
     socket.Dispose();
     writer.Dispose();
     reader.Dispose();

     socket = null;
     exchangeTask = null;
 }
#endif
 writer = null;
 reader = null;
}

public void OnDestroy()
{
 StopExchange();
}

}
