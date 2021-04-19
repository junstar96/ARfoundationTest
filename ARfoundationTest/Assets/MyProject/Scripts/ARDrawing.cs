using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARDrawing : MonoBehaviour
{
    public Transform _pivotPoint;

    public GameObject _lineRendererPrefeb;

    private LineRenderer _lineranderer;

    private List<LineRenderer> _lines = new List<LineRenderer>();

    public Transform _linepool;

    public bool  _use = false;

    public bool _isdrawing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_use)
        {
            if(_isdrawing)
            {
                DrawLineContinue();
            }
        }
    }

    public void MakeLineRenderer()
    {
        GameObject tLine = Instantiate(_lineRendererPrefeb);
        tLine.transform.SetParent(_linepool);
        tLine.transform.position = Vector3.zero;
        tLine.transform.localScale = new Vector3(1, 1, 1);

        _lineranderer = tLine.GetComponent<LineRenderer>();
        _lineranderer.positionCount = 1;
        _lineranderer.SetPosition(0, _pivotPoint.position);

        _isdrawing = true;
        _lines.Add(_lineranderer);
    }

    public void StartDrawLine()
    {
        _use = true;

        if(!_isdrawing)
        {
            MakeLineRenderer();
        }
    }

    public void StopDrawLine()
    {
        _isdrawing = false;
        _use = false;
        _lineranderer = null;
    }

    public void DrawLineContinue()
    {
        _lineranderer.positionCount = _lineranderer.positionCount + 1;
        _lineranderer.SetPosition(_lineranderer.positionCount - 1, _pivotPoint.position);
    }
}
