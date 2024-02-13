using UnityEngine;

public class NormalTile : MonoBehaviour
{
    protected SpriteRenderer _sprite;
    protected int _hitPoints;
    protected TileKind _tileKind;
    private void Start()
    {
        _tileKind = TileKind.Normal;
    }
}
