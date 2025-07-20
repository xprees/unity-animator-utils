using UnityEditor.Animations;
using UnityEngine;

namespace Xprees.AnimatorUtils.Editor.Extensions
{
    public static class StateMachineBehaviourExtensions
    {
        /// Retrieves the current <see cref="AnimatorState"/> associated with the given <see cref="StateMachineBehaviour"/>.
        public static AnimatorState FindCurrentAnimatorState(this StateMachineBehaviour behaviour)
        {
            var context = AnimatorController.FindStateMachineBehaviourContext(behaviour);
            if (context.Length > 0)
            {
                return context[0].animatorObject as AnimatorState;
            }

            return null;
        }
    }
}