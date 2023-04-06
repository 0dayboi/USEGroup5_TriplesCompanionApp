using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class SocketManager : MonoBehaviour
{
    public int socket_nr = 1;

    public Text socket_status_label;

    public double power;

    public double pricePerKwh = 0.4;

    public Text labelPower;

    public Text PowerUsage;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(updateDataSocket()); //Start updating corouting
        StartCoroutine(updateLabels());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator updateDataSocket()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);//Wait every 4 second before updating
            if (socket_status_label.text == "On")
            {
               
                UnityWebRequest uwr = UnityWebRequest.Get("http://3.121.124.133:3000/readpower_socket" + socket_nr.ToString());
                uwr.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();

                yield return uwr.SendWebRequest();
                string data = uwr.downloadHandler.text;
                data = data.Replace("\"", "");
                try
                {
                    double powerInWatts = double.Parse(data);
                    power = powerInWatts;
                }
                catch
                {

                }
            }
           

        }
    }


    IEnumerator updateLabels()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);//Wait every 4 second before updating

            if (socket_status_label.text == "On")
            {
                double pricePerHour = computeMoneyBeingSpent(power);
                labelPower.text = pricePerHour.ToString() + " €/h";
                PowerUsage.text = "Socket " + socket_nr.ToString() + " power usage: " + power.ToString() + " W";

            }else
            {
                labelPower.text = "0 €/h";
                PowerUsage.text = "Socket " + socket_nr.ToString() + " power usage: 0 W";

            }


        }
    }

    double computeMoneyBeingSpent(double power)
    {
        double price = pricePerKwh * (power/1000);
        return price;
    }


}
