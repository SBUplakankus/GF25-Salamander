using PrimeTween;
using UnityEngine;

public class TruckMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject pos1;
    void Start()
    {
        Tween.Delay(duration: 15f, () => Tween.Custom(transform.position, pos1.transform.position, duration: 6f, onValueChange: newVal => transform.position = newVal));
    }
}
