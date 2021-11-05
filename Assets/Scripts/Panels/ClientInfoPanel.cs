using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public InputField firstName, lastName;
    private string _date;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        if(string.IsNullOrEmpty(firstName.text) || string.IsNullOrEmpty(lastName.text))
        {
            Debug.Log("First or Last name is empty!");
        }
        else
        {
            DateTime today = DateTime.Today;
            _date = today.ToString("MMMM dd, yyyy");

            UIManager.Instance.activeCase.name = firstName.text + " " + lastName.text;
            UIManager.Instance.activeCase.date = _date ;
            UIManager.Instance.locationPanel.gameObject.SetActive(true);
        }
    }
}
