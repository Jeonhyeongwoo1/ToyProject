using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;
    public LayerMask targetsLayer;
    public LayerMask obstacleLayer;
    public List<Transform> visibleTargets;
    public int meshResolution;
    public int literation;

    public MeshFilter meshFilter;
    Mesh mesh;

    public Vector3 DirFromAngle(float angle, bool IsGlobal)
    {
        if (!IsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "View Mesh";
        meshFilter.mesh = mesh; 
        StartCoroutine(FindingTargetVisible(0.02f));
    }

    IEnumerator FindingTargetVisible(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindTargetVisible();
            DrawFieldOfView();
        }
    
    }

    void FindTargetVisible(){
        visibleTargets.Clear();
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetsLayer);

        for(int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2){
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleLayer))
                {
                    visibleTargets.Add(target);
                } 
                
            }

        }
    }

    EdgeInfo FindEdge(ViewCastInfo oldCastInfo, ViewCastInfo newCastInfo)
    {
        float minAngle = oldCastInfo.angle;
        float maxAngle = newCastInfo.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < literation; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo castInfo = ViewCast(angle);

            if(castInfo.isHit != oldCastInfo.isHit)
            {
                minAngle = castInfo.angle;
                minPoint = castInfo.point;
            }else
            {
                maxAngle = castInfo.angle;
                maxPoint = castInfo.point;
            }
        }


        return new EdgeInfo(minPoint, maxPoint);
    }

    void DrawFieldOfView()
    {
        int count = Mathf.RoundToInt(viewAngle * meshResolution);
        float angleSize = viewAngle / count;
        
        List<Vector3> viewPoint = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for(int i = 0; i < count; i++)
        {
            float meshAngle = transform.eulerAngles.y - (viewAngle / 2) + (angleSize * i);
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(meshAngle, true) * viewRadius, Color.red);

            ViewCastInfo viewCast = ViewCast(meshAngle);
            if (i > 0)
            {
                if (oldViewCast.isHit != viewCast.isHit)
                {
                    EdgeInfo edgeInfo = FindEdge(oldViewCast, viewCast);
                    if (edgeInfo.pointA != Vector3.zero)
                    {
                        viewPoint.Add(edgeInfo.pointA);
                    }
                    else if (edgeInfo.pointB != Vector3.zero)
                    {
                        viewPoint.Add(edgeInfo.pointB);
                    }
                }
            }
           
            viewPoint.Add(viewCast.point);
            oldViewCast = viewCast;
        }

        int vertexCount = viewPoint.Count + 1;
        Vector3[] vertice = new Vector3[vertexCount];
        int[] triangle = new int[(vertexCount -2) * 3];

        vertice[0] = Vector3.zero;
        for(int i = 0; i < vertexCount-1; i++)
        {
            vertice[i + 1] = transform.InverseTransformPoint(viewPoint[i]);

            if (i < vertexCount - 2)
            {
                triangle[i * 3] = 0;
                triangle[i * 3 + 1] = i + 1;
                triangle[i * 3+ 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertice;
        mesh.triangles = triangle;
        mesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float angle)
    {
        Vector3 dir = DirFromAngle(angle, true);

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, obstacleLayer))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, angle);
        }
    }

    public struct ViewCastInfo
    {
        public bool isHit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool isHit, Vector3 point, float dst, float angle){
            this.isHit = isHit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }

    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }

}
