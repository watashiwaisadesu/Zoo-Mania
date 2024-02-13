using System.Collections;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private Hud _hud;
    private GamePiece _gamePiece;
    private Board _board;
    private Vector2 _firstTouchPos;
    private Vector2 _finalTouchPos;
    private Vector2 _tempPosition;
   

    private void Awake()
    {
        _gamePiece = GetComponent<GamePiece>();
        _board = FindObjectOfType<Board>();
        _hud = FindObjectOfType<Hud>();
    }

    private void OnMouseDown()
    {
        _gamePiece.Hintmanager.DestroyHint();
        _board.CurrentGamePiece = _gamePiece;
        if (Camera.main != null && GameController.CurrentState == GameState.Move)
        {
            _firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    
    private void OnMouseUp()
    {
        if (Camera.main != null && GameController.CurrentState == GameState.Move)
        {
            _finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateSwipeAngle();
        }
    }
    
    private void CalculateSwipeAngle()
    {
        float swipeResist = _gamePiece.GamePieceData.SwipeResist;
        if (_gamePiece != null &&
            (Mathf.Abs(_finalTouchPos.x - _firstTouchPos.x) > swipeResist ||
             Mathf.Abs(_finalTouchPos.y - _firstTouchPos.y) > swipeResist))
        {
            Vector2 swipeDirection = _finalTouchPos - _firstTouchPos;
            GameController.CurrentState=GameState.Animating;
            _gamePiece.GamePieceData.SwipeAngle = Mathf.Atan2(swipeDirection.y, swipeDirection.x) * Mathf.Rad2Deg;
            MovePieces();
        }
        else
        {
            GameController.CurrentState=GameState.Move;
        }
    }
    
    public void UpdatePosition()
    {
        _gamePiece.GamePieceData.TargetX = _gamePiece.GamePieceData.Column;
        _gamePiece.GamePieceData.TargetY = _gamePiece.GamePieceData.Row;
        Vector2 targetPosition = new Vector2(_gamePiece.GamePieceData.TargetX, _gamePiece.GamePieceData.TargetY);
        if (Vector2.Distance(_gamePiece.transform.position, targetPosition) > 0.1f )
        {
            _gamePiece.transform.position = Vector2.Lerp(_gamePiece.transform.position, targetPosition, 0.7f);
            _board.AllPieces[_gamePiece.GamePieceData.Column, _gamePiece.GamePieceData.Row] = _gamePiece;
            if ( GameController.CurrentState!=GameState.Wait)
            {
                _gamePiece.FindMatches.FindAllMatches();
            }
        }
        else
        {
            _gamePiece.transform.position = targetPosition; // Snap to the target position
        }
    }

    private void MovePieces()
    {
        if (_board.IsGameOver) return;
        int newColumn = 0;
        int newRow = 0;
        float swipeAngle = _gamePiece.GamePieceData.SwipeAngle;
    
        if (swipeAngle > -45 && swipeAngle <= 45 && _gamePiece.GamePieceData.Column < _board.Width - 1) newColumn = 1;
   
        else if (swipeAngle > 45 && swipeAngle <= 135 && _gamePiece.GamePieceData.Row < _board.Height - 1) newRow = 1;
 
        else if (swipeAngle > 135 || swipeAngle <= -135 && _gamePiece.GamePieceData.Column > 0)  newColumn = -1;
   
        else if (swipeAngle < -45 && swipeAngle >= -135 && _gamePiece.GamePieceData.Row > 0)  newRow = -1;

        if (newColumn != 0 || newRow != 0)
        {
            AudioSource.PlayClipAtPoint(_gamePiece.moveSound, transform.position);
            int newColumnIndex = _gamePiece.GamePieceData.Column + newColumn;
            int newRowIndex = _gamePiece.GamePieceData.Row + newRow;
            if (_board.AllPieces[newColumnIndex, newRowIndex]!=null&&_board.AllPieces[newColumnIndex, newRowIndex].IsMovable())
            {
                _gamePiece.GamePieceData.otherGamePiece = _board.AllPieces[newColumnIndex, newRowIndex];
                _board.AllPieces[newColumnIndex, newRowIndex] = _gamePiece;
                _board.AllPieces[_gamePiece.GamePieceData.Column, _gamePiece.GamePieceData.Row] = _gamePiece.GamePieceData.otherGamePiece;
                _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Column = _gamePiece.GamePieceData.Column;
                _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Row = _gamePiece.GamePieceData.Row;
                _gamePiece.GamePieceData.Column = newColumnIndex;
                _gamePiece.GamePieceData.Row = newRowIndex;
                _gamePiece.StartCoroutine(CheckMoveCo());
            }  
            else
            {
                GameController.CurrentState=GameState.Move;
            }
        }
        _hud.level.OnMove();
    }
    private IEnumerator CheckMoveCo()
    {
        if (_gamePiece.IsColored()&&_gamePiece.GamePieceData.otherGamePiece.IsColored())
        {
            if (_gamePiece.GamePieceData.Type==PieceType.ColorClear)
            {
                _gamePiece.GetComponent<ClearColorPiece>().Color =
                    _gamePiece.GamePieceData.otherGamePiece.ColorableComponent.Color;
                _gamePiece.ClearableComponent.ActivatePieces();
                _gamePiece.ClearableComponent.ClearPiece();
                if (_board.BoardCheck.MatchesOnBoard())
                {
                    _board.BoardAnim.DestroyMatches();
                }
            }
            else if (_gamePiece.GamePieceData.otherGamePiece.GamePieceData.Type==PieceType.ColorClear)
            {
                _gamePiece.GamePieceData.otherGamePiece.GetComponent<ClearColorPiece>().Color =
                    _gamePiece.ColorableComponent.Color;
                _gamePiece.GamePieceData.otherGamePiece.ClearableComponent.ActivatePieces();
                _gamePiece.GamePieceData.otherGamePiece.ClearableComponent.ClearPiece();
                if (_board.BoardCheck.MatchesOnBoard())
                {
                    _board.BoardAnim.DestroyMatches();
                }
            }
        }
        yield return new WaitForSeconds(.5f);
        if (_gamePiece.gameObject != null && _gamePiece.GamePieceData.otherGamePiece != null)
        {
            if (_gamePiece.GamePieceData.MatchedType!=MatchedType.Normal && _gamePiece.GamePieceData.otherGamePiece.GamePieceData.MatchedType!=MatchedType.Normal)
            {
                AudioSource.PlayClipAtPoint(_gamePiece.moveSound, transform.position);
                _gamePiece.GamePieceData.PreviousColumn = _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Column;
                _gamePiece.GamePieceData.PreviousRow = _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Row;
                _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Column = _gamePiece.GamePieceData.Column;
                _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Row = _gamePiece.GamePieceData.Row;

                _gamePiece.GamePieceData.Column = _gamePiece.GamePieceData.PreviousColumn;
                _gamePiece.GamePieceData.Row = _gamePiece.GamePieceData.PreviousRow;

                // Update the positions of dots in the _board.AllDots array
                _board.AllPieces[_gamePiece.GamePieceData.Column, _gamePiece.GamePieceData.Row] = _gamePiece;
                _board.AllPieces[_gamePiece.GamePieceData.otherGamePiece.GamePieceData.Column, _gamePiece.GamePieceData.otherGamePiece.GamePieceData.Row] =
                    _gamePiece.GamePieceData.otherGamePiece;
                _board.CurrentGamePiece = null;
                GameController.CurrentState=GameState.Move;
            }
            else
            {
                _board.BoardAnim.DestroyMatches();
            }
        }
    }
}

