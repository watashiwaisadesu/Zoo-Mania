using UnityEngine;

[System.Serializable]
public class BoardLayout
{
    [Range(0, 9)]
    public int xStart;
    [Range(0, 9)]
    public int yStart;
    public TileKind tileKind;
}

[System.Serializable]
public class BoardLayoutArea : BoardLayout
{
    [Range(0, 9)]
    public int xEnd;
    [Range(0, 9)]
    public int yEnd;
}