using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using Google.XR.ARCoreExtensions.Samples.PersistentCloudAnchors;

public class MakeCloudAnchor : MonoBehaviour
{

    public enum MakeState
    {
        Host,
        Resolve,
        none
    }

    public MakeState checkstate = MakeState.none;

    [Header("ARfoundation")]
    public ARSessionOrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public string cloudid = string.Empty;

    [Space(4)]
    [Header("Prefeb")]
    public GameObject anchorprefeb;
    public UnityEngine.UI.Text checktext;
    public GameObject MappingPrefeb;


    private ARAnchor anchor;
    private List<ARCloudAnchor> pendingcloudanchor = new List<ARCloudAnchor>();
    private bool check = false;
    private MapQualityIndicator _qualityIndicator = null;


    private int count = 0;
    
    


    private ARCloudAnchor cloudanchor = null;


    // Start is called before the first frame update
    private void Start()
    {
        //Debug.Log("Application.persistentDataPath :" + Application.persistentDataPath);
        //Debug.Log("Application.dataPath" + Application.dataPath);
        //Debug.Log("Application.streamingAssetsPath"+Application.streamingAssetsPath);
        //fileinfo = new FileInfo(Application.dataPath + "//" + "test.txt");
    }

    public void MakeFile()
    {

        var filepath = Application.persistentDataPath + "/hello_world.txt";
        if(!File.Exists(filepath))
        {
            Debug.Log("now make");
            using(File.Create(filepath))
            {
                
                
            }
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                Debug.Log(cloudid);
                sw.WriteLine(cloudid);
                sw.Close();
            }
        }
        else
        {
            Debug.Log("already make");
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                Debug.Log(cloudid);
                sw.WriteLine(cloudid);
                sw.Close();
            }
        }
        

        

    }

    public void ReadFile()
    {
        var filepath = Application.persistentDataPath + "/hello_world.txt";
        string line = string.Empty;
        using(StreamReader sr = new StreamReader(filepath))
        {
            while((line = sr.ReadLine()) != null)
            {
                cloudid = line;
                
            }
        }
    }


    private void Update()
    {
     
        if (!sessionOrigin.gameObject.activeSelf)
        {
            return;
        }


        if (ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }

       



        if (check)
        {
            return;
        }

        if (pendingcloudanchor.Count == 0 && !check)
        {
            switch(checkstate)
            {
                case MakeState.Host:
                    if (anchor == null)
                    {
                        // If the player has not touched the screen then the update is complete.
                        Touch touch;
                        if (Input.touchCount < 1 ||
                            (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                        {
                            return;
                        }
                        var planeType = PlaneAlignment.HorizontalUp;
                        // Ignore the touch if it's pointing on UI objects.
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            return;
                        }

                        List<ARRaycastHit> hits = new List<ARRaycastHit>();

                        raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);


                        var hitpose = hits[0].pose;

                        anchor = anchorManager.AddAnchor(hitpose);

                        if(anchor != null)
                        {
                            Instantiate(anchorprefeb, anchor.transform);

                            // Attach map quality indicator to this anchor.
                            var indicatorGO =
                                Instantiate(MappingPrefeb, anchor.transform);
                            _qualityIndicator = indicatorGO.GetComponent<MapQualityIndicator>();
                            _qualityIndicator.DrawIndicator(planeType, sessionOrigin.camera);
                            

 
                        }
                        


                    }
                    StartCoroutine(ParasiteCloud());
                    break;
                case MakeState.Resolve:
                    StartCoroutine(ResolveAnchor());
                    break;
                case MakeState.none:
                    break;

            }
            
            
           
        }

    }

    public IEnumerator ResolveAnchor()
    {
        
        check = true;
        ReadFile();
        cloudanchor = anchorManager.ResolveCloudAnchorId(cloudid);
       
        
        if (cloudanchor != null)
        {
            
            pendingcloudanchor.Add(cloudanchor);
            while(true)
            {
                
                if(cloudanchor.cloudAnchorState == CloudAnchorState.Success)
                {
                    checktext.text = "success";
                    Instantiate(anchorprefeb, cloudanchor.transform);
                    
                    break;
                }
                checktext.text = cloudanchor.cloudAnchorState.ToString();
                yield return null;
            }

        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            check = false;
            count++;
            checktext.text = "fail" + count;
        }
    }

    public IEnumerator ParasiteCloud()
    {
        check = true;
       
        yield return new WaitForSeconds(5);

        cloudanchor = anchorManager.HostCloudAnchor(anchor);


        if (cloudanchor != null)
        {
            Instantiate(anchorprefeb, cloudanchor.transform);
            StartCoroutine(CloudState());
            pendingcloudanchor.Add(cloudanchor);
        }
        else
        {
            checktext.text = "실패 : " + count;
        }


        
        check = false;
    }

    
    public IEnumerator CloudState()
    {
        while(true)
        {
            checktext.text = cloudanchor.cloudAnchorState.ToString();
            count++;
            if(cloudanchor.cloudAnchorState == CloudAnchorState.Success)
            {
                
                checktext.text = "cloud id : " + cloudanchor.cloudAnchorId + " \n" + "count : " + count;
                cloudid = cloudanchor.cloudAnchorId;
                //Debug.Log("cloud id : " + cloudanchor.cloudAnchorId);
                MakeFile();
                break;
            }
            yield return null;
        }

        
    }

    public void HostButtonClick()
    {
        checkstate = MakeState.Host;
    }

    public void ResolveButtonClick()
    {
        checkstate = MakeState.Resolve;
    }


   
}
