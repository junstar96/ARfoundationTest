using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onpress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public ARDrawing ardrawing;
    public bool _pressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        ardrawing.StopDrawLine();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_pressed)
        {
            ardrawing.StartDrawLine();
        }
    }
}
