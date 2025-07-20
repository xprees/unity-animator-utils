using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Xprees.AnimatorUtils.Editor
{
    [CustomPreview(typeof(AnimatorState))]
    public class AnimatorStateObjectPreview : ObjectPreview
    {
        private static FieldInfo _cachedAvatarPreviewField;
        private static FieldInfo _cachedTimeControlField;
        private static FieldInfo _cachedStopTimeField;
        private static PropertyInfo _cachedNormalizedTimeProperty;

        private UnityEditor.Editor _preview;
        private int _animationClipId;

        public override void Initialize(Object[] targets)
        {
            base.Initialize(targets);
            if (targets.Length > 1 || Application.isPlaying) return;

            CacheSourceAnimationEditorFields();

            var clip = GetAnimationClip(target as AnimatorState);
            if (clip != null)
            {
                InitializeClipPreview(clip);
            }
        }

        private void InitializeClipPreview(AnimationClip clip)
        {
            _preview = UnityEditor.Editor.CreateEditor(clip);
            _animationClipId = clip.GetInstanceID();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            CleanupClipPreview();
        }

        private void CleanupClipPreview()
        {
            if (!_preview) return;

            Object.DestroyImmediate(_preview);
            _preview = null;
            _animationClipId = 0;
        }

        public override bool HasPreviewGUI() => _preview?.HasPreviewGUI() ?? false;

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            base.OnInteractivePreviewGUI(r, background);

            var currentClip = GetAnimationClip(target as AnimatorState);
            if (currentClip != null && currentClip.GetInstanceID() != _animationClipId)
            {
                CleanupClipPreview();
                InitializeClipPreview(currentClip);
                return;
            }

            if (_preview)
            {
                UpdateAnimationClipEditor(_preview, currentClip);
                _preview.OnInteractivePreviewGUI(r, background);
            }
        }

        private AnimationClip GetAnimationClip(AnimatorState state) => state?.motion as AnimationClip;

        // Beware: this is a bit of a hack, but it allows us to update the AnimationClipEditor
        // -> This is dependent on the internal structure of Unity's AnimationClipEditor,
        // so it may break in future Unity versions.
        private void UpdateAnimationClipEditor(UnityEditor.Editor editor, AnimationClip clip)
        {
            if (_cachedAvatarPreviewField == null || _cachedTimeControlField == null || _cachedStopTimeField == null)
            {
                return;
            }

            var avatarPreview = _cachedAvatarPreviewField.GetValue(editor);
            var timeControl = _cachedTimeControlField.GetValue(avatarPreview);

            _cachedStopTimeField.SetValue(timeControl, clip.length);
        }

        /// Returns the current normalized time of the preview animation.
        public float GetCurrentPreviewNormalizedTime()
        {
            if (_cachedAvatarPreviewField == null
                || _cachedTimeControlField == null
                || _cachedNormalizedTimeProperty == null)
            {
                return 0f;
            }

            var avatarPreview = _cachedAvatarPreviewField.GetValue(_preview);
            var timeControl = _cachedTimeControlField.GetValue(avatarPreview);
            return (float)_cachedNormalizedTimeProperty.GetValue(timeControl);
        }

        /// We need to a bit of reflection magic to get the fields from the AnimationClipEditor - it's internal :)
        static void CacheSourceAnimationEditorFields()
        {
            if (_cachedAvatarPreviewField == null)
            {
                _cachedAvatarPreviewField = Type.GetType("UnityEditor.AnimationClipEditor, UnityEditor")
                    ?.GetField("m_AvatarPreview", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (_cachedTimeControlField == null)
            {
                _cachedTimeControlField = Type.GetType("UnityEditor.AvatarPreview, UnityEditor")
                    ?.GetField("timeControl", BindingFlags.Instance | BindingFlags.Public);
            }

            if (_cachedStopTimeField == null)
            {
                _cachedStopTimeField = Type.GetType("UnityEditor.TimeControl, UnityEditor")
                    ?.GetField("stopTime", BindingFlags.Instance | BindingFlags.Public);
            }

            if (_cachedNormalizedTimeProperty == null)
            {
                _cachedNormalizedTimeProperty = Type.GetType("UnityEditor.TimeControl, UnityEditor")
                    ?.GetProperty("normalizedTime", BindingFlags.Instance | BindingFlags.Public);
            }
        }
    }
}