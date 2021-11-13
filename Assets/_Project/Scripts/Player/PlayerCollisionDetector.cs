using HOTS.Manager.Battle;
using UnityEngine;
using Zenject;

namespace HOTS.Player
{
    /// <summary>
    /// Class contains a method to check collisions
    /// from the player.
    /// </summary>
    public class PlayerCollisionDetector : MonoBehaviour
    {
        /// <summary>
        /// Contains the battle event system.
        /// </summary>
        [Inject] private BattleEventSystem _battleEventSystem;

        /// <summary>
        /// Method will be called when the <see cref="GameObject"/>,
        /// which contains the script, has hit an collider while
        /// containing a controller.
        /// </summary>
        /// <param name="hit">Controller collider hit object</param>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Handle collision with an enemy
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("Test");
                _battleEventSystem.BattleBegin(gameObject, hit.gameObject);
            }
        }
    }

}