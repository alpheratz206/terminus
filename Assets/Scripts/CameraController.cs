using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public float yawSpeed = 180f;

    public float lerpSpeed = 30f;

    #endregion

    private bool bAutoUpdateCamera = true;

    private const string scrollWheelAxisName = "Mouse ScrollWheel";
    private const string horizontalAxisName = "Mouse X";

    private void Start()
    {
        PartyController.Instance.OnPlayerChange.Add((prevChar, newChar) => SwitchFocus(newChar.transform));
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            zoom -= Input.GetAxis(scrollWheelAxisName) * zoomSpeed;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        if (Input.GetMouseButtonDown(1))
            StartCoroutine(ChangeYaw());
    }

    private IEnumerator ChangeYaw()
    {
        Cursor.visible = false;
        while (Input.GetMouseButton(1))
        {
            yaw -= Input.GetAxis(horizontalAxisName) * yawSpeed * Time.deltaTime;

            yield return null;
        }
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (!bAutoUpdateCamera)
            return;

        SetCamera(this.transform, this.focus);
    }

    private void SetCamera(Transform cam, Transform target)
    {
        cam.position = target.position + (offset * zoom);
        cam.LookAt(target.position + (Vector3.up * pitch));
        cam.RotateAround(target.position, Vector3.up, yaw);
    }

    Transform simulatedCamera;

    public void SwitchFocus(Transform newFocus)
    {
        StopAllCoroutines();

        if(simulatedCamera && simulatedCamera.gameObject)
            Destroy(simulatedCamera.gameObject);

        StartCoroutine(MoveCamera(newFocus));
    }

    private IEnumerator MoveCamera(Transform newFocus)
    {
        bAutoUpdateCamera = false;

        float t = 0f;
        var startingPos = transform.position;
        var startingRot = transform.rotation;

        var dist = Vector3.Distance(startingPos, newFocus.position + (offset * zoom));
        var lerpDuration = dist / lerpSpeed;

        simulatedCamera = new GameObject().transform;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * (1 / lerpDuration);

            SetCamera(simulatedCamera, newFocus);

            transform.position = Vector3.Lerp(startingPos, simulatedCamera.position, t);
            transform.rotation = Quaternion.Slerp(startingRot, simulatedCamera.rotation, t);

            yield return null;
        }

        Destroy(simulatedCamera.gameObject);

        focus = newFocus;

        bAutoUpdateCamera = true;
    }
}
