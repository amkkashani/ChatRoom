using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform whereMustAdd;
    public GameObject lineElement;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeNewRow(string text)
    {
        GameObject newRow = Instantiate(lineElement, whereMustAdd, true);
        Text textfield = newRow.GetComponent<Text>();
        textfield.text = text;
    }
    
    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8005/see");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }


}
