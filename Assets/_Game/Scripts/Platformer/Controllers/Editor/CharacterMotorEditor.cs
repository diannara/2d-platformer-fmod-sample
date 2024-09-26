using UnityEngine;
using UnityEditor;

namespace TIGD.Platformer.Controllers.Editors
{
    [CustomEditor(typeof(CharacterMotor))]
    public class CharacterMotorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterMotor motor = (CharacterMotor)target;

            if(motor.ShowDebugInfo)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Vector3Field("Current Velocity", motor.CurrentVelocity);
                EditorGUILayout.Toggle("Is Grounded", motor.IsGrounded);
                EditorGUI.EndDisabledGroup();
            }

            if(Application.isPlaying)
            {
                Repaint();
            }
        }
    }
}