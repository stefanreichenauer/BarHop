using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GoalReacher : MonoBehaviour
{
    [SerializeField] UnityEvent reachedGoal;
    private void OnTriggerEnter(Collider other)
    {
        reachedGoal.Invoke();
        Debug.Log("Reached Goal!");
        gameObject.SetActive(false);
    }
}
