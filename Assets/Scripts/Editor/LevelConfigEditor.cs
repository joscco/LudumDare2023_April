using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LevelDesign
{
    [CustomEditor(typeof(LevelData))]
    public class LevelConfigEditor : Editor
    {
        private ReorderableList _textsList;

        private ReorderableList _riverList;
        private ReorderableList _bridgeList;
        private ReorderableList _streetList;

        private ReorderableList _supplierList;
        private ReorderableList _consumerList;

        private ReorderableList _dogList;
        private ReorderableList _policeList;
        private ReorderableList _trafficLightList;

        private bool _showTexts;
        private bool _showLandscape;
        private bool _showInfrastructure;

        private void OnEnable()
        {
            _textsList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("IntroTexts"),
                true, true, true, true);

            _textsList.elementHeight = 100;
            _textsList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _textsList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 70), element.FindPropertyRelative("Text"));
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 80, rect.width, 20),
                    element.FindPropertyRelative("ShowMafiaGuy"));
            };
            _textsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Intro Texts");

            // Landscape
            _riverList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Rivers"),
                true, true, true, true);

            _riverList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _riverList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
            };
            _riverList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "River Fields");

            _bridgeList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Bridges"),
                true, true, true, true);

            _bridgeList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _bridgeList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 160, rect.y - 7, 150, 30), "Type");
                EditorGUI.PropertyField(new Rect(rect.x + 200, rect.y, 150, 30),
                    element.FindPropertyRelative("Type"), GUIContent.none);
            };
            _bridgeList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Bridges");

            _streetList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Streets"),
                true, true, true, true);

            _streetList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _streetList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
            };
            _streetList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Streets");

            // Infrastructure
            _consumerList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Consumers"),
                true, true, true, true);
            _consumerList.elementHeight = 50;
            _consumerList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _consumerList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 210, rect.y - 7, 150, 30), "Type");
                EditorGUI.PropertyField(new Rect(rect.x + 245, rect.y, 150, 30),
                    element.FindPropertyRelative("Type"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x, rect.y + 23, 150, 30), "House");
                EditorGUI.PropertyField(new Rect(rect.x + 40, rect.y + 30, 150, 30),
                    element.FindPropertyRelative("HouseType"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 210, rect.y + 23, 150, 30), "Sign");
                EditorGUI.PropertyField(new Rect(rect.x + 245, rect.y + 30, 150, 30),
                    element.FindPropertyRelative("SignPosition"), GUIContent.none);
            };
            _consumerList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Consumers");

            _supplierList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Suppliers"),
                true, true, true, true);
            _supplierList.elementHeight = 50;
            _supplierList.drawElementCallback = (rect, index, active, focused) => {
                var element = _supplierList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 210, rect.y - 7, 150, 30), "Type");
                EditorGUI.PropertyField(new Rect(rect.x + 245, rect.y, 150, 30),
                    element.FindPropertyRelative("Type"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x, rect.y + 23, 150, 30), "Amount");
                EditorGUI.PropertyField(new Rect(rect.x + 50, rect.y + 30, 140, 17),
                    element.FindPropertyRelative("SuppliedItems"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 210, rect.y + 23, 150, 30), "Sign");
                EditorGUI.PropertyField(new Rect(rect.x + 245, rect.y + 30, 150, 30),
                    element.FindPropertyRelative("SignPosition"), GUIContent.none);
            };
            _supplierList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Suppliers");

            _dogList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Dogs"),
                true, true, true, true);
            _dogList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _dogList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
            };
            _dogList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Dogs");

            _policeList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Polices"),
                true, true, true, true);
            _policeList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _policeList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
            };
            _policeList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Police Stations");

            _trafficLightList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("TrafficLights"),
                true, true, true, true);
            _trafficLightList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = _trafficLightList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, 30),
                    element.FindPropertyRelative("Position"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x + 170, rect.y - 7, 50, 30), "Status");
                EditorGUI.PropertyField(new Rect(rect.x + 210, rect.y, 150, 30),
                    element.FindPropertyRelative("StartStatus"), GUIContent.none);
            };
            _trafficLightList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Traffic Lights");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LevelData data = (LevelData)target;
            EditorGUILayout.LabelField("Basic Information");
            EditorGUILayout.Separator();
            data.level = EditorGUILayout.IntField("Level", data.level);
            data.startActionsLeft = EditorGUILayout.IntField("Available Actions", data.startActionsLeft);
            data.moneyNeeded = EditorGUILayout.IntField("Money Needed", data.moneyNeeded);
            data.playerStartPosition = EditorGUILayout.Vector2IntField("Player Position", data.playerStartPosition);

            EditorGUILayout.Separator();
            _showTexts = EditorGUILayout.Foldout(_showTexts, "Text Stuff");
            if (_showTexts)
            {
                _textsList.DoLayoutList();
            }

            EditorGUILayout.Separator();
            _showLandscape = EditorGUILayout.Foldout(_showLandscape, "Landscape");
            if (_showLandscape)
            {
                data.LevelDimensions = EditorGUILayout.Vector2IntField("Level Dimensions", data.LevelDimensions);
                EditorGUILayout.Separator();
                _riverList.DoLayoutList();
                _bridgeList.DoLayoutList();
                _streetList.DoLayoutList();
            }

            EditorGUILayout.Separator();
            _showInfrastructure = EditorGUILayout.Foldout(_showInfrastructure, "Infrastructure");
            if (_showInfrastructure)
            {
                _supplierList.DoLayoutList();
                _consumerList.DoLayoutList();
                _dogList.DoLayoutList();
                _trafficLightList.DoLayoutList();
                _policeList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}