using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class HintObject
{
    public GamePiece thisGamePiece;
    public GamePiece otherGamePiece;
}

public class HintManager : MonoBehaviour
{
    public List<HintObject> hintPieces = new List<HintObject>();
    private Board _board;
    public float hintDelay;
    private float _hintDelaySeconds;
    private HintObject _moveDot;
    public float HintDelaySeconds
    {
        get { return _hintDelaySeconds; }
        set { _hintDelaySeconds = value; }
    }
    public void Initialize(Board board)
    {
        _board = board;
        _hintDelaySeconds = hintDelay;
    }

    private IEnumerator ShowHint(HintObject hint)
    {
        Vector2 originalPositionThis = hint.thisGamePiece.transform.position;
        Vector2 originalPositionOther = hint.otherGamePiece.transform.position;
        yield return new WaitForSeconds(.2f);
        // Swap positions temporarily
        if (hint.thisGamePiece!=null&&hint.otherGamePiece!=null)
        {
            hint.thisGamePiece.IsShowingHint();
            hint.otherGamePiece.IsShowingHint();
            Coroutine moveThis = StartCoroutine(MoveToPosition(hint.thisGamePiece.transform, originalPositionOther, 0.2f));
            Coroutine moveOther = StartCoroutine(MoveToPosition(hint.otherGamePiece.transform, originalPositionThis, 0.2f));
            yield return StartCoroutine(WaitForBothCoroutines(moveThis, moveOther));
            hint.thisGamePiece.IsShowingHint();
            hint.otherGamePiece.IsShowingHint();
        }
    }
    
    private IEnumerator WaitForBothCoroutines(Coroutine coroutine1, Coroutine coroutine2)
    {
        yield return coroutine1;
        yield return coroutine2;
    }
    
    private IEnumerator MoveToPosition(Transform startPos, Vector2 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector2 startingPosition = startPos.position;

        // Move to the target position
        while (elapsedTime < duration)
        {
            if (startPos != null)
            {
                startPos.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        // Ensure final position is exactly the target position
        startPos.position = targetPosition;

        // Wait for a short duration
        yield return new WaitForSeconds(0.2f);

        // Reset elapsed time for the return movement
        elapsedTime = 0;

        // Move back to the starting position
        while (elapsedTime < duration)
        {
            if (startPos != null)
            {
                startPos.position = Vector2.Lerp(targetPosition, startingPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // Ensure final position is exactly the starting position
        startPos.position = startingPosition;
        
    }

    private List<HintObject> FindAllHints()
    {
        if (GameController.CurrentState == GameState.Move)
        {
            for (int i = 0; i < _board.Width; i++)
            {
                for (int j = 0; j < _board.Height; j++)
                {
                    if (_board.AllPieces[i, j] != null && !_board.BlankSpaces[i, j])
                    {
                        if (i < _board.Width - 1 && _board.DeadLock.SwitchAndCheck(i, j, Vector2.right))
                        {
                            Vector2 dir = Vector2.right;
                            HintObject obj = new HintObject();
                            obj.thisGamePiece = _board.AllPieces[i, j];
                            obj.otherGamePiece = _board.AllPieces[i + (int)dir.x, j + (int)dir.y];
                            if (!ContainsSimilarHint(hintPieces, obj))
                            {
                                hintPieces.Add(obj);
                            }
                        }

                        if (j < _board.Height - 1 && _board.DeadLock.SwitchAndCheck(i, j, Vector2.up))
                        {
                            Vector2 dir = Vector2.up;
                            HintObject obj = new HintObject();
                            obj.thisGamePiece = _board.AllPieces[i, j];
                            obj.otherGamePiece = _board.AllPieces[i + (int)dir.x, j + (int)dir.y];
                            if (!ContainsSimilarHint(hintPieces, obj))
                            {
                                hintPieces.Add(obj);
                            }
                        }
                    }
                }
            }
        }
        return hintPieces;
    }
    private bool ContainsSimilarHint(List<HintObject> hintList, HintObject newHint)
    {
        foreach (var hint in hintList)
        {
            if ((hint.thisGamePiece == newHint.thisGamePiece && hint.otherGamePiece == newHint.otherGamePiece) ||
                (hint.thisGamePiece == newHint.otherGamePiece && hint.otherGamePiece == newHint.thisGamePiece))
            {
                return true; // Similar hint already exists
            }
        }

        return false; // No similar hint found
    }

    HintObject PickOneRandomly(List<HintObject> pieces)
    {
        if (pieces.Count > 0)
        {
            int pieceToUse = Random.Range(0, pieces.Count);
            return pieces[pieceToUse];
        }

        return null;
    }

    private void MarkHint()
    {
        _moveDot = PickOneRandomly(hintPieces);

        if (_moveDot != null)
        {
            StartCoroutine(ShowHint(_moveDot));
        }
    }

    private void Update()
    {
        if (GameController.CurrentState==GameState.Move)
        {
            _hintDelaySeconds -= Time.deltaTime;
            if (_hintDelaySeconds<0)
            {
                hintPieces.Clear();
                hintPieces = FindAllHints();
                MarkHint();
                _hintDelaySeconds = hintDelay;
            }
        }
    }
    public void DestroyHint()
    {
        if(hintPieces != null)
        {
            _hintDelaySeconds = hintDelay;
        }
    }
}