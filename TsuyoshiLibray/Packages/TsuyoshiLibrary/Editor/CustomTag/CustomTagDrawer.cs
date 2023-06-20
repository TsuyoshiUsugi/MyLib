using UnityEngine;
using UnityEditor;
using TsuyoshiLibrary;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// Control Editor of SkillCategoryTagCollection
    /// </summary>
    [CustomPropertyDrawer(typeof(CustomTag), true)]  //プロパティとして定義されたクラスのインスペクタ上の表現を変える
    public class CustomTagDrawer : PropertyDrawer
    {
        private CustomTagCandiudatePopup toolTip;
        static private GUIContent registButton = new GUIContent("Regist", "このタグをリポジトリに登録"); //何をレンダリングするか定義する
        static private GUIStyle attentionStyle;
        private int index = -1;
        static CustomTagDrawer()
        {
            attentionStyle = new GUIStyle(EditorStyles.textField);
            var orange = new Color(0.9f, 0.4f, 0);
            attentionStyle.normal.textColor = orange;
            attentionStyle.focused.textColor = orange;
            attentionStyle.hover.textColor = orange;
        }

        private string lastControlForPopup;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null || property.serializedObject == null) return;

            // フォーカス制御のため名前をつける
            string control_name = "Tag" + GUIUtility.GetControlID(FocusType.Keyboard).ToString();
            GUI.SetNextControlName(control_name);

            var tag = GetChildProperty(property, "Tag");
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                //　左側に表示するボタンの領域
                var buttonArea = new Rect(position);
                buttonArea.width = EditorStyles.toolbarButton.CalcSize(registButton).x;

                // メインのTextFieldの領域
                var mainArea = new Rect(position);
                var shift = buttonArea.width + EditorGUIUtility.standardVerticalSpacing;
                mainArea.x += shift;
                mainArea.width -= shift;

                // 既存のタグに一致するかどうか
                var contains = tag.stringValue.Length == 0 || CustomTagRepository.Contains(tag.stringValue);
                // テキストフィールドの描画、既存のタグかどうかでスタイル切り替え
                tag.stringValue = EditorGUI.TextField(mainArea, tag.stringValue, contains ? EditorStyles.textField : attentionStyle);

                string focused_name = GUI.GetNameOfFocusedControl();
                if (check.changed)
                {
                    if (tag.stringValue.Length != 0)
                    {
                        // 候補のツールチップ表示
                        var popRect = new Rect(position);
                        toolTip = new CustomTagCandiudatePopup(tag.stringValue);
                        toolTip.OpenModeless(popRect);
                        // フォーカスを再設定する
                        GUI.FocusControl(focused_name);
                        lastControlForPopup = focused_name;
                    }
                }
                else if (toolTip != null && focused_name != lastControlForPopup)
                {
                    // 不要なツールチップを消す
                    toolTip.editorWindow.Close();
                }

                // 未登録のタグなら、登録ボタンを出し、押したらリポジトリに登録できるようにする。
                if (!contains && GUI.Button(buttonArea, registButton))
                {
                    CustomTagRepository.Instance.AddNewTag(tag.stringValue);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }
    }
}
