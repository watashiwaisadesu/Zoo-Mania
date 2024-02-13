using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FindMatches : MonoBehaviour
{
    private static Board _board;
    public CheckToGenerateBomb CheckToGenerateBomb { get; private set; }
    public List<GamePiece> currentMatches = new List<GamePiece>();
    public void Initialize(Board board)
    {
        _board = board;
        CheckToGenerateBomb = new CheckToGenerateBomb();
        CheckToGenerateBomb.Initialize(board);
    }

    public void FindAllMatches()
    {
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                GamePiece currentGamePiece = _board.AllPieces[i, j];
                if (currentGamePiece != null)
                {
                    if (i > 0 && i < _board.Width - 1)
                    {
                        GamePiece leftGamePiece = _board.AllPieces[i - 1, j];
                        GamePiece rightGamePiece = _board.AllPieces[i + 1, j];

                        if (leftGamePiece != null && rightGamePiece != null)
                        {
                            if (currentGamePiece.ColorableComponent.Color==leftGamePiece.ColorableComponent.Color 
                                && currentGamePiece.ColorableComponent.Color==rightGamePiece.ColorableComponent.Color)
                            {
                                Match(currentGamePiece, leftGamePiece, rightGamePiece);
                            }
                        }
                    }

                    if (j > 0 && j < _board.Height - 1)
                    {
                        GamePiece upGamePiece = _board.AllPieces[i, j + 1];
                        GamePiece downGamePiece = _board.AllPieces[i, j - 1];

                        if (upGamePiece != null && downGamePiece != null)
                        {
                            if (currentGamePiece.ColorableComponent.Color==upGamePiece.ColorableComponent.Color
                                && currentGamePiece.ColorableComponent.Color==downGamePiece.ColorableComponent.Color)
                            {
                                Match(currentGamePiece, upGamePiece, downGamePiece);
                            }
                        }
                    }
                }
            }
        }
    }

    private void Match(params GamePiece[] dots)
    {
        foreach (var dot in dots)
        {
            if (dot.GamePieceData.MatchedType==MatchedType.None)
            {
                if (!currentMatches.Contains(dot))
                {
                    currentMatches.Add(dot);
                }
                dot.GamePieceData.MatchedType = MatchedType.Normal;
            }
        }
    }
}

