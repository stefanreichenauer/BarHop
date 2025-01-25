using UnityEngine;

public class PlaneDefiner : MonoBehaviour
{
    [SerializeField] private GameObject fanSolids;
    [SerializeField] private GameObject airflow;
    private Vector3 planeNormal;
    private Plane myPlane;
    private void Awake()
    {
        
        planeNormal = Vector3.Normalize(airflow.transform.position - fanSolids.transform.position);
        myPlane = new Plane(planeNormal, transform.position);
    }
    public Plane getPlane()
    {
        return myPlane;
    }

    public Vector2 getNormal()
    {

        return new Vector2(planeNormal.x, planeNormal.y);
    }
}
