using System.Collections;
using UnityEngine;

public class WinCelebration : MonoBehaviour
{
    [SerializeField] private float wait_after_goal_seconds;
    [SerializeField] private SubMenu submenu;
    private void onReachedGoal()
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
