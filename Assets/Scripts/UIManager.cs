using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get 
        { 
            if (_instance == null) 
            { 
                Debug.LogError("The UIManager is NULL"); 
            } 
            
            return _instance; 
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Case activeCase;

    [Header("Panels")]
    public SearchPanel searchPanel;
    public SelectPanel selectPanel;
    public ClientInfoPanel clientInfoPanel;
    public LocationPanel locationPanel;
    public TakePhotoPanel takePhotoPanel;
    public OverviewPanel overviewPanel;
    public GameObject borderPanel;

    public void CreateCase()
    {
        activeCase = new Case();
        int randomCaseID = Random.Range(0, 1000);
        activeCase.caseID = "" + randomCaseID;

        clientInfoPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }

    public void FindCase()
    {
        searchPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }

    public void SubmitCase()
    {
        Case caseSave = new Case();
        caseSave.caseID = activeCase.caseID;
        caseSave.name = activeCase.name;
        caseSave.date = activeCase.date;
        caseSave.location = activeCase.location;
        caseSave.locationNotes = activeCase.locationNotes;
        caseSave.photoTaken = activeCase.photoTaken;
        caseSave.photoNotes = activeCase.photoNotes;

        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + $"/case#{caseSave.caseID}.dat";
        FileStream fs = File.Create(filePath);
        bf.Serialize(fs, caseSave);
        fs.Close();
        Debug.Log(Application.persistentDataPath);
        AWSManager.Instance.UploadToS3(filePath, caseSave.caseID);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
}
