public class ColumnBombActivator : BombActivatorBase
{
    public override void Activate(int column=0,int row=0)
    {
        for (int i = 0; i < Board.Height; i++)
        {
            if (Board.AllPieces[column, i] == null|| Board.BlankSpaces[column,i]) continue;
            Board.AllPieces[column, i].GamePieceData.MatchedType = MatchedType.Normal;
        }
    }
}