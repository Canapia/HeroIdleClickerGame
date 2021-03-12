using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoomControl : MonoBehaviour
{
    private Camera m_Camera;
    private float zoomfactor = 3f;
    private float targetZoom;
    private float zoomLerpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        targetZoom = m_Camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomfactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 8f);
        m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
