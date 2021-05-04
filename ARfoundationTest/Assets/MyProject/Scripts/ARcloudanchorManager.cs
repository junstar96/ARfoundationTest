using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using DilmerGames.Core.Singletons;
using UnityEngine.UI;
public class createAnchorEvents : UnityEvent<Transform>
{ 
}

public class ARcloudanchorManager : Singleton<ARcloudanchorManager>
{
    [SerializeField]
    private Camera arcamera;

    [SerializeField]
    private float resolveAnchorPassTimeout = 10.0f;

    private ARAnchorManager anchormanager = null;

    private ARAnchor pendingHostAnchor = null;

    private ARCloudAnchor cloudanchor = null;

    private string anchorToResolve = string.Empty;

    private bool anchorHostInProgress = false;

    private bool anchorResolveInProgress = false;

    private float safeToResolvePassed = 0;

    private createAnchorEvents createAnchorEvents = null;

    public Text statustext;

    public Text debugtext;

    
    private void Awake()
    {
        anchormanager = GetComponent<ARAnchorManager>();


        createAnchorEvents = new createAnchorEvents();
        createAnchorEvents.AddListener((t) => ARPlacementManager.Instance.ReCreatePlacement(t));
    }

    private Pose GetCameraPose()
    {
        return new Pose(arcamera.transform.position, arcamera.transform.rotation);
    }


    //// Start is called before the first frame update
    //void Start()
    //{
        

    //   // createAnchorEvents.AddListener((t) => ARPLan)
    //}

    public void QueueAnchor(ARAnchor anchor)
    {
        pendingHostAnchor = anchor;
    }

    public void HostAnchor()
    {
        debugtext.text = "HostAnchor executing";


        FeatureMapQuality quality = anchormanager.EstimateFeatureMapQualityForHosting(GetCameraPose());

        cloudanchor = anchormanager.HostCloudAnchor(pendingHostAnchor, 1);
        
        if(cloudanchor == null)
        {
            debugtext.text = "Unable to host cloud anchor";
        }
        else
        {
            anchorHostInProgress = true;
        }
    }

    public void Resolve()
    {
        debugtext.text = "Resolve executing";

        cloudanchor = anchormanager.ResolveCloudAnchorId(anchorToResolve);

        if (cloudanchor == null)
        {
            debugtext.text = "Failed to resolve cloud achor id " + cloudanchor.cloudAnchorId;
    }
        else
        {
            anchorHostInProgress = true;
        }
    }

    private void CheckHostingProgress()
    {
        CloudAnchorState cloudanchorstate = cloudanchor.cloudAnchorState;

        

        if(cloudanchorstate == CloudAnchorState.Success)
        {
            anchorHostInProgress = false;

            anchorToResolve = cloudanchor.cloudAnchorId;
        }
        else if(cloudanchorstate != CloudAnchorState.TaskInProgress)
        {
            anchorHostInProgress = false;
            debugtext.text = "state : CloudAnchorState.TaskInProgress";
        }
    }

    private void CheckResolveProgress()
    {
        CloudAnchorState cloudAnchorState = cloudanchor.cloudAnchorState;
        if (cloudAnchorState == CloudAnchorState.Success)
        {
            debugtext.text = "Anchor successfully hosted";
            anchorHostInProgress = false;

            anchorToResolve = cloudanchor.cloudAnchorId;
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            anchorHostInProgress = false;
            debugtext.text = "Fail to host anchor with state";
           
        }
    }


    // Update is called once per frame
    void Update()
    {



        //checking for host result
        if (anchorHostInProgress)
        {
            CheckHostingProgress();
            return;
        }


        //checking for resolve result
        if(anchorResolveInProgress && safeToResolvePassed <= 0)
        {
            safeToResolvePassed = resolveAnchorPassTimeout;

            if (!string.IsNullOrEmpty(anchorToResolve))
            {
                CheckResolveProgress();
            }

          
            return;
        }
        else
        {
            safeToResolvePassed -= Time.deltaTime * 1.0f;
        }

       
    }
}
