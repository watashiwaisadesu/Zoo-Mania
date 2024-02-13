using System.Collections;
using TMPro;
using UnityEngine;

public class PointsEffect : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        if (pointsText == null)
        {
            Debug.LogError("UI Text component is not assigned to the script.");
        }
    }

    public void Activate(Vector3 position, float pointValue,float verticalOffset)
    {
        transform.position = position + new Vector3(0, verticalOffset, 0);
        pointsText.text = "+" + pointValue.ToString();
        StartCoroutine(AnimatePointsEffect());
    }

    IEnumerator AnimatePointsEffect()
    {
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

        // Return the points effect to the pool
        FindObjectOfType<PointsEffectManager>().ReturnToPool(this);
    }
}