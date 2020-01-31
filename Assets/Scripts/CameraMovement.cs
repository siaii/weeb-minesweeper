using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;
    [SerializeField] private float zoomSpeed=5f;

    private float zoomParameter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        zoomParameter += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime * 20f;
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(5, 2, zoomParameter);
    }
}
