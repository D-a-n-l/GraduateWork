using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    [SerializeField] 
    public Transform start;

    [SerializeField]
    public Transform end;

    [Min(.25f)]
    [SerializeField] 
    private float maxDistance;

    [Min(2)]
    [SerializeField]
    private int pointsCount = 50;

    [SerializeField] 
    private float length = 10f;

    [SerializeField, Range(0, 1)]
    private float rigidity = 0.5f;

    [SerializeField] 
    private float weight = 1f;

    private LineRenderer lineRenderer;

    private TextTips textTips;

    private Vector3[] points;

    private Color defaultColor;

    public RaycastControl raycast;
    public float _maxDistance { get { return maxDistance; } }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        textTips = GetComponent<TextTips>();

        defaultColor = lineRenderer.material.color;
    }

    private void Update()
    {
        if (end != null)
        {
            if (points == null || points.Length != pointsCount)
                points = new Vector3[pointsCount];

            lineRenderer.positionCount = pointsCount;
            lineRenderer.SetPositions(points);

            Lerp(start, end, pointsCount);

            float distance = (start.position - end.position).magnitude;

            if (distance >= maxDistance / 1.5f)
            {
                ChangeColor(true, Color.red);

                if (distance >= maxDistance)
                {
                    SetEnd(null);

                    raycast.useWire = false;

                    textTips.ChangeActiveText();
                    //StartCoroutine(textTips.ChangeCanUseWire(0f, true, textTips._canUseWireTypeOutput));
                }
            }
            else if (distance <= maxDistance / 1.5f)
            {
                ChangeColor(false, Color.white);
            }
        }
    }

    private void Lerp(Transform start, Transform end, int count)
    {
        var rigidity = Mathf.Clamp01(this.rigidity);
        var L = (this.start.position - this.end.position);
        var D = L.magnitude + 0.001f;
        var DD = Mathf.Max(D, length);
        var P0 = this.start.position;
        var P1 = this.start.position + this.start.forward * DD * rigidity / 2;
        var P2 = this.end.position + this.end.forward * DD * rigidity / 2;
        var P3 = this.end.position;
        var overLength = Mathf.Max(0, length - D);

        for (int i = 0; i < count; i++)
        {
            var t = (float)i / (count - 1);

            //Cubic Bezier
            var P01 = Vector3.Lerp(P0, P1, t);
            var P12 = Vector3.Lerp(P1, P2, t);
            var P23 = Vector3.Lerp(P2, P3, t);
            var P012 = Vector3.Lerp(P01, P12, t);
            var P123 = Vector3.Lerp(P12, P23, t);
            var P1234 = Vector3.Lerp(P012, P123, t);

            //add gravity
            var t1 = (t - 0.5f) * 2;//linear -1 : 0 : 1
            var t2 = t1 * t1;//parabola 1 : 0 : 1
            var t3 = 1 - t2;//parabola 0 : 1 : 0

            var gravity = Vector3.up * (t2 * t2 - 1) * weight * t3 * (1 - rigidity) * overLength;

            P1234 += gravity;

            points[i] = P1234;
        }
    }

    private void ChangeColor(bool change, Color changed)
    {
        if(change) lineRenderer.material.color = changed;
        else lineRenderer.material.color = defaultColor;
    }

    public void SetStart(Transform newPoint)
    {
        start = newPoint;

        points = new Vector3[1];

        lineRenderer.positionCount = points.Length;

        lineRenderer.SetPositions(points);
    }

    public void SetEnd(Transform newPoint)
    {
        end = newPoint;

        points = new Vector3[1];

        lineRenderer.positionCount = points.Length;

        lineRenderer.SetPositions(points);
    }
}