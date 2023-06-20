using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using TsuyoshiLibrary;

namespace TsuyoshiLibrary
{
    public class CustomTagCandiudatePopup : PopupWindowContent
    {
        static private GUIStyle _labelStyle;
        static CustomTagCandiudatePopup()
        {
            _labelStyle = new GUIStyle(EditorStyles.label);
            _labelStyle.richText = true;
        }
        private string _currentText;
        private List<string> _candidates;
        SerializedProperty _tag;
        float _width;

        public CustomTagCandiudatePopup(string currentText)
        {
            if (currentText == null || currentText.Length == 0)
            {
                this._currentText = "";
                _candidates = new List<string>();
            }
            else
            {
                this._currentText = currentText;
                _candidates = CustomTagRepository.GetCandidates(currentText);
            }
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(_width, EditorGUIUtility.singleLineHeight * _candidates.Count);
        }

        /// <summary>
        /// GUI•`‰æ
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            Rect line = new Rect(rect);
            line.height = EditorGUIUtility.singleLineHeight;
            foreach (var t in _candidates)
            {
                var txt = _currentText.Length == 0 ? t : t.Replace(_currentText, $"<color=cyan>{_currentText}</color>");
                EditorGUI.LabelField(line, new GUIContent(txt), _labelStyle);

                if (Event.current.type == EventType.MouseDown && line.Contains(Event.current.mousePosition))
                {
                    _tag.stringValue= t;
                    editorWindow.Close();
                }

                line.y += EditorGUIUtility.singleLineHeight;
            }
        }

        public void OpenModeless(SerializedProperty tag, Rect position)
        {
            this._tag = tag;
            _width = position.width;
            // PopupWindow.Init(activatorRect, windowContent, locationPriorityOrder, showMode, giveFocus: true);
            var method = typeof(PopupWindow).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);

            PopupWindow popWin = ScriptableObject.CreateInstance<PopupWindow>();
            if (popWin != null)
            {
                var showMode = Enum.Parse(method.GetParameters()[3].ParameterType, "PopupMenu");
                method.Invoke(popWin, new object[] { position, this, null, showMode, false });
            }

        }
    }
}

