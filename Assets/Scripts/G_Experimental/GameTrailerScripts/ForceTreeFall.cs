using System;
using PrimeTween;
using UnityEngine;

namespace World
{
    public class ForceTreeFall : MonoBehaviour
    {
        [SerializeField] private AudioSource treeAudioSource;
        [SerializeField] private GameObject tree;
        [SerializeField] private float delay;
        [SerializeField] private float rotation;

        private Vector3 _treeRotation;
        private const int AnimationDuration = 5;
        private const Ease AnimationType = Ease.InExpo;

        private void Start()
        {
            _treeRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
            Invoke("CutDownTree", delay);
        }
        private void CutDownTree()
        {
            Tween.Rotation(tree.transform, _treeRotation, AnimationDuration, AnimationType);
            treeAudioSource.Play();
        }
    }
}
