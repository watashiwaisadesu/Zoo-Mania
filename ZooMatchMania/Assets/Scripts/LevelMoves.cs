
    using System.Collections;

    public class LevelMoves : Level
    {

        public int numMoves;
        public int targetScore;

        private int _movesUsed;

        public override void Initialize()
        {
            base.Initialize();
            type = LevelType.Moves;
            hud.SetLevelType(type);
            hud.SetScore(CurrentScore);
            hud.SetTarget(targetScore);
            hud.SetRemaining(numMoves);
        }

        public override void OnMove()
        {
            _movesUsed++;
            
            hud.SetRemaining(numMoves - _movesUsed);

            if (_movesUsed >= numMoves)
            {
                StartCoroutine(wEnumerator());
            }
        }

        private IEnumerator wEnumerator()
        {
            while (GameController.CurrentState!=GameState.Move)
            {
                yield return null;
            }
            if (CurrentScore >= targetScore)
            {
                GameWin();
            }
            else
            {
                GameLose();
            }
        }

    }
