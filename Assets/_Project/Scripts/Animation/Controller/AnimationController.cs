using UnityEngine;

namespace HOTS.Animation.Controller
{
    /// <summary>
    /// Abstract base animation controller class.
    /// </summary>
    public abstract class AnimationController : MonoBehaviour
    {
        /// <summary>
        /// Activates walk animation.
        /// </summary>
        public abstract void ActivateWalkAnimation();

        /// <summary>
        /// Deactivates walk animation.
        /// </summary>
        public abstract void DeactivateWalkAnimation();
    }
}
