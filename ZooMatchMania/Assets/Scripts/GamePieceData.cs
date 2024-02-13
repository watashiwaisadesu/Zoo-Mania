using UnityEngine;


[System.Serializable]
public class GamePieceData
{
    public MatchedType MatchedType = MatchedType.None;
    public int score;
    [Header("Board variables")] 
    private int _column;
    private int _row;
    public int Column
    {
        get => _column;
        set => _column = value; 
    }
    public int Row
    {
        get => _row;
        set => _row = value;
    }
    internal int PreviousColumn;
    internal int PreviousRow;
    internal int TargetX;
    internal int TargetY;

   
    public float SwipeAngle { get; set; }
    public readonly float SwipeResist = 1f;
    public GamePiece otherGamePiece;
    private PieceType _type;
    public PieceType Type
    {
        get => _type;
        set => _type = value;
    }

    public void GetPosition(out int column,out int row)
    {
        column = Column;
        row = Row;
    }
}
