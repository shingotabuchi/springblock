using UnityEngine;
using TMPro;

public class BondLineRenderer : MonoBehaviour
{
    public bool debugBool;
    public LineRenderer lineRenderer;
    public Transform endPoint0;
    public Transform endPoint1;
    public TextMeshProUGUI text;
    private void Update()
    {
        lineRenderer.SetPosition(0, endPoint0.position);
        lineRenderer.SetPosition(1, endPoint1.position);
        text.transform.position = (endPoint0.position + endPoint1.position) / 2f;
    }
}