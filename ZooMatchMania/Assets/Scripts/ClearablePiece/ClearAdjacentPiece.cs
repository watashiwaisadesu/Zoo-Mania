
using System;

public class ClearAdjacentPiece : ClearablePiece
{
    private BombActivatorBase _bombActivator;

    private void Start()
    {
        _bombActivator = new AdjacentBombActivator();
        _bombActivator.Initialize(Piece.Board);
    }

    public override void ActivatePieces()
    {
        base.ActivatePieces();
        _bombActivator.Activate(Piece.GamePieceData.Column,Piece.GamePieceData.Row);
    }
}