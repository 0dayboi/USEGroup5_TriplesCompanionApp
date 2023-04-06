using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SocketOnOff : MonoBehaviour
{
    bool socket1_status = false;
    bool socket2_status = false;
    bool socket3_status = false;
    bool done = false;
    string contentDownloaded1 = "";

    public Image box1;
    public Image box2;
    public Image box3;
    public Text label1;
    public Text label2;
    public Text label3;
    public Sprite onimage;
    public Sprite offimage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GettingStats());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator GettingStats()
    {
        StartCoroutine(getRequest("http://3.121.124.133:3000/getupdate_socket1"));
        yield return new WaitUntil(() => done);
        string s1 = contentDownloaded1;
        StartCoroutine(getRequest("http://3.121.124.133:3000/getupdate_socket2"));
        yield return new WaitUntil(() => done);
        string s2 = contentDownloaded1;
        StartCoroutine(getRequest("http://3.121.124.133:3000/getupdate_socket3"));
        yield return new WaitUntil(() => done);

        string s3 = contentDownloaded1;

        if (s1 == "\"1\"")
        {
            //  box1.material.mainTexture = onimage;
            label1.text = "On";
            socket1_status = true;
            box1.sprite =  onimage;
        }
        else if (s1 == "\"0\"")
        {
            //  box1.material.mainTexture = offimage;
            label1.text = "Off";
            socket1_status = false;
            box1.sprite = offimage;
        }

        if (s2 == "\"1\"")
        {
            // box2.material.mainTexture = onimage;
            label2.text = "On";
            socket2_status = true;
            box2.sprite = onimage;

        }
        else if (s2 == "\"0\"")
        {
            //box2.material.mainTexture = offimage;
            label2.text = "Off";
            socket2_status = false;
            box2.sprite = offimage;

        }

        if (s3 == "\"1\"")
        {//
         // box3.material.mainTexture = onimage;
            label3.text = "On";
            socket3_status = true;
            box3.sprite = onimage;

        }
        else if (s3 == "\"0\"")
        {
            // box3.material.mainTexture = offimage;
            label3.text = "Off";
            socket3_status = false;
            box3.sprite = offimage;

        }


    }


    public void box1_clicked()
    {
        StartCoroutine(box1_numr());
    }

    IEnumerator box1_numr()
    {
        if (socket1_status)
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket1?status=0"));
            yield return new WaitUntil(() => done);

            socket1_status = false;
            label1.text = "Off";
            box1.sprite = offimage;

        }
        else
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket1?status=1"));
            yield return new WaitUntil(() => done);
            socket1_status = true;
            label1.text = "On";
            box1.sprite= onimage;  
        }
    }

    IEnumerator box2_numr()
    {
        if (socket2_status)
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket2?status=0"));
            yield return new WaitUntil(() => done);

            socket2_status = false;
            label2.text = "Off";
            box2.sprite = offimage;
        }
        else
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket2?status=1"));
            yield return new WaitUntil(() => done);
            socket2_status = true;
            label2.text = "On";
            box2.sprite = onimage;
        }
    }

    IEnumerator box3_numr()
    {
        if (socket3_status)
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket3?status=0"));
            yield return new WaitUntil(() => done);

            socket3_status = false;
            label3.text = "Off";
            box3.sprite = offimage; 
        }
        else
        {
            StartCoroutine(getRequest("http://3.121.124.133:3000/update_socket3?status=1"));
            yield return new WaitUntil(() => done);
            socket3_status = true;
            label3.text = "On";
            box3.sprite = onimage;
        }
    }

    public void box2_clicked()
    {
        StartCoroutine(box2_numr());
    }

    public void box3_clicked()
    {
        StartCoroutine(box3_numr());
    }


    IEnumerator getRequest(string uri)
    {
        done = false; 
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        uwr.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        yield return  uwr.SendWebRequest();
       
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            contentDownloaded1 = uwr.error;

        }
        else
        {
            contentDownloaded1 = uwr.downloadHandler.text;
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
        done = true;
    }
}
