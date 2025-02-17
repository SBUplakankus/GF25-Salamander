using System;
using PrimeTween;
using UnityEngine;

namespace World
{
    public class TreeObject : MonoBehaviour
    {
        [SerializeField] private AudioSource treeAudioSource;
        [SerializeField] private GameObject tree;
        private bool _cutDown;

        private readonly Quaternion _treeRotation = Quaternion.Euler(-80f, 0f, 0f);
        private const int AnimationDuration = 5;
        private const Ease AnimationType = Ease.InExpo;

        private void Start()
        {
            _cutDown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_cutDown) return;
            if (!other.gameObject.CompareTag("Player")) return;
            CutDownTree();
        }

        private void CutDownTree()
        {
            Tween.Rotation(tree.transform, _treeRotation, AnimationDuration, AnimationType);
            _cutDown = true;
            treeAudioSource.Play();
        }
    }
}
