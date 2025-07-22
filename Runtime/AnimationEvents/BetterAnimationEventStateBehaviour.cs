using UnityEngine;

namespace Xprees.AnimatorUtils.AnimationEvents
{
    /// This class is a state machine behaviour that triggers animation events at specified normalized times.
    public class BetterAnimationEventStateBehaviour : StateMachineBehaviour
    {
#if UNITY_EDITOR
        [Tooltip(
            "Description of the animation event. This is for documentation purposes Editor-only and does not affect functionality.")]
        [SerializeField]
        [TextArea(1, 5)]
        private string description;

#endif
        [Header("Trigger Settings")] [Tooltip("If true, the event will be triggered every time the animation loops.")]
        public bool activateOnLoop = false;

        [Header("Event")]
        [Tooltip("Name of the event to trigger when the specified time is reached. " +
                 "This should match the event name (case-sensitive) in BetterAnimationEventReceiver.")]
        public string eventName;

        [Tooltip("Normalized time at which the event should be triggered (0 to 1).")] [Range(0f, 1f)]
        public float triggerTime;

        private bool _eventTriggered;
        private float _prevNormalizedTime;

        private BetterAnimationEventReceiver _eventReceiver;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetEventTrigger();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var currentTime = stateInfo.normalizedTime % 1f;

            var didLooped = currentTime < _prevNormalizedTime;
            if (activateOnLoop && didLooped)
            {
                ResetEventTrigger();
            }

            if (!_eventTriggered && currentTime >= triggerTime)
            {
                TriggerEvent(animator);
            }

            _prevNormalizedTime = currentTime;
        }

        private void TriggerEvent(Animator animator)
        {
            NotifyReceiver(animator);
            _eventTriggered = true;
        }

        private void ResetEventTrigger() => _eventTriggered = false;

        private void NotifyReceiver(Animator animator)
        {
            if (!_eventReceiver) _eventReceiver = animator.GetComponent<BetterAnimationEventReceiver>();

            if (_eventReceiver)
            {
                _eventReceiver.OnAnimationEventTriggered(eventName);
                return;
            }

            Debug.LogWarning(
                $"Animator '{animator.name}' does not have a {nameof(BetterAnimationEventReceiver)} component to handle the event '{eventName}'.");
        }
    }
}