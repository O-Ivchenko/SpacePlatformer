using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]

public class CameraController : MonoBehaviour
{
    private enum CameraState
    {
        ZoomIn,
        ZoomingIn,
        ZoomingOut,
        ZoomOut
    }
    public Transform player;
    public Vector3 cameraOffset;
    public float zAcceleration = -80f;
    public float speedLimit = 15f;
    public Vector2 playerBoundaryDistance = new Vector2(50, 50);
    public float cameraMoveDistance = 0.5f;

    private Transform _transform;
    private Camera mainCamera;
    private float zDecceleration = 30f;
    private Rigidbody2D shipRigid;
    private CameraState state = CameraState.ZoomIn;

    private void Awake()
    {
        _transform = transform;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
        _transform.position = player.position + cameraOffset;
        zDecceleration = cameraOffset.z;
        shipRigid = player.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        //CheckIfPlayerInFrame();
        _transform.position = player.position + new Vector3(cameraOffset.x, cameraOffset.y, _transform.position.z);

        if(InputController.Instance.KeyQ)
        {
            switch (state)
            {
                case CameraState.ZoomIn:
                    state = CameraState.ZoomingOut;
                    break;
                case CameraState.ZoomingIn:
                    state = CameraState.ZoomingOut;
                    break;
                case CameraState.ZoomingOut:
                    state = CameraState.ZoomingIn;
                    break;
                case CameraState.ZoomOut:
                    state = CameraState.ZoomingIn;
                    break;
            }
            print(state);
        }
        switch (state)
        {
            case CameraState.ZoomingIn:
                if (_transform.position.z < zDecceleration)
                    _transform.position += new Vector3(0, 0, 1);
                else
                    state = CameraState.ZoomIn;
                break;

            case CameraState.ZoomingOut:
                if (_transform.position.z > zAcceleration)
                    _transform.position += new Vector3(0, 0, -1);
                else
                    state = CameraState.ZoomOut;
                break;
        }
    }

    private void CheckIfPlayerInFrame()
    {
        Vector3 playerInCameraPosition = mainCamera.WorldToScreenPoint(player.position);
        if(playerInCameraPosition.x + playerBoundaryDistance.x > mainCamera.pixelWidth)
        {
            _transform.position += new Vector3(cameraMoveDistance, 0, 0);
        }
        if (playerInCameraPosition.x < playerBoundaryDistance.x)
        {
            _transform.position += new Vector3(-cameraMoveDistance, 0, 0);
        }
        if (playerInCameraPosition.y + playerBoundaryDistance.y > mainCamera.pixelHeight)
        {
            _transform.position += new Vector3(0, cameraMoveDistance, 0);
        }
        if (playerInCameraPosition.y < playerBoundaryDistance.y)
        {
            _transform.position += new Vector3(0, -cameraMoveDistance, 0);
        }
    }
}
