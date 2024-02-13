
internal class ClearLinePiece : ClearablePiece
{
    public bool isRow;
    private BombActivatorBase _bombActivator;

    private void Start()
    {
        _bombActivator = isRow ? new RowBombActivator() : new ColumnBombActivator();
        _bombActivator.Initialize(Piece.Board);
    }

    public override void ActivatePieces()
    {
        base.ActivatePieces();
        _bombActivator.Activate( Piece.GamePieceData.Column , Piece.GamePieceData.Row);
    }
    
}