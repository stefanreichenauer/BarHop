using UnityEngine;

public class PlaneDefiner : MonoBehaviour
{

    [SerializeField] private Vector3 planeNormal;
    private Plane myPlane;
    private void Start()
    {
        myPlane = new Plane(planeNormal, transform.position);
    }
    public Plane getPlane()
    {
        return myPlane;
    }
}
