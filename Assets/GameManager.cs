using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform whereMustAdd;
    public GameObject lineElement;

    private int targetNumberMassage = 0;
    private string myName = "oops";

    private List<GameObject> created;

    // Start is called before the first frame update
    void Start()
    {
        created = new List<GameObject>();
//        StartCoroutine(updatIndex(2));
//        sendButton("hello guys");
    }

    public void makeNewRow(string text)
    {
        GameObject newRow = Instantiate(lineElement, whereMustAdd, true);
        Text textfield = newRow.GetComponent<Text>();
        textfield.text = text;
        created.Add(newRow);
    }

    IEnumerator GetAll(string name)
    {
        destroyAllMassages();
        myName = name;

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8005/all");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            string json = www.downloadHandler.text;
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;


            List<Massage> x = JsonConvert.DeserializeObject<List<Massage>>(json);

            if (x != null)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    Massage temp = x[i];
                    makeNewRow(String.Format("[{0}] : \n {1} ", temp.Owner, temp.Text));
                }

                targetNumberMassage = x.Count;
            }
        }

        StartCoroutine(updatIndex());
    }


    IEnumerator updatIndex()
    {
        string currentName = myName;
        while (myName == currentName)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:8005/update?index=" + targetNumberMassage);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;
                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;


                List<Massage> x = JsonConvert.DeserializeObject<List<Massage>>(json);

                if (x != null)
                {
                    int mine =  0;
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (x[i].Owner == myName)
                        {
                            mine++;
                            continue;
                        }
                        Massage temp = x[i];
                        makeNewRow(String.Format("[{0}] : \n {1} ", temp.Owner, temp.Text));
                    }

                    targetNumberMassage += x.Count - mine;
                }
            }
        }
    }

    public void getAllButton(InputField text)
    {
        StartCoroutine(GetAll(text.text));
    }


    public void sendButton(InputField str)
    {
        Massage newMassage = new Massage() {Owner = myName, Text = str.text};
        string sendToServer = JsonConvert.SerializeObject(newMassage);
        StartCoroutine(send(sendToServer));
    }

    IEnumerator send(string sendToServer)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:8005/send", sendToServer);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            string json = www.downloadHandler.text;
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;


            Massage x = JsonConvert.DeserializeObject<Massage>(json);

            if (x != null)
            {
                makeNewRow(String.Format("[{0}] : \n {1} ", x.Owner, x.Text));
                targetNumberMassage++;
            }
        }
    }


    private void destroyAllMassages()
    {
        for (int i = 0; i < created.Count; i++)
        {
            Destroy(created[i]);
        }
    }


    public class Massage
    {
        public string Owner;
        public string Text;
    }
}