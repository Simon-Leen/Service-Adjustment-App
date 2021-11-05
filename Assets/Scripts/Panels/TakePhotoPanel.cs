using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public RawImage photoTaken;
    public Texture2D noImg;
    public InputField photoNotes;
    public byte[] imgData;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
    }

    public void TakePhoto()
    {
        ProcessPhoto(512);
    }

    private void ProcessPhoto(int maxSize)
    {
        NativeCamera.Permission camPerm = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if(texture == null)
                {
                    Debug.Log("Can't load texture from " + path);
                    return;
                }
                imgData = texture.EncodeToPNG();
                photoTaken.texture = texture;
                photoTaken.gameObject.SetActive(true);
            }
        }, maxSize);
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(photoNotes.text))
        {
            UIManager.Instance.activeCase.photoNotes = photoNotes.text;
        }
        if(imgData != null && imgData.Length > 0)
        {
            UIManager.Instance.activeCase.photoTaken = imgData;
        }
        else
        {
            imgData = noImg.EncodeToPNG();
            UIManager.Instance.activeCase.photoTaken = imgData;
        }
        UIManager.Instance.overviewPanel.gameObject.SetActive(true);
    }
}