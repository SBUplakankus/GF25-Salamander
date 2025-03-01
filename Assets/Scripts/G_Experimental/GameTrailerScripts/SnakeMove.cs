using PrimeTween;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    [SerializeField] private GameObject pos1;
    [SerializeField] private GameObject cameraModel;
    [SerializeField] private GameObject pos2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tween.Delay(duration: 6f, () => Tween.Custom(transform.position, pos1.transform.position, duration: 6f, onValueChange: newVal => transform.position = newVal));
        Tween.Delay(duration: 6.5f, () => Tween.Custom(cameraModel.transform.position, pos2.transform.position, duration: 3.5f, onValueChange: newVal => cameraModel.transform.position = newVal));
    }
}
