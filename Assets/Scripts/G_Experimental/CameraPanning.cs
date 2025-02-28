using PrimeTween;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject salamander;
    
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.main.fieldOfView = 30;
        camera.transform.position = new Vector3(0f, 1.8f, 7f);
        
        lights.SetActive(false);
        _animator = salamander.GetComponent<Animator>();
        
        
        Invoke("StartScene", 2f);
    }

    private void PanCamera()
    {
        Tween.PositionZ(camera.transform, 2, 15f);
        Tween.Custom(30, 60, duration: 15f, onValueChange: newVal  => Camera.main.fieldOfView = newVal).OnComplete(() => Invoke("EndScene", 1f));;
        //Tween.Delay(duration: 16f, () => Tween.Custom(60, 20, duration: 4f, onValueChange: newVal => Camera.main.fieldOfView = newVal));
        //Invoke("TestReset", 25f);
    }

    private void StartScene()
    {
        lights.SetActive(true);
        _animator.SetTrigger("Start");
        Invoke("PanCamera", 5f);
    }

    private void EndScene()
    {
        lights.SetActive(false);
    }
}
