public class ColorBombActivator : BombActivatorBase
{
    private readonly ColorType _colorType;

    public ColorBombActivator(ColorType colorType)
    {
        _colorType = colorType;
    }

    public override void Activate(int column=0, int row=0)
    {
        for (int i = 0; i < Board.Width; i++)
        {
            for (int j = 0; j < Board.Height; j++)
            {
                if (Board.AllPieces[i, j] != null && Board.AllPieces[i, j].ColorableComponent.Color == _colorType)
                {
                    Board.AllPieces[i, j].GamePieceData.MatchedType = MatchedType.Normal;
                }
            }
        }
    }
}