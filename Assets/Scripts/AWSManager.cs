using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get 
        {
            if(_instance == null)
            {
                Debug.Log("AWSManager Instance is NULL");
            }
            return _instance;
        }
        set { }
    }

    private string _idPoolID = "eu-west-1:84169b0b-209e-4454-9f1e-69fed852f5a6";
    private string _bucketName = "gamedevhqprojects";
    private AmazonS3Client S3Client;

    private void Awake()
    {
        _instance = this;
        UnityInitializer.AttachToGameObject(this.gameObject);
        CognitoAWSCredentials creds = new CognitoAWSCredentials(_idPoolID, RegionEndpoint.EUWest1);
        S3Client = new AmazonS3Client(creds, RegionEndpoint.EUWest1);
    }

    public void UploadToS3(string path, string caseID)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        var request = new PostObjectRequest()
        {
            Bucket = _bucketName,
            Key = "case#"+caseID+".dat",
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = RegionEndpoint.EUWest1
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log( responseObj.Request.Key + ", " + responseObj.Request.Bucket);
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log(responseObj.Response.HttpStatusCode.ToString());
            }
        });
    }

    public void GetCaseList(string caseNumber, Action onComplete = null)
    {
        string targetCase = $"case#{caseNumber}.dat";

        var request = new ListObjectsRequest()
        {
            BucketName = _bucketName
        };

        S3Client.ListObjectsAsync(request, (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                bool caseFound = responseObject.Response.S3Objects.Any(o => o.Key == targetCase);
                
                if(caseFound)
                {
                    Debug.Log($"Searched case {caseNumber} found");
                    S3Client.GetObjectAsync(_bucketName, targetCase, (responseObj) =>
                    {
                        if(responseObj.Response.ResponseStream != null)
                        {
                            byte[] data = null;

                            using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    var buffer = new byte[512];
                                    var bytesRead = default(int);

                                    while((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        ms.Write(buffer, 0, bytesRead);
                                    }
                                    data = ms.ToArray();
                                }
                            }
                            using(MemoryStream ms = new MemoryStream(data))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                Case fetchedCase = (Case)bf.Deserialize(ms);
                                UIManager.Instance.activeCase = fetchedCase;
                                if(onComplete != null)
                                {
                                    onComplete();
                                }
                            }
                        }
                    });
                }
                else
                {
                    Debug.Log($"Searched case {caseNumber} not found");
                }
            }
            else
            {
                Debug.Log("Got Exception: " + responseObject.Exception);
            }
        });
    }
}
