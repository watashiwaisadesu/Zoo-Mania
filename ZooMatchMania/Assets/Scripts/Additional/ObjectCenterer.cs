using UnityEngine;

public class ObjectCenterer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Board _board;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    private void Start()
    {
        _board = FindObjectOfType<Board>();
        // Find the SpriteRenderer component on the object
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Call the method to center the sprite within the object
        if (spriteRenderer != null)
        {
            CenterSprite();
        }
    }

    private void CenterSprite()
    {
        // Calculate the center position within the object
        float centerX = _board.Width / 2 + xOffset;
        float centerY = _board.Width / 2 + yOffset;

        // Set the local position of the sprite to center it within the object
        spriteRenderer.transform.localPosition = new Vector3(centerX, centerY, 0);
    }
}