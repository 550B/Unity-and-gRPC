using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grpc.Core;
using Pos;
using System;
using TMPro;

public class Grpc_Client
{
    private Channel _channel;
    private Pos.PosServicer.PosServicerClient _client;
    static private Grpc_Client _instance;
    public bool _reply;
    public int _code;

    static public Grpc_Client Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Grpc_Client();
            return _instance;
        }
    }

    //// �������ӵķ����IP�͵�ַ
    public string ServerIpPort { get; set; } = "127.0.0.1:5173";

    // ͨ��
    private Channel channel
    {
        get
        {
            if (_channel == null)
            {
                //���ӷ���˵�ͨѶ��ϢIP�˿ڡ���������
                _channel = new Channel(ServerIpPort, ChannelCredentials.Insecure);
            }
            return _channel;
        }
    }

    // ����״̬
    public ChannelState GetConnectState()
    {
        return channel.State;
    }

    // �ͻ���
    private Pos.PosServicer.PosServicerClient Client
    {
        get
        {
            if (_client == null)
            {
                //��ʼ���ͻ���
                _client = new Pos.PosServicer.PosServicerClient(channel);
            }
            return _client;
        }
    }

    // ִ�е���ָ��
    public bool EasyCall(EasyCallRequest request)
    {
        try
        {
            Debug.Log("����״̬" + GetConnectState());
            //���ýӿ�
            var reply = Client.EasyCall(request);
            Debug.Log("����״̬" + GetConnectState());
            if (reply != null)
            {
                _code = reply.Code;
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
        return false;
    }

    public bool PosCall(PosRequest request)
    {
        try
        {
            Debug.Log("����״̬" + GetConnectState());
            //���ýӿ�
            var reply = Client.PosCall(request);
            Debug.Log("����״̬" + GetConnectState());
            if (reply != null)
            {
                _reply = reply.Inside;
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.StackTrace);
        }
        return false;
    }

    ~Grpc_Client()
    {
        _channel.ShutdownAsync().Wait();
    }
}

public class makeRPC : MonoBehaviour
{
    public Grpc_Client client;
    float interval = 0;
    int cnt = 0;
    public TextMeshProUGUI textbox;
    private bool isBegin = false;

    // Start is called before the first frame update
    void Start()
    {
        client = new Grpc_Client();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBegin)
        {
            interval += Time.deltaTime;
            if (interval > 1f)
            {
                Pos.PosRequest request = new PosRequest();
                request.X = transform.position.x;
                request.Y = transform.position.y;
                request.Z = transform.position.z;
                bool reply = client.PosCall(request);

                string msg;
                if (reply)
                {
                    msg = "[" + (++cnt).ToString() + "]second(s)' reply is: " + client._reply.ToString();
                }
                else
                {
                    msg = "[" + (++cnt).ToString() + "]second(s)' reply is: " + client._reply.ToString();
                }
                textbox.text = msg;

                interval = 0;
            }
        }
    }

    public void localButtonClick()
    {
        EasyCallRequest request = new EasyCallRequest();
        request.Code = 0;
        bool reply = client.EasyCall(request);
        textbox.text = "Local connection " + (reply ? "succeeded! " : "failed! ") + "The return code is: " + client._code.ToString();
        client.ServerIpPort = "127.0.0.1:5173";
        isBegin = reply;
    }

    public void remoteButtonClick()
    {
        EasyCallRequest request = new EasyCallRequest();
        request.Code = 0;
        bool reply = client.EasyCall(request);
        textbox.text = "Remote connection " + (reply ? "succeeded! " : "failed! ") + "The return code is: " + client._code.ToString();
        client.ServerIpPort = "47.120.41.97:2333";
        isBegin = reply;
    }
}
