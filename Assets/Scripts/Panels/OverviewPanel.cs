using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText, nameTitle, dateTitle, locationTitle, locationNotes, photoTitle, photoNotes;
    public RawImage photoTaken;
    public Button saveButton, backButton;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        nameTitle.text = "NAME: " + UIManager.Instance.activeCase.name;
        dateTitle.text = "DATE: " + UIManager.Instance.activeCase.date;
        locationTitle.text = "LOCATION: \n" + UIManager.Instance.activeCase.location;
        locationNotes.text = "LOCATION NOTES: \n" + UIManager.Instance.activeCase.locationNotes;
        photoTitle.text = "PHOTO:";
        Texture2D tex2d = new Texture2D(1, 1);
        tex2d.LoadImage(UIManager.Instance.activeCase.photoTaken);
        photoTaken.texture = tex2d;
        photoNotes.text = "PHOTO NOTES: \n" + UIManager.Instance.activeCase.photoNotes;
    }

    public void ProcessInfo()
    {
        
    }
}
