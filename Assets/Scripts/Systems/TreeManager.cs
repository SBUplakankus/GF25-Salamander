// ============================================================================================
// CLASS: TreeManager
// ============================================================================================
// Description:
//   Triggers the trees falling around the map as the game progresses
// ============================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Systems
{
    public class TreeManager : MonoBehaviour
    {
        [SerializeField] private List<TreeObject> treesToFall;
        private float _gameTime;

        private void Start()
        {
            _gameTime = 0;
        }

        private void Update()
        {
            _gameTime += Time.deltaTime;
            switch (_gameTime)
            {
                case > 120:
                    treesToFall[5].SetTreeToFall();
                    break;
                case > 100:
                    treesToFall[4].SetTreeToFall();
                    break;
                case > 80:
                    treesToFall[3].SetTreeToFall();
                    break;
                case > 60:
                    treesToFall[2].SetTreeToFall();
                    break;
                case > 40:
                    treesToFall[1].SetTreeToFall();
                    break;
                case > 20:
                    treesToFall[0].SetTreeToFall();
                    break;
            }
        }
    }
}
