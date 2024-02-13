using UnityEngine;

public class Point : MonoBehaviour
{
    public float point;
    private float GetHitEffect;
    private float targY;
    private Vector3 PointPosition;

    public GUISkin PointSkin;
    public GUISkin PointSkinShadow;

    void Start()
    {
        point = Mathf.Round(Random.Range(point / 2, point * 2));
        PointPosition = transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        targY = Screen.height / 2;
    }

    void OnGUI()
    {
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(PointPosition);
        GetHitEffect += Time.deltaTime * 30;
        GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (GetHitEffect - 50) / 7);
        GUI.skin = PointSkinShadow;
        GUI.Label(new Rect(screenPos2.x + 8, targY - 2, 80, 70), "+" + point.ToString());
        GUI.skin = PointSkin;
        GUI.Label(new Rect(screenPos2.x + 10, targY, 120, 120), "+" + point.ToString());
    }

    void Update()
    {
        targY -= Time.deltaTime * 200;
    }
}