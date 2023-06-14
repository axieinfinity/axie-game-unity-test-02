using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AIController : MonoBehaviour
    {
        DefenseState _defenseState;
        int _shootAngle = 30;
        int _power = 0;

        private void Awake()
        {
            var manager = FindObjectOfType<GameDefenseManager>();
            if (manager == null)
            {
                this.enabled = false;
            }
            else
            {
                _defenseState = manager.GetState();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_defenseState == null || !_defenseState.isPlaying || _defenseState.playerState.shootDelay > 0) return;

            EnemyState nearestEnemy = FindTarget();
            if (nearestEnemy == null) return;

            int nearestPower = DeterminePower(nearestEnemy.pos);
            if (nearestPower != -1)
            {
                if (_defenseState.energy >= DefenseState.ENERGY_SHOT_MAX_CHARGE)
                {
                    _defenseState.DoShootSpecial(_shootAngle, nearestPower);
                }
                else
                {
                    _defenseState.DoShoot(_shootAngle, nearestPower);
                }
            }
        }

        EnemyState FindTarget()
        {
            EnemyState nearestEnemy = null;
            float nearestDist = -1;
            foreach (var p in _defenseState.enemyStates)
            {
                var enemy = p.Value;
                if (nearestEnemy == null || enemy.pos.x < nearestDist)
                {
                    nearestDist = enemy.pos.x;
                    nearestEnemy = enemy;
                }
            }
            return nearestEnemy;
        }

        int DeterminePower(Vector2 target)
        {
            _power += 10;
            if(_power > 100)
            {
                _power = 0;
            }
            return _power;
        }
    }
}
