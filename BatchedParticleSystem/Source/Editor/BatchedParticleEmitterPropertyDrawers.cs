using UnityEditor;
using UnityEngine;

namespace GrizzlyMachine
{
    [CustomPropertyDrawer(typeof(BatchedParticleEmitter.FloatOverride))]
    [CustomPropertyDrawer(typeof(BatchedParticleEmitter.UIntOverride))]
    [CustomPropertyDrawer(typeof(BatchedParticleEmitter.Vector3Override))]
    [CustomPropertyDrawer(typeof(BatchedParticleEmitter.Color32Override))]
    public class BatchedParticleEmitterPropertyDrawers : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect toggleRect = position;
            toggleRect.width = 15f;
            EditorGUI.PropertyField(toggleRect, property.FindPropertyRelative("enabled"), new GUIContent(), true);

            Rect valueRect = position;
            valueRect.x += 15f;
            valueRect.width -= 20f;
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), label, true);
        }
    }
}