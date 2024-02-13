using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardAnimation : MonoBehaviour
{
    private static Board _board;
    private readonly float _refillDelay = 0.5f;
    [SerializeField] private float fillTime=1;
    [SerializeField] private AudioClip spawnBombSound;
    
    public void Initialize(Board board)
    {
        _board = board;
    }
    public void DestroyMatches()
    {
        if (GameController.CurrentState==GameState.Wait)
        {
            return;
        }
        _board.HintManager.hintPieces.Clear();
        GameController.CurrentState=GameState.Wait;
        StartCoroutine(WaitDestroyMatchesAndRefillBoardCo());
    }

    private IEnumerator WaitDestroyMatchesAndRefillBoardCo()
    {
        yield return StartCoroutine(DestroyMatchAndRefillBoard());
    }
    private IEnumerator DestroyMatchAndRefillBoard()
    {
        yield return  StartCoroutine(DestroyMatchesCo());
        yield return new WaitForSeconds(_refillDelay);
        yield return StartCoroutine(DecreaseRowCo());
        yield return new WaitForSeconds(_refillDelay);
        yield return StartCoroutine(FillBoardCo());
        yield return StartCoroutine(CheckBoardCo());
    }
    
     private IEnumerator DestroyMatchesCo()
    {
        //make bomb if matches more than 3
        if (_board.findMatches.currentMatches.Count >= 4)
        {
            _board.BoardCheck.CheckToMakeBombs();
        }
        //make matchType Normal
        foreach (var piece in _board.AllPieces)  
        {
            if (piece != null && piece.IsClearable())
            {
                if (piece.GamePieceData.MatchedType==MatchedType.Special)
                {
                    yield return StartCoroutine(FindAndDestroyNearestPieces(piece.GamePieceData.Column, piece.GamePieceData.Row, MatchedType.Normal));
                }
                if (piece.GamePieceData.MatchedType == MatchedType.Normal)
                {
                    piece.ClearableComponent.ActivatePieces();
                }
            }
        }
        //find nearest pieces and destroy
        foreach (var piece in _board.AllPieces)  
        {
            if (piece != null && piece.IsClearable())
            {
                if (_board.CurrentGamePiece != null &&
                    piece.GamePieceData.MatchedType == MatchedType.Normal 
                    )
                {
                    if (piece.GamePieceData.MatchedType == MatchedType.Normal &&
                        piece == _board.CurrentGamePiece)
                    {
                        int column = piece.GamePieceData.Column;
                        int row = piece.GamePieceData.Row;
                        piece.ClearableComponent.ClearPiece();
                        yield return new WaitForSeconds(fillTime);
                        yield return StartCoroutine(FindAndDestroyNearestPieces(column, row, MatchedType.Normal));
                        break;
                    }

                    if (piece.GamePieceData.MatchedType == MatchedType.Normal &&
                        piece == _board.CurrentGamePiece.GamePieceData.otherGamePiece)
                    {
                        int columnOther = piece.GamePieceData.Column;
                        int rowOther = piece.GamePieceData.Row;
                        piece.ClearableComponent.ClearPiece();
                        yield return new WaitForSeconds(fillTime);
                        yield return StartCoroutine(FindAndDestroyNearestPieces(columnOther, rowOther,
                            MatchedType.Normal));
                        break;
                    }
                }

            }
        }
        if (_board.CurrentGamePiece==null)
        {
            foreach (var piece in _board.AllPieces)
            {
                if (piece != null && piece.IsClearable())
                {
                    if (piece.GamePieceData.MatchedType==MatchedType.Normal)
                    {
                        if (piece.GetComponent<ClearAdjacentPiece>()||
                            piece.GetComponent<ClearColorPiece>()||
                            piece.GetComponent<ClearLinePiece>())
                        {
                            yield return StartCoroutine(FindAndDestroyNearestPieces(piece.GamePieceData.Column, piece.GamePieceData.Row, MatchedType.Normal));
                            break;
                        }
                    }
                }
            }
            foreach (var piece in _board.AllPieces)
            {
                if (piece != null && piece.IsClearable())
                {
                    
                    if (piece.GamePieceData.MatchedType==MatchedType.Normal)
                    {
                        piece.ClearableComponent.ClearPiece();
                        yield return new WaitForSeconds(fillTime);
                    }
                }
            }
        }
        _board.HintManager.HintDelaySeconds = _board.HintManager.hintDelay;
        _board.findMatches.currentMatches.Clear();
        _board.CurrentGamePiece = null;
    }
     
     private IEnumerator DecreaseRowCo()
     {
         for (int i = 0; i < _board.Width; i++)
         {
             for (int j = 0; j < _board.Height; j++)
             {
                 if (!_board.BlankSpaces[i, j] && _board.AllPieces[i, j] == null)
                 {
                     for (int k = j + 1; k < _board.Height; k++)
                     {
                         if (_board.AllPieces[i, k] != null)
                         {
                             _board.AllPieces[i, k].GamePieceData.Row = j;
                             _board.AllPieces[i, k] = null;
                             yield return new WaitForSeconds(fillTime/2);
                             break;
                         }
                     }
                 }
             }
         }
     }
     
     private IEnumerator CheckBoardCo()
     {
         _board.findMatches.FindAllMatches();
         foreach (var piece in _board.AllPieces)  
         {
             if (piece != null && piece.IsClearable())
             {
                 if (piece.GamePieceData.MatchedType == MatchedType.Special)
                 {
                     piece.GamePieceData.MatchedType = MatchedType.None;
                 }
             }
         }
         // Continue looping until there are no more matches on the board
         if (_board.BoardCheck.MatchesOnBoard())
         {
             _board.streak++;
             GameController.CurrentState = GameState.Animating;
             StartCoroutine(DestroyMatchAndRefillBoard());
             yield break;
         }
         _board.streak=1;
         _board.findMatches.currentMatches.Clear();
         _board.CurrentGamePiece = null;
         if (_board.DeadLock.IsDeadLocked())
         {
             _board.DeadLock.ShuffleBoard();
             GameController.CurrentState=GameState.Move;
             yield break;
         }
         // Set the game state to Move
         GameController.CurrentState=GameState.Move;
     }
    
     public List<GamePiece> GetPiecesOfType(PieceType type)
     {
         var piecesOfType = new List<GamePiece>();

         for (int x = 0; x < _board.Width; x++)
         {
             for (int y = 0; y < _board.Height; y++)
             {
                 if (_board.AllPieces[x, y].GamePieceData.Type == type)
                 {
                     piecesOfType.Add(_board.AllPieces[x, y]);
                 }
             }
         }

         return piecesOfType;
     }
     
     
     private IEnumerator FindAndDestroyNearestPieces(int column, int row, MatchedType type)
     {
         List<Vector2Int> pos = new List<Vector2Int>();
         DestroyNearestPieces(column, row, type, pos);
         while (pos.Count > 0)
         {
             Vector2Int[] pieces = pos.ToArray();
             pos.Clear();
             foreach (var piecePos in pieces)
             {
                 yield return new WaitForSeconds(fillTime);
                 DestroyNearestPieces(piecePos.x, piecePos.y, MatchedType.Normal, pos);
             }
         }
     }

     private void DestroyNearestPieces(int column, int row, MatchedType type, List<Vector2Int> pos)
     {
         const int distance = 1;
         if (_board.AllPieces[column,row] != null&&_board.AllPieces[column,row].GamePieceData.MatchedType==type)
         {
             _board.AllPieces[column,row].ClearableComponent.ClearPiece();
         }
            
         for (int i = -distance; i <= distance; i++)
         {
             for (int j = -distance; j <= distance; j++)
             {
                 int targetRow = row + i;
                 int targetColumn = column + j;
                 if (IsValidPosition(targetColumn, targetRow) &&
                     _board.AllPieces[targetColumn, targetRow].GamePieceData.MatchedType == type &&
                     _board.AllPieces[targetColumn, targetRow] != null)
                 {
                     pos.Add(new Vector2Int(targetColumn,targetRow));
                     _board.AllPieces[targetColumn,targetRow].ClearableComponent.ClearPiece();
                 }
             }
         }
     }
     private bool IsValidPosition(int column, int row)
     {
         return column >= 0 && column < _board.Width && row >= 0 && row < _board.Height &&
                _board.AllPieces[column, row] != null && !_board.BlankSpaces[column, row];
     }

  
    private IEnumerator FillBoardCo()
    {
        Debug.Log("Refilling Board...");
        bool inverse = false;

        for (int j = 0; j < _board.Height; j++)
        {
            // Use the boolean variable to determine the order of iteration
            int startColumn = inverse ? _board.Width - 1 : 0;
            int endColumn = inverse ? -1 : _board.Width;
            int columnStep = inverse ? -1 : 1;

            for (int i = startColumn; i != endColumn; i += columnStep)
            {
                if (_board.AllPieces[i, j] == null && !_board.BlankSpaces[i, j])
                {
                    GamePiece gamePiece = SpawnNewPiece(i, j, PieceType.NormalClear, yOffset: _board.Offset);
                    yield return new WaitForSeconds(fillTime*2f);
                    gamePiece.GamePieceData.Row = j;
                    gamePiece.GamePieceData.Column = i;
                    gamePiece.name = $"Piece ( {i} , {j} )";
                }
            }
            // Toggle the inverse for the next iteration
            inverse = !inverse;
        }
    }

    internal GamePiece SpawnNewPiece(int column, int row, PieceType type, ColorType colorType = ColorType.Empty,MatchedType matchedType=MatchedType.None, int yOffset = 0)
    {
        _board.AllPieces[column, row]= Instantiate(_board.PiecePrefabDict[type], new Vector3(column, row + yOffset, 0), Quaternion.identity, this.transform.Find("Piece Container"));
        _board.AllPieces[column, row].Init(column, row, type,matchedType);
      
        if (colorType != ColorType.Empty) 
            _board.AllPieces[column, row].ColorableComponent.Color=colorType;
        else
        {
            if (type == PieceType.ColorClear)
                _board.AllPieces[column, row].ColorableComponent.Color=ColorType.Any;
            else
                _board.AllPieces[column, row].ColorableComponent.Color=((ColorType)Random.Range(0,_board.AllPieces[column, row].ColorableComponent.NumColors));
        }
        return _board.AllPieces[column, row];
    }
    
    public void MakeBomb(int column, int row, PieceType pieceType, ColorType colorType)
    {
        foreach (PieceType type in new[] { PieceType.AdjacentClear, PieceType.ColorClear, PieceType.RowClear, PieceType.ColumnClear })
        {
            if (pieceType == type)
            {
                AudioSource.PlayClipAtPoint(spawnBombSound, transform.position);
                SpawnNewPiece(column, row, type, colorType,MatchedType.Special);
                break;
            }
        }
    }

    private IEnumerator Setup()
    {
        yield return StartCoroutine(FillBoardCo());
        yield return StartCoroutine(CheckBoardCo());
    }
    public void FillBoard()
    {
        StartCoroutine(Setup());
    }

}



