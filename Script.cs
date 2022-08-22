using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

public class Script : MonoBehaviour
{
    private Thread _thread;
    private UdpClient _client;
    private int _port = 9999;
    private Vector3 _translate = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _thread = new Thread(new ThreadStart(Receive));
        _thread.IsBackground = true;
        _thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
        float coe = 0.1f;
        
        transform.position += _translate * coe;

        // Key Press Event
        if (Input.GetKey(KeyCode.Keypad0))
        {
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private void Receive()
    {
        _client = new UdpClient(_port);
        while (true)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _client.Receive(ref ip);
                string data = Encoding.UTF8.GetString(buffer);
                String[] d = data.Split(",");

                _translate.x = float.Parse(d[0]);
                _translate.y = float.Parse(d[1]);
                _translate.z = float.Parse(d[2]);

            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    private void OnDestroy()
    {
        _client.Close();
        _thread.Abort();
    }
}
