using PrimeTween;
using UnityEngine;

public class SpitController : MonoBehaviour
{
    [Header("Spit Attributes")]
    [SerializeField] private float finalSize;
    [SerializeField] private float durationToGrow;
    [SerializeField] private float durationToDisappear;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tween.Custom(transform.localScale, transform.localScale*finalSize, duration: durationToGrow, onValueChange: newVal => transform.localScale = newVal);
        Invoke("Disappear", durationToDisappear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
