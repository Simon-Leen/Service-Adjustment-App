using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocationPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public RawImage map;
    public InputField mapNotes;
    public string apiKey;
    public float xCoord, yCoord;
    public int zoom, mapSizeX, mapSizeY;
    private string mapURL = "https://maps.googleapis.com/maps/api/staticmap?";


    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        StartCoroutine(GetStaticMap());
    }

    IEnumerator GetLocation()
    {
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            xCoord = Input.location.lastData.latitude;
            yCoord = Input.location.lastData.longitude;
        }

        Input.location.Stop();

        StartCoroutine(GetStaticMap());
    }

    IEnumerator GetStaticMap()
    {
        mapURL += $"center={xCoord},{yCoord}&zoom={zoom}&size={mapSizeX}x{mapSizeY}&key={apiKey}";

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(mapURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                map.texture = DownloadHandlerTexture.GetContent(request);
            }
        }
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(mapNotes.text))
        {
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
        }
        UIManager.Instance.activeCase.location = xCoord + "," + yCoord; 
        UIManager.Instance.takePhotoPanel.gameObject.SetActive(true);
    }
}
