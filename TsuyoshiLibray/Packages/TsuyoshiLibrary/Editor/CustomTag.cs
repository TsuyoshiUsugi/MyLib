using System;
using System.Diagnostics;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �C���X�y�N�^�[�Ɍ��Ƃ��ĕ\���\�ȃ^�O�̃N���X
    /// </summary>
    [Serializable, DebuggerDisplay("{ToString()}")] //DebuggerDisplay��VS�Ńf�o�b�O���Ɏw�肵�����̂��Ăяo����
    public class CustomTag
    {
        public string Tag;

        public CustomTag(string str)
        {
            Tag = str;
        }

        public override string ToString()
        {
            return Tag;
        }

        /// <summary>
        /// CustomTag�I�u�W�F�N�g����string�̈ÖٓI�ϊ����s���L���X�g���Z�q�̃I�[�o�[���[�h
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator string(CustomTag obj) //implicit�ňÖٓI�ȕϊ����\�ɂ���
        {
            return obj.Tag;
        }

        /// <summary>
        /// string��CustomTag�I�u�W�F�N�g�̈ÖٓI�Ԋ҂��s���L���X�g���Z�q�̃I�[�o�[���[�h
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator CustomTag(string str)
        {
            return new CustomTag(str);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CustomTag;
            if (other == null) return false;
            return Tag.Equals(other.Tag);
        }

        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }
    }
}
