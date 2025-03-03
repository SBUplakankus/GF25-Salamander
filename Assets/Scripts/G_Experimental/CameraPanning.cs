// ============================================================================================
// CLASS: CameraPanning
// ============================================================================================
// Description:
//   Controls the final cutscene at the end of the game
// ============================================================================================

using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CameraPanning : MonoBehaviour
{
    [FormerlySerializedAs("camera")]
    [Header("Settings")]
    [SerializeField] private GameObject cameraModel;
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject salamander;
    [SerializeField] private CanvasGroup text;
    
    private Animator _animator;
    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Camera.main.fieldOfView = 30;
        cameraModel.transform.position = new Vector3(0f, 1.8f, 7f);
        
        lights.SetActive(false);
        _animator = salamander.GetComponent<Animator>();
        _audioSource = cameraModel.GetComponent<AudioSource>();
        
        Invoke("StartSound", 0.5f);
        //_audioSource.Play();
        Invoke("StartScene", 2f);
    }

    private void PanCamera()
    {
        Tween.PositionZ(cameraModel.transform, 2, 15f);
        Tween.Custom(30, 60, duration: 15f, onValueChange: newVal  => Camera.main.fieldOfView = newVal).OnComplete(() => Invoke("EndScene", 1f));;
        //Tween.Delay(duration: 16f, () => Tween.Custom(60, 20, duration: 4f, onValueChange: newVal => Camera.main.fieldOfView = newVal));
        //Invoke("TestReset", 25f);
    }

    private void StartScene()
    {
        //_audioSource.Play();
        lights.SetActive(true);
        _animator.SetTrigger("Start");
        Invoke("PanCamera", 5f);
    }

    private void EndScene()
    {
        lights.SetActive(false);
        StartCoroutine(EndingCoroutine());
    }

    private void StartSound()
    {
        _audioSource.Play();
    }

    private IEnumerator EndingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Tween.Alpha(text, 1, 2.5f);
        yield return new WaitForSeconds(4f);
        Tween.Alpha(text, 0, 2.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }
}
