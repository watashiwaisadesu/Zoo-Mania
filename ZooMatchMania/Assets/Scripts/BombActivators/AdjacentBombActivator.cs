public class AdjacentBombActivator : BombActivatorBase
{
    public override void Activate(int column, int row)
    {
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                if (i >= 0 && i < Board.Width && j >= 0 && j < Board.Height)
                {
                    if (Board.AllPieces[i, j] != null)
                    {
                        Board.AllPieces[i, j].GamePieceData.MatchedType = MatchedType.Normal;
                    }
                }
            }
        }
    }
}