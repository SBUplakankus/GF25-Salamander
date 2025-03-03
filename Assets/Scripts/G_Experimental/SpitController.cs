// ============================================================================================
// CLASS: SpitController
// ============================================================================================
// Description:
//   Controls the spit object the player shoots out
// ============================================================================================

using System;
using AI;
using PrimeTween;
using UnityEngine;

public class SpitController : MonoBehaviour
{
    [Header("Spit Attributes")]
    [SerializeField] private float finalSize;
    [SerializeField] private float durationToGrow;
    [SerializeField] private float durationToDisappear;
    [SerializeField] private int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tween.Custom(transform.localScale, transform.localScale*finalSize, duration: durationToGrow, onValueChange: newVal => transform.localScale = newVal);
        Invoke("Disappear", durationToDisappear);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        Disappear();
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
