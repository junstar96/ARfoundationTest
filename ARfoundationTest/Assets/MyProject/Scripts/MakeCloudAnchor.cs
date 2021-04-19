using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARPointCloudManager))]
[RequireComponent(typeof(ARAnchorManager))]

public class MakeCloudAnchor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    //어떤 게 표시되도록 할 것인가
    public GameObject cloudanchorPrefeb;

    //어디에 만들어질 것인가.
    public Transform _anchorcontainer;

    private ARRaycastManager raycastManager;

    private ARPlaneManager planemanager;

    private ARPointCloudManager pointcloudmanager;

    private ARAnchorManager anchormanager;

    private ARAnchor anchor;

    private List<ARAnchor> anchorlist = new List<ARAnchor>();

    private static List<RaycastHit> raycasthits = new List<RaycastHit>();

    // Start is called before the first frame update
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        planemanager = GetComponent<ARPlaneManager>();

        pointcloudmanager = GetComponent<ARPointCloudManager>();

        anchormanager = GetComponent<ARAnchorManager>();

        var testinstnat = Instantiate(cloudanchorPrefeb, new Vector3(0, 0, 0), Quaternion.identity);

        testinstnat.transform.parent = _anchorcontainer;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        


    }

    public void OnPointerUp(PointerEventData eventData)
    {
     
    }


    
    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount <= 0)
        {
            return;
        }

        
        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
        {
            
            return;
        }

        Debug.Log("touch began");

        List<ARRaycastHit> raycasthits = new List<ARRaycastHit>();
        Debug.Log("before raycastcount :" + raycasthits.Count);
        raycastManager.Raycast(touch.position, raycasthits, TrackableType.PlaneWithinPolygon);
        Debug.Log("after raycastcount :" + raycasthits.Count);

        var planeType = PlaneAlignment.HorizontalUp;
        if (raycasthits.Count > 0)
        {
            ARPlane plane = planemanager.GetPlane(raycasthits[0].trackableId);
            if (plane == null)
            {
                Debug.LogWarningFormat("Failed to find the ARPlane with TrackableId {0}",
                    raycasthits[0].trackableId);
                return;
            }

            planeType = plane.alignment;
            var hitPose = raycasthits[0].pose;

            //anchor = anchormanager.GetAnchor();
            
            
            anchor = anchormanager.AddAnchor(hitPose);
        }


        if(anchor != null)
        {
            var hello_anchor = Instantiate(cloudanchorPrefeb, anchor.transform);
            Debug.Log("make instance : " + hello_anchor.name);
            //hello_anchor.transform.parent = _anchorcontainer;
        }

    }


}
