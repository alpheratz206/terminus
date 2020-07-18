using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Editor Variables

    public Transform focus;
    public Vector3 offset = new Vector3(0f, 2.15f, -1.65f);
    public float pitch = 1.8f;

    public float zoom = 3f;
    public float minZoom = 1.5f;
    public float maxZoom = 6f;
    public float zoomSpeed = 2f;

    public float yaw = 0f;
    public float yawSpeed;

    #endregion

    private const string scrollWheelAxisName = "Mouse ScrollWheel";
    private const string horizontalAxisName = "Horizontal";

    private void Start()
    {
        PartyController.Instance.OnPlayerChange.Add(x => SwitchFocus(x.transform));
    }

    private void Update()
    {
        zoom -= Input.GetAxis(scrollWheelAxisName) * zoomSpeed;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        yaw -= Input.GetAxis(horizontalAxisName) * yawSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        transform.position = focus.position + (offset * zoom);
        transform.LookAt(focus.position + (Vector3.up * pitch));

        transform.RotateAround(focus.position, Vector3.up, yaw);
    }

    public void SwitchFocus(Transform newFocus)
    {
        Debug.Log($"Camera moves!");
    }
}
