using System.Collections;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private AudioClip explosionSound;
    protected GamePiece Piece;
    private PointsEffectManager _pointsEffectManager;

    private void Awake()
    {
        Piece = GetComponent<GamePiece>();
        _pointsEffectManager = FindObjectOfType<PointsEffectManager>();
    }

    public void ClearNoAnimation()
    {
        Destroy(gameObject);
    }

    public virtual void ActivatePieces()
    {
    }

    public virtual void ClearPiece()
    {
        _pointsEffectManager.ShowPointsEffect(Piece.GamePieceData.score*Piece.Board.streak);
        if (Piece != null)
        {
            if ( Piece.Hud.level != null)
            {
                Piece.Hud.level.OnPieceCleared(Piece);
            }
            if (Piece.Board != null && Piece.Board.AllPieces != null)
            {
                Piece.Board.AllPieces[Piece.GamePieceData.Column, Piece.GamePieceData.Row] = null;
            }
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }
            Destroy(gameObject);
        }
    }
}