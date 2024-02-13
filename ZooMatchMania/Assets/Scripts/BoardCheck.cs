using UnityEngine;


public class BoardCheck
{
    private static Board _board;
    public void Initialize(Board board)
    {
        _board = board;
    }

    internal bool MatchesAt(int column, int row, GamePiece gamePiece)
    {
        if (row > 1 && column > 1)
        {
            if ( _board.AllPieces[column - 2, row] != null&& _board.AllPieces[column-1,row]!=null && _board.AllPieces[column, row - 2] != null && _board.AllPieces[column, row - 1] != null) 
            {
                if ((_board.AllPieces[column - 2, row].ColorableComponent.Color==gamePiece.ColorableComponent.Color &&
                     _board.AllPieces[column - 1, row].ColorableComponent.Color==gamePiece.ColorableComponent.Color) ||
                    (_board.AllPieces[column, row - 2].ColorableComponent.Color==gamePiece.ColorableComponent.Color && _board.AllPieces[column, row - 1].ColorableComponent.Color==gamePiece.ColorableComponent.Color))
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (_board.AllPieces[column, row - 2] != null && _board.AllPieces[column, row - 1] != null)
                {
                    if (_board.AllPieces[column, row - 1].ColorableComponent.Color==gamePiece.ColorableComponent.Color &&
                        _board.AllPieces[column, row - 2].ColorableComponent.Color==gamePiece.ColorableComponent.Color)
                    {
                        return true;
                    }
                }
            }

            if (column > 1)
            {
                if (_board.AllPieces[column - 2, row] != null&& _board.AllPieces[column-1,row]!=null)
                {
                    if (_board.AllPieces[column - 1, row].ColorableComponent.Color==gamePiece.ColorableComponent.Color &&
                        _board.AllPieces[column - 2, row].ColorableComponent.Color==gamePiece.ColorableComponent.Color)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    internal bool MatchesOnBoard()
    {
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board.AllPieces[i, j] != null)
                {
                    if (_board.AllPieces[i, j].GamePieceData.MatchedType!=MatchedType.None)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool ColumnOrRow()
    {
        int numberVertical = 0;
        int numberHorizontal = 0;
        GamePiece firstGamePiece = _board.findMatches.currentMatches[0];
        if (firstGamePiece != null)
        {
            foreach (var i in _board.findMatches.currentMatches)
            {
                GamePiece gamePiece = i;
                if (gamePiece.GamePieceData.Row == firstGamePiece.GamePieceData.Row)
                {
                    numberHorizontal++;
                }

                if (gamePiece.GamePieceData.Column == firstGamePiece.GamePieceData.Column)
                {
                    numberVertical++;
                }
            }
        }
        return (numberHorizontal == 5 || numberVertical == 5);
    }

    internal void CheckToMakeBombs()
    {
        if (_board.findMatches.currentMatches.Count == 4 || _board.findMatches.currentMatches.Count == 7)
        {
            _board.findMatches.CheckToGenerateBomb.IsLineBomb();
        }
        else if (_board.findMatches.currentMatches.Count == 5 || _board.findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                _board.findMatches.CheckToGenerateBomb.IsColorBomb();
            }
            else
            {
                _board.findMatches.CheckToGenerateBomb.IsAdjacentBomb();
            }
        }
    }

}
