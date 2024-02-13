using System;
using UnityEngine;

public class ClearColorPiece : ClearablePiece
{
    [field:SerializeField]public ColorType Color { get; set; }
    private BombActivatorBase _bombActivator;

    private void Start()
    {
        _bombActivator = new ColorBombActivator(Color);
        _bombActivator.Initialize(Piece.Board);
    }

    public override void ActivatePieces()
    {
        _bombActivator.Activate();
        base.ActivatePieces();
    }
}
