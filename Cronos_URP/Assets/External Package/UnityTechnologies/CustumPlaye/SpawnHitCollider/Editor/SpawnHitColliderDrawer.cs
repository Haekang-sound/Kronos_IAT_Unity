using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnHitColliderBehaviour))]
public class SpawnHitColliderDrawer : PropertyDrawer
{
    SerializedProperty colliderTypeProp;
    int colliderTypeValue;

    float offset = 4f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        colliderTypeProp = property.FindPropertyRelative("colliderType");

        colliderTypeValue = colliderTypeProp.enumValueIndex;

        int fieldCount = 10;

        //switch (colliderTypeValue)
        //{
        //    case 0: // Box
        //        fieldCount = 5;
        //        break;
        //    case 1: // Sphere
        //        fieldCount = 5;
        //        break;
        //    case 2: // Capsule
        //        fieldCount = 8;
        //        break;
        //    default:
        //        fieldCount = 0; 
        //        break;
        //}

        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //public float damageAmount = 1;
        //public LayerMask targetLayers;

        SerializedProperty damageAmountProp = property.FindPropertyRelative("damageAmount");
        SerializedProperty targetLayersProp = property.FindPropertyRelative("targetLayers");
        SerializedProperty centerProp = property.FindPropertyRelative("center");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight + offset);
        EditorGUI.PropertyField(singleFieldRect, damageAmountProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
        EditorGUI.PropertyField(singleFieldRect, targetLayersProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
        EditorGUI.PropertyField(singleFieldRect, colliderTypeProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
        EditorGUI.PropertyField(singleFieldRect, centerProp);

        switch (colliderTypeValue)
        {
            case 0: // Box
                {
                    SerializedProperty sizeProp = property.FindPropertyRelative("size");

                    singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
                    EditorGUI.PropertyField(singleFieldRect, sizeProp);
                }
                break;
            case 1: // Sphere
                {
                    SerializedProperty radiusProp = property.FindPropertyRelative("radius");

                    singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
                    EditorGUI.PropertyField(singleFieldRect, radiusProp);
                }
                break;
            case 2: // Capsule
                {
                    SerializedProperty radiusProp = property.FindPropertyRelative("radius");
                    SerializedProperty heightProp = property.FindPropertyRelative("height");
                    SerializedProperty directionProp = property.FindPropertyRelative("direction");

                    singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
                    EditorGUI.PropertyField(singleFieldRect, radiusProp);

                    singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
                    EditorGUI.PropertyField(singleFieldRect, heightProp);

                    singleFieldRect.y += EditorGUIUtility.singleLineHeight + offset;
                    EditorGUI.PropertyField(singleFieldRect, directionProp);
                }
                break;
            default:
                break;
        }
    }
}
