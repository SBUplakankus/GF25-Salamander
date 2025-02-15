using System;
using PrimeTween;
using UnityEngine;

namespace World
{
    public class TreeObject : MonoBehaviour
    {
        [SerializeField] private AudioSource treeAudioSource;
        [SerializeField] private GameObject tree;

        private readonly Quaternion _treeRotation = Quaternion.Euler(-80f, 0f, 0f);
        private const int AnimationDuration = 3;
        private const Ease AnimationType = Ease.InCubic;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            CutDownTree();
        }

        private void CutDownTree()
        {
            Tween.Rotation(tree.transform, _treeRotation, AnimationDuration, AnimationType);
        }
    }
}
