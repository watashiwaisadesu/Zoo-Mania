using UnityEngine;

public class LevelTimer : Level
{
    public int timeInSeconds;
    public int targetScore;

    private float _timer;

    public override void Initialize()
    {
        base.Initialize();
        type = LevelType.Timer;
        hud.SetLevelType(type);
        hud.SetScore(CurrentScore);
        hud.SetTarget(targetScore);
        hud.SetRemaining($"{timeInSeconds / 60}:{timeInSeconds % 60:00}");
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        hud.SetRemaining(
            $"{(int) Mathf.Max((timeInSeconds - _timer) / 60, 0)}:{(int) Mathf.Max((timeInSeconds - _timer) % 60, 0):00}");

        if (timeInSeconds - _timer <= 0)
        {
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

}
