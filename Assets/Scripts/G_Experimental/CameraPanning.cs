using PrimeTween;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    public GameObject camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("TestReset", 5f);
    }

    void PanCamera()
    {
        Tween.PositionZ(camera.transform, 2, 15f);
        Tween.Custom(30, 60, duration: 15f, onValueChange: newVal  => Camera.main.fieldOfView = newVal);
        Tween.Delay(duration: 16f, () => Tween.Custom(60, 20, duration: 4f, onValueChange: newVal => Camera.main.fieldOfView = newVal));
        Invoke("TestReset", 25f);
    }

    void TestReset()
    {
        Camera.main.fieldOfView = 30;
        camera.transform.position = new Vector3(0f, 1.7f, 7f);
        Invoke("PanCamera", 10f);
    }
}
