using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private PlayerController player;
    [SerializeField] private float cameraSizeNormal;
    [SerializeField] private float cameraSizeSneak;
    [SerializeField] private float zoomSpeed;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = cameraSizeNormal;
    }

    private void Update()
    {
        if (player.sneaking) cam.orthographicSize = Vector2.Lerp(
            new Vector2(cam.orthographicSize, 0),
            new Vector2(cameraSizeSneak, 0),
            Time.deltaTime * zoomSpeed
        ).x;
        else cam.orthographicSize = Vector2.Lerp(
            new Vector2(cam.orthographicSize, 0),
            new Vector2(cameraSizeNormal, 0),
            Time.deltaTime * zoomSpeed
        ).x;
    }
}
