using PrimeTween;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    public GameObject camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("PanCamera", 5f);
    }

    void PanCamera()
    {
        Tween.PositionZ(camera.transform, -12, 10f);
        Tween.Custom(60, 15, duration: 10f, onValueChange: newVal => Camera.main.fieldOfView = newVal);
    }
  
}
