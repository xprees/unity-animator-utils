using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xprees.AnimatorUtils.AnimationEvents
{
    /// Animation event receiver that listens for specific animation events and invokes corresponding actions.
    public class BetterAnimationEventReceiver : MonoBehaviour
    {
        [Tooltip("List of animation events to handle.")] [SerializeField]
        private List<BetterAnimationEvent> animationEvents = new();

        public void OnAnimationEventTriggered(string eventName)
        {
            // Check should be case-sensitive probably
            var matchingEvent = animationEvents
                .Find(e => e.eventName.Equals(eventName, StringComparison.InvariantCulture));
            matchingEvent?.onAnimationEvent?.Invoke();
        }
    }
}