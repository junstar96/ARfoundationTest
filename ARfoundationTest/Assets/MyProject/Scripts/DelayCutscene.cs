using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DelayCutscene : MonoBehaviour
{
    public ARSessionOrigin arsessionoriginl;
    public ARSession arsession;
    public ARCoreExtensions extension;
    // Start is called before the first frame update
    private void Awake()
    {
        arsessionoriginl.gameObject.SetActive(false);
        arsession.gameObject.SetActive(false);
        extension.gameObject.SetActive(false);
         

        StartCoroutine("Delaytime");
    }

    // Update is called once per frame
   
    public IEnumerator Delaytime()
    {
        yield return new WaitForSeconds(3);
        arsessionoriginl.gameObject.SetActive(true);
        arsession.gameObject.SetActive(true);
        extension.gameObject.SetActive(true);
    }
}
