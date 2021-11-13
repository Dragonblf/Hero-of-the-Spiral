using System;
using UnityEngine;

namespace HOTS.Manager.Battle
{
    /// <summary>
    /// Class provides an event system for battle
    /// related aspects.
    /// </summary>
    public class BattleEventSystem
    {
        /// <summary>
        /// Event to subscribe on to get
        /// notified when a battle begin was
        /// invoked.
        /// </summary>
        public event Action<GameObject, GameObject> onBattleBegin = (o1, o2) => { };

        /// <summary>
        /// Method to invoke to notify about a battle begin.
        /// </summary>
        /// <param name="player">Player in battle</param>
        /// <param name="enemy">Enemy in battle</param>
        public void BattleBegin(GameObject player, GameObject enemy)
        {
            onBattleBegin(player, enemy);
        }


        /// <summary>
        /// Event to subscribe on to get
        /// notified when a battle end was
        /// invoked.
        /// </summary>
        public event Action onBattleEnd = () => { };

        /// <summary>
        /// Method to invoke to notify about a battle end.
        /// </summary>
        public void BattleEnd()
        {
            onBattleEnd();
        }
    }
}