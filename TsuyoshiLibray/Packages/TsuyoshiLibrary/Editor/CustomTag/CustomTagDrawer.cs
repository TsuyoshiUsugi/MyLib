using UnityEngine;
using UnityEditor;
using TsuyoshiLibrary;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// Control Editor of SkillCategoryTagCollection
    /// </summary>
    [CustomPropertyDrawer(typeof(CustomTag), true)]  //�v���p�e�B�Ƃ��Ē�`���ꂽ�N���X�̃C���X�y�N�^��̕\����ς���
    public class CustomTagDrawer : PropertyDrawer
    {
        private CustomTagCandiudatePopup toolTip;
        static private GUIContent registButton = new GUIContent("Regist", "���̃^�O�����|�W�g���ɓo�^"); //���������_�����O���邩��`����
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

            // �t�H�[�J�X����̂��ߖ��O������
            string control_name = "Tag" + GUIUtility.GetControlID(FocusType.Keyboard).ToString();
            GUI.SetNextControlName(control_name);

            var tag = GetChildProperty(property, "Tag");
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                //�@�����ɕ\������{�^���̗̈�
                var buttonArea = new Rect(position);
                buttonArea.width = EditorStyles.toolbarButton.CalcSize(registButton).x;

                // ���C����TextField�̗̈�
                var mainArea = new Rect(position);
                var shift = buttonArea.width + EditorGUIUtility.standardVerticalSpacing;
                mainArea.x += shift;
                mainArea.width -= shift;

                // �����̃^�O�Ɉ�v���邩�ǂ���
                var contains = tag.stringValue.Length == 0 || CustomTagRepository.Contains(tag.stringValue);
                // �e�L�X�g�t�B�[���h�̕`��A�����̃^�O���ǂ����ŃX�^�C���؂�ւ�
                tag.stringValue = EditorGUI.TextField(mainArea, tag.stringValue, contains ? EditorStyles.textField : attentionStyle);

                string focused_name = GUI.GetNameOfFocusedControl();
                if (check.changed)
                {
                    if (tag.stringValue.Length != 0)
                    {
                        // ���̃c�[���`�b�v�\��
                        var popRect = new Rect(position);
                        toolTip = new CustomTagCandiudatePopup(tag.stringValue);
                        toolTip.OpenModeless(popRect);
                        // �t�H�[�J�X���Đݒ肷��
                        GUI.FocusControl(focused_name);
                        lastControlForPopup = focused_name;
                    }
                }
                else if (toolTip != null && focused_name != lastControlForPopup)
                {
                    // �s�v�ȃc�[���`�b�v������
                    toolTip.editorWindow.Close();
                }

                // ���o�^�̃^�O�Ȃ�A�o�^�{�^�����o���A�������烊�|�W�g���ɓo�^�ł���悤�ɂ���B
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
