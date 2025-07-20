using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Xprees.AnimatorUtils.AnimationEvents;
using Xprees.AnimatorUtils.Editor.Extensions;

namespace Xprees.AnimatorUtils.Editor
{
    [CustomEditor(typeof(BetterAnimationEventStateBehaviour))]
    public class BetterAnimationEventStateBehaviourEditor : UnityEditor.Editor
    {
        private Texture2D _recordIcon;

        private void OnEnable()
        {
            _recordIcon = EditorGUIUtility.Load("d_Animation.Record") as Texture2D;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            DrawSyncTriggerTimeButton();
        }

        private void DrawSyncTriggerTimeButton()
        {
            var prevEnabled = GUI.enabled;

            var behaviour = (BetterAnimationEventStateBehaviour)target;
            var animatorState = behaviour?.FindCurrentAnimatorState();

            GUI.enabled =
                !Application.isPlaying
                && behaviour
                && animatorState
                && (AnimatorStateObjectPreview.ActiveInstance?.HasPreviewGUI() ?? false);
            // Ensure the button is only enabled when not playing and the behaviour and animator state are valid.
            if (GUILayout.Button(new GUIContent("Sync Trigger with Preview", _recordIcon)))
            {
                if (behaviour && animatorState)
                {
                    SyncPreviewTimeWithTriggerTime(behaviour, animatorState);
                }
            }

            GUI.enabled = prevEnabled;
        }

        private void SyncPreviewTimeWithTriggerTime(
            BetterAnimationEventStateBehaviour behaviour,
            AnimatorState animatorState)
        {
            var animatorStateInstanceId = animatorState.GetInstanceID();
            var preview = AnimatorStateObjectPreview.ActiveInstance;
            if (preview == null || preview.ActiveTargetInstanceId != animatorStateInstanceId)
            {
                Debug.LogWarning($"No active {nameof(AnimatorStateObjectPreview)} found for the current state.");
                return;
            }

            var previewTime = preview.GetCurrentPreviewNormalizedTime();
            Undo.RecordObject(behaviour, $"{animatorState.name} - Preview Time Sync with Trigger Time");
            behaviour.triggerTime = previewTime;
            EditorUtility.SetDirty(behaviour);
        }
    }
}