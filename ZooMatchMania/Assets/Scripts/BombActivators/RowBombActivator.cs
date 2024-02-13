public class RowBombActivator : BombActivatorBase
{
    public override void Activate(int column = 0,int row = 0)
    {
        for (int j = 0; j < Board.Width; j++)
        {
            if (Board.AllPieces[j, row] == null || Board.BlankSpaces[j,row]) continue;
            Board.AllPieces[j, row].GamePieceData.MatchedType = MatchedType.Normal;
        }
    }
}