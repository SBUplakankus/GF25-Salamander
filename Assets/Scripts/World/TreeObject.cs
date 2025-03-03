// ============================================================================================
// CLASS: TreeObject
// ============================================================================================
// Description:
//   Controls the trees that fall in various places around the map
//
// Methods:
//   - SetTreeToFall: Sets the tree to fall over at the first available oppurtunity
//   - CutDownTree: Plays the tree chop down animation and audio
//
// Related Classes:
//   - TreeManager: Controls when each Tree Object gets set to fall
// ============================================================================================

using System;
using PrimeTween;
using UnityEngine;

namespace World
{
    public class TreeObject : MonoBehaviour
    {
        [SerializeField] private AudioSource treeAudioSource;
        [SerializeField] private GameObject tree;
        [SerializeField] private GameObject fallBlocker;
        private bool _playerInside;
        private bool _readyToBeCut;
        private bool _treeCutDown;

        private Vector3 _treeRotation;
        private const int AnimationDuration = 5;
        private const Ease AnimationType = Ease.InExpo;

        private void Start()
        {
            fallBlocker.SetActive(false);
            _playerInside = false;
            _treeCutDown = false;
            _readyToBeCut = false;
            _treeRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 100);
        }

        private void Update()
        {
            if (_playerInside || _treeCutDown || !_readyToBeCut) return;
            CutDownTree();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _playerInside = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _playerInside = false;
        }
        
        /// <summary>
        /// Setting the tree to fall means it will trigger the fall animation at the first oppurtunity
        /// </summary>
        public void SetTreeToFall()
        {
            _readyToBeCut = true;
        }
        
        private void CutDownTree()
        {
            fallBlocker.SetActive(true);
            Tween.Rotation(tree.transform, _treeRotation, AnimationDuration, AnimationType);
            treeAudioSource.Play();
            _treeCutDown = true;
        }
    }
}
