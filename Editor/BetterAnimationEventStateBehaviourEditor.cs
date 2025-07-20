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
        private Texture2D _loadTimeIcon;

        private void OnEnable()
        {
            _recordIcon = EditorGUIUtility.Load("d_Animation.Record") as Texture2D;
            _loadTimeIcon = EditorGUIUtility.Load("d_SceneViewCamera") as Texture2D;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            DrawUtilButtons();
        }

        private void DrawUtilButtons()
        {
            var prevEnabled = GUI.enabled;

            var behaviour = (BetterAnimationEventStateBehaviour)target;
            var animatorState = behaviour?.FindCurrentAnimatorState();

            GUI.enabled =
                !Application.isPlaying
                && behaviour
                && animatorState
                && (AnimatorStateObjectPreview.ActiveInstance?.HasPreviewGUI() ?? false);
            // Ensure the buttons are only enabled when not playing and the behaviour and animator state are valid.

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("Record from Preview", _recordIcon)))
            {
                if (behaviour && animatorState)
                {
                    SyncTriggerWithPreview(behaviour, animatorState);
                }
            }

            if (GUILayout.Button(new GUIContent("Show in Preview", _loadTimeIcon)))
            {
                if (behaviour && animatorState)
                {
                    SetPreviewToTriggerTime(behaviour, animatorState);
                }
            }

            GUILayout.EndHorizontal();

            GUI.enabled = prevEnabled;
        }

        private void SyncTriggerWithPreview(BetterAnimationEventStateBehaviour behaviour, AnimatorState animatorState)
        {
            var animatorStateInstanceId = animatorState.GetInstanceID();
            var preview = AnimatorStateObjectPreview.ActiveInstance;
            if (preview == null || preview.ActiveTargetInstanceId != animatorStateInstanceId)
            {
                Debug.LogWarning($"No active {nameof(AnimatorStateObjectPreview)} found for the current state.");
                return;
            }

            var previewTime = preview.GetCurrentPreviewNormalizedTime();
            Undo.RecordObject(behaviour, $"{animatorState.name} - Record Trigger Time from Preview");
            behaviour.triggerTime = previewTime;
            EditorUtility.SetDirty(behaviour);
        }

        private void SetPreviewToTriggerTime(BetterAnimationEventStateBehaviour behaviour, AnimatorState animatorState)
        {
            var animatorStateInstanceId = animatorState.GetInstanceID();
            var preview = AnimatorStateObjectPreview.ActiveInstance;
            if (preview == null || preview.ActiveTargetInstanceId != animatorStateInstanceId)
            {
                Debug.LogWarning($"No active {nameof(AnimatorStateObjectPreview)} found for the current state.");
                return;
            }

            var triggerTime = behaviour.triggerTime;
            preview.SetCurrentPreviewNormalizedTime(triggerTime);
        }
    }
}