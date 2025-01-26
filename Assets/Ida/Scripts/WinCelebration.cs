using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinCelebration : MonoBehaviour
{
    [SerializeField] private float wait_after_goal_seconds;
    [SerializeField] private SubMenu submenu;

    private void Start()
    {
        if (submenu == null)
        {
            Debug.LogWarning("Win menu should be specified in inspector, currently using workaround");
            submenu = FindFirstObjectByType<SubMenu>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(waitUntilCelebration());
    }

    private IEnumerator waitUntilCelebration()
    {
        yield return new WaitForSeconds(wait_after_goal_seconds);
        Celebrate();
    }

    private void Celebrate()
    {
        submenu.OpenSubMenu();
    }
}
