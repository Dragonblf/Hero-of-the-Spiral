using HOTS.Player;
using UnityEngine;

namespace HOTS.Animation.Controller
{
    /// <summary>
    /// Player animation controller.
    /// </summary>
    public class PlayerAnimationController : AnimationController
    {
        /// <summary>
        /// Contains the player controller.
        /// </summary>
        private PlayerController _playerController;


        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
        }

        public override void ActivateWalkAnimation()
        {
            _playerController?.ActivateWalkAnimation();
        }

        public override void DeactivateWalkAnimation()
        {
            _playerController?.DeactivateWalkAnimation();
        }
    }
}
