using UnityEngine;

namespace Xprees.AnimatorUtils.AnimationEvents
{
    /// This class is a state machine behaviour that triggers animation events at specified normalized times.
    public class BetterAnimationEventStateBehaviour : StateMachineBehaviour
    {
        [Tooltip("If true, the event will be triggered every time the animation loops.")]
        public bool activateOnLoop = false;

        [Header("Event")]
        [Tooltip("Name of the event to trigger when the specified time is reached. " +
                 "This should match the event name (case-sensitive) in BetterAnimationEventReceiver.")]
        public string eventName;

        [Tooltip("Normalized time at which the event should be triggered (0 to 1).")] [Range(0f, 1f)]
        public float triggerTime;

        private bool _eventTriggered;
        private float _prevNormalizedTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _eventTriggered = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var currentTime = stateInfo.normalizedTime % 1f;

            if (activateOnLoop && currentTime < _prevNormalizedTime)
            {
                // Reset the event trigger when the animation loops
                _eventTriggered = false;
            }

            if (!_eventTriggered && currentTime >= triggerTime)
            {
                NotifyReceiver(animator);
                _eventTriggered = true;
            }

            _prevNormalizedTime = currentTime;
        }

        private void NotifyReceiver(Animator animator)
        {
            var receiver = animator.GetComponent<BetterAnimationEventReceiver>();
            if (receiver)
            {
                receiver.OnAnimationEventTriggered(eventName);
                return;
            }

            Debug.LogWarning(
                $"Animator '{animator.name}' does not have a {nameof(BetterAnimationEventReceiver)} component to handle the event '{eventName}'.");
        }
    }
}