using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace TsuyoshiLibrary
{
    public class CustomTagCandiudatePopup : PopupWindowContent
    {
        static private GUIStyle labelStyle;
        static CustomTagCandiudatePopup()
        {
            labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.richText = true;
        }
        private string currentText;
        private List<string> candidates;

        public CustomTagCandiudatePopup(string currentText)
        {
            if (currentText == null || currentText.Length == 0)
            {
                this.currentText = "";
                candidates = new List<string>();
            }
            else
            {
                this.currentText = currentText;
                candidates = CustomTagRepository.GetCandidates(currentText);
            }
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, EditorGUIUtility.singleLineHeight * candidates.Count);
        }

        /// <summary>
        /// GUI•`‰æ
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            Rect line = new Rect(rect);
            line.height = EditorGUIUtility.singleLineHeight;
            foreach (var t in candidates)
            {
                var txt = currentText.Length == 0 ? t : t.Replace(currentText, $"<color=cyan![register.gif](https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/75035/32561793-39f0-8408-207e-148d61fe1589.gif)>{currentText}</ color > ");
                EditorGUI.LabelField(line, new GUIContent(txt), labelStyle);
                line.y += EditorGUIUtility.singleLineHeight;
            }
        }

        public void OpenModeless(Rect position)
        {
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
