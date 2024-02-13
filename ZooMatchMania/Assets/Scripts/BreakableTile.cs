using UnityEngine;

public class BreakableTile : NormalTile
{
    private void Start()
    {
        _hitPoints = 3;
        _tileKind = TileKind.Breakable;
        _sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (HitPoints<=0)
        {
            Destroy(this.gameObject);
        }
    }
    public int HitPoints
    {
        get => _hitPoints;
        private set => _hitPoints = value;
    }
    public void TakeDamage(int damage)
    {
        HitPoints -= damage;
        MakeLighter();
    }   
    private void MakeLighter()
    {
        Color color = _sprite.color;
        float newAlpha = color.a * .5f;
        _sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }

}
