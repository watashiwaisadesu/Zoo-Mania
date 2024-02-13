using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDeadLock 
{
    private static Board _board;
    public void Initialize(Board board)
    {
        _board = board;
    }
    private bool SwitchPieces(int column, int row, Vector2 direction)
    {
        if (_board.AllPieces[column + (int)direction.x, row + (int)direction.y] !=null && _board.AllPieces[column, row] != null)
        {
            (_board.AllPieces[column + (int)direction.x, row + (int)direction.y], _board.AllPieces[column, row]) 
             = (_board.AllPieces[column, row], _board.AllPieces[column + (int)direction.x, row + (int)direction.y]);
            return true;
        }
        return false;
    }

    private bool CheckForMatches()
    {
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board.AllPieces[i, j] != null)
                {
                    if (j < _board.Height - 2)
                    {
                        if (_board.AllPieces[i, j + 1] != null &&
                            _board.AllPieces[i, j + 2] != null &&
                            !_board.BlankSpaces[i,j+1] &&
                            !_board.BlankSpaces[i,j+2])
                        {
                            if (_board.AllPieces[i, j + 1].ColorableComponent.Color==_board.AllPieces[i, j].ColorableComponent.Color &&
                                _board.AllPieces[i, j + 2].ColorableComponent.Color==_board.AllPieces[i, j].ColorableComponent.Color)
                            {
                                return true;
                            }
                        }
                    }

                    if (i < _board.Width - 2)
                    {
                        if (_board.AllPieces[i + 1, j] != null &&
                            _board.AllPieces[i + 2, j] != null &&
                            !_board.BlankSpaces[i+1,j]&&
                            !_board.BlankSpaces[i+2,j])
                        {
                            if (_board.AllPieces[i + 1, j].ColorableComponent.Color ==
                                _board.AllPieces[i, j].ColorableComponent.Color &&
                                _board.AllPieces[i + 2, j].ColorableComponent.Color ==
                                _board.AllPieces[i, j].ColorableComponent.Color)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        } 
        return false;
    }

    internal bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        if (SwitchPieces(column, row, direction))
        {
            if (CheckForMatches())
            {
                SwitchPieces(column, row, direction);
                return true;
            }
            SwitchPieces(column, row, direction);
        }
        return false;
    }

    internal bool IsDeadLocked()
    {
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board.AllPieces[i, j] != null)
                {
                    if (i < _board.Width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if (j < _board.Height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public void ShuffleBoard()
    {
        CoroutineHandler.Instance.StartCoroutine(ShuffleBoardCo());
    }
    
    private IEnumerator ShuffleBoardCo()
    {
        List<GamePiece> newBoard = new List<GamePiece>();
        // Populate newBoard with non-null dots
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board.AllPieces[i, j] != null)
                {
                    newBoard.Add(_board.AllPieces[i, j]);
                }
            }
        }
        ShuffleAndPlaceNewBoardContents(newBoard);
        _board.findMatches.FindAllMatches();
        yield return new WaitForSeconds(1f);
        
        while (_board.BoardCheck.MatchesOnBoard())
        {
            _board.BoardAnim.DestroyMatches();
            yield break;
        }
        if (IsDeadLocked())
        {
            CoroutineHandler.Instance.StartCoroutine(ShuffleBoardCo());
        }
    }

    private void ShuffleAndPlaceNewBoardContents(List<GamePiece> newBoard)
    {
        // Shuffle the board contents asynchronously
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (newBoard.Count <= 0)
                {
                    return;
                }
                if (!_board.BlankSpaces[i, j])
                {
                    int pieceToUse = Random.Range(0, newBoard.Count - 1);
                    int maxIteration = 0;

                    while (_board.BoardCheck.MatchesAt(i, j, newBoard[pieceToUse]) && maxIteration < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count - 1);
                        maxIteration++;
                        Debug.Log(maxIteration);
                    }

                    GamePiece gamePiece = newBoard[pieceToUse];
                    gamePiece.GamePieceData.Row = j;
                    gamePiece.GamePieceData.Column = i;
                    _board.AllPieces[i, j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }
    }

    
}
