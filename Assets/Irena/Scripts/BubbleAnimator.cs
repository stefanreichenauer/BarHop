using UnityEngine;

public class BubbleAnimator : MonoBehaviour
{
    public ParticleSystem particleSystem = null;
    public GameObject proxyModel = null;
    float timer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        proxyModel.SetActive(false);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Max(timer - Time.deltaTime, 0.0f);
        if(timer == 0)
        {
            proxyModel.transform.localScale = Vector3.zero;
            enabled = false;
            proxyModel.SetActive(false);
        }

        float f = timer / 0.5f;
        float scale = Mathf.Lerp(0.0f, 1.0f, f);
        proxyModel.transform.localScale = Vector3.one * scale;
    }

    public void PopBubble()
    {
        Debug.Log("pop bubble");
        proxyModel.SetActive(true);
        proxyModel.transform.localScale = Vector3.one;
        enabled = true;
        timer = 0.5f;
        particleSystem.Play();
    }
}
