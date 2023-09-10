using UnityEngine;

public class BondLineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform endPoint0;
    public Transform endPoint1;
    private void Update()
    {
        lineRenderer.SetPosition(0, endPoint0.position);
        lineRenderer.SetPosition(1, endPoint1.position);
    }
}