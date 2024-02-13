using System.Collections;
using TMPro;
using UnityEngine;

public class PointEffect : MonoBehaviour
{
    public float Point;
    private float GetHitEffect;
    private float targY;
    private Vector3 PointPosition;

    public TextMeshProUGUI pointsText;
    public float fadeDuration = 1.0f;

    void Start()
    {
        Point = Mathf.Round(Random.Range(Point / 2, Point * 2));
        PointPosition = transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        targY = Screen.height / 2;

        // Ensure the UI Text component is assigned
        if (pointsText == null)
        {
            Debug.LogError("UI Text component is not assigned to the script.");
        }
    }

    void Update()
    {
        targY -= Time.deltaTime * 200;
    }

    public void OnDestroy()
    {
        // Trigger the points effect when the object is destroyed
        DisplayPointsEffect();
    }

    void DisplayPointsEffect()
    {
        StartCoroutine(AnimatePointsEffect());
    }

    IEnumerator AnimatePointsEffect()
    {
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(PointPosition);

        GetHitEffect = 0;

        while (GetHitEffect < 50)
        {
            GetHitEffect += Time.deltaTime * 30;
            float alpha = 1.0f - (GetHitEffect - 50) / 7;
            pointsText.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            yield return null;
        }

        // Set the points text and start fading out
        pointsText.text = "+" + Point.ToString();
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer / fadeDuration);
            pointsText.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            yield return null;
        }

        // Ensure the points text is fully faded out
        pointsText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Destroy(this);
    }
}