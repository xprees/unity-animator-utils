using System;
using UnityEngine.Events;

namespace Xprees.AnimatorUtils.AnimationEvents
{
    /// This class represents an animation event that can be triggered during an animation.
    [Serializable]
    public class BetterAnimationEvent
    {
        public string eventName;
        public UnityEvent onAnimationEvent;
    }
}