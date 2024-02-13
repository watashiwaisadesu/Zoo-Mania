using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board _board;
    [SerializeField] private float cameraOffset;
    [SerializeField] private float padding = 2;
    [SerializeField] private float aspectRatio = 0.625f;
    [SerializeField] private float yOffset = -1f;

    private void Start()
    {
        _board = FindObjectOfType<Board>();
        if (_board != null)
        {
            RepositionCamera(_board.Width - 1, _board.Height - 1);
        }
    }

    private void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x / 2, y / 2+yOffset, cameraOffset);
        transform.position = tempPosition;
        if (Camera.main!=null)
        {
            if (_board.Width >= _board.Height)
            {
                 Camera.main.orthographicSize = (_board.Width / 2 + padding) / aspectRatio;
            }
            else
            {
                Camera.main.orthographicSize = (_board.Width / 2 + padding) / aspectRatio;
            }
        }
    }
}

