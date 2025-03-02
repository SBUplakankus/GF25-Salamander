using System.Collections;
using PrimeTween;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [Header("Water Attributes")]
    [SerializeField] private float delay;
    [SerializeField] private float finalSize;
    [SerializeField] private float duration;
    void Start()
    {
        //Tween.Delay(duration: delay, () => Tween.Custom(transform.localScale, transform.localScale*finalSize, duration: duration, onValueChange: newVal => transform.localScale = newVal));
        StartCoroutine(WaterScale());
    }

    private IEnumerator WaterScale()
    {
        yield return new WaitForSeconds(delay);
        Tween.Scale(transform, transform.localScale * finalSize, duration);
    }
}
