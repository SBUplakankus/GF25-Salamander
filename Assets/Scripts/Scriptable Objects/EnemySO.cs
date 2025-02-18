using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        [Header("Enemy Stats")] 
        public int enemyHealth;
        public int enemySpeed;
        public int enemyDamage;
        public float enemyAttackInterval;
        
    }
}
