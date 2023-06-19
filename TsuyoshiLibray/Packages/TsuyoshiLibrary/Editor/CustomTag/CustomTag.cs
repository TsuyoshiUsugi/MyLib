using System;
using System.Diagnostics;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// インスペクターに候補として表示可能なタグのクラス
    /// </summary>
    [Serializable, DebuggerDisplay("{ToString()}")] //DebuggerDisplayはVSでデバッグ時に指定したものを呼び出せる
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
        /// CustomTagオブジェクト→のstringの暗黙的変換を行うキャスト演算子のオーバーロード
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator string(CustomTag obj) //implicitで暗黙的な変換を可能にする
        {
            return obj.Tag;
        }

        /// <summary>
        /// string→CustomTagオブジェクトの暗黙的返還を行うキャスト演算子のオーバーロード
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
