public class CheckToGenerateBomb
{
    private static Board _board;

    public void Initialize(Board board)
    {
        _board = board;
    }

    public void IsLineBomb()
    {
        if (_board.CurrentGamePiece != null)
        {
            var currentGamePiece = _board.CurrentGamePiece;
            int column = currentGamePiece.GamePieceData.Column;
            int row = currentGamePiece.GamePieceData.Row;
            float swipeAngle = _board.CurrentGamePiece.GamePieceData.SwipeAngle;

            if (_board.CurrentGamePiece != null)
            {
                if (_board.CurrentGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
                {
                    _board.CurrentGamePiece.GamePieceData.MatchedType =  MatchedType.None;
                    if ((swipeAngle > -45 && swipeAngle <= 45) 
                        || (swipeAngle > 135 && swipeAngle <= -135))
                    {
                        currentGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(column, row, PieceType.RowClear,_board.AllPieces[column,row].ColorableComponent.Color);
                    }
                    else
                    {
                        currentGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(column, row, PieceType.ColumnClear,_board.AllPieces[column,row].ColorableComponent.Color);
                    }
                }

                
                if (_board.CurrentGamePiece.GamePieceData.otherGamePiece != null && _board.CurrentGamePiece.GamePieceData.otherGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
                {
                    var otherGamePiece = _board.CurrentGamePiece.GamePieceData.otherGamePiece;
                    int columnOther = otherGamePiece.GamePieceData.Column;
                    int rowOther = otherGamePiece.GamePieceData.Row;
                    otherGamePiece.GamePieceData.MatchedType =  MatchedType.None;
                    if ((swipeAngle > -45 && swipeAngle <= 45) 
                        || (swipeAngle > 135 && swipeAngle <= -135))
                    {
                        otherGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(columnOther, rowOther, PieceType.RowClear,_board.AllPieces[columnOther,rowOther].ColorableComponent.Color);
                    }
                    else
                    {
                        otherGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(columnOther, rowOther, PieceType.ColumnClear,_board.AllPieces[columnOther,rowOther].ColorableComponent.Color);
                    }
                }
            }
        }
    }

    public void IsColorBomb()
    {
        if (_board.CurrentGamePiece != null)
        {
            var currentGamePiece = _board.CurrentGamePiece;
            int column = currentGamePiece.GamePieceData.Column;
            int row = currentGamePiece.GamePieceData.Row;

            var otherGamePiece = currentGamePiece.GamePieceData.otherGamePiece;
            int columnOther = otherGamePiece.GamePieceData.Column;
            int rowOther = otherGamePiece.GamePieceData.Row;
            if (currentGamePiece != null && currentGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
            {
                currentGamePiece.GamePieceData.MatchedType = MatchedType.None;
                if (currentGamePiece.GamePieceData.Type != PieceType.ColorClear)
                {
                    currentGamePiece.ClearableComponent.ClearNoAnimation();
                    _board.BoardAnim.MakeBomb(column, row, PieceType.ColorClear,_board.AllPieces[column,row].ColorableComponent.Color);
                }
            }

            if (otherGamePiece != null && otherGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
            {
                otherGamePiece.GamePieceData.MatchedType =  MatchedType.None;
                if (otherGamePiece.GamePieceData.Type != PieceType.ColorClear)
                {
                    otherGamePiece.ClearableComponent.ClearNoAnimation();
                    _board.BoardAnim.MakeBomb(columnOther, rowOther, PieceType.ColorClear,_board.AllPieces[columnOther,rowOther].ColorableComponent.Color);
                }
            }
        }
    }

    public void IsAdjacentBomb()
    {
        if (_board.CurrentGamePiece != null)
        {
            GamePiece currentGamePiece = _board.CurrentGamePiece;
            int column = currentGamePiece.GamePieceData.Column;
            int row = currentGamePiece.GamePieceData.Row;

            if (currentGamePiece != null)
            {
                if (currentGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
                {
                    currentGamePiece.GamePieceData.MatchedType = MatchedType.None;
                    if (currentGamePiece.GamePieceData.Type != PieceType.AdjacentClear)
                    {
                        currentGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(column, row, PieceType.AdjacentClear,_board.AllPieces[column,row].ColorableComponent.Color);
                    }
                }

                if (currentGamePiece.GamePieceData.otherGamePiece != null && currentGamePiece.GamePieceData.otherGamePiece.GamePieceData.MatchedType==MatchedType.Normal)
                {
                    GamePiece otherGamePiece = currentGamePiece.GamePieceData.otherGamePiece;
                    int columnOther = otherGamePiece.GamePieceData.Column;
                    int rowOther = otherGamePiece.GamePieceData.Row;
                    otherGamePiece.GamePieceData.MatchedType = MatchedType.None;
                    if (otherGamePiece.GamePieceData.Type != PieceType.AdjacentClear)
                    {
                        otherGamePiece.ClearableComponent.ClearNoAnimation();
                        _board.BoardAnim.MakeBomb(columnOther, rowOther, PieceType.AdjacentClear,_board.AllPieces[columnOther,rowOther].ColorableComponent.Color);
                    }
                }
            }
        }
    }

}
