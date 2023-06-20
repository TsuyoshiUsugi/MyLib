using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TsuyoshiLibrary
{
    [CreateAssetMenu(menuName = "TsuyoshiLibrary/Tag/CustomTagRepository")]
    /// <summary>
    /// CustomTag のマスターリポジトリ
    /// 二つ以上存在出来ないようにsingletonなクラスになっている
    /// </summary>
    public class CustomTagRepository : ScriptableObject
    {
        [SerializeField, Header("このファイルのPath")] public static string StaticAssetDataPath;

        private static CustomTagRepository _repository = null;

        public static CustomTagRepository Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_repository == null)
                {
                    _repository =
                        AssetDatabase.LoadAssetAtPath<CustomTagRepository>(
                            StaticAssetDataPath);
                }
#endif
                return _repository;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 現在登録されている全てのタグを取ってくる
        /// </summary>
        /// <param name="currentText"></param>
        /// <returns></returns>
        public static List<string> GetCandidates(string currentText)
        {
            return Instance.GetAllTags().Where(t => t.IndexOf(currentText) != -1).ToList();
        }

        /// <summary>
        /// AssetDataPathをロードする
        /// </summary>
        public static void SetAssetDataPath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StaticAssetDataPath = assembly.Location;
        }

#endif

        [SerializeField, ReadOnly]
        private List<string> _tags = new List<string>();

        public static bool Contains(string tag)
        {
            return Instance._tags.Contains(tag);
        }

        public void AddNewTag(string name)
        {
            if (_tags.Contains(name)) return;
            _tags.Add(name);
        }

        public IEnumerable<string> GetAllTags()
        {
            return _tags;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// タグのリポジトリのパスを変更する
    /// </summary>
    public class CustomAssetProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (!path.Contains(".asset")) return;
                if (path.Contains("CustomTagRepository")) return;

                CustomTagRepository.StaticAssetDataPath = path;
                Debug.LogError("CustomTagRepositoryが作られました");
            }

            foreach (var path in movedAssets)
            {
                if (!path.Contains(".asset")) return;
                if (path.Contains("CustomTagRepository")) return;

                CustomTagRepository.StaticAssetDataPath = path;
                Debug.LogError("CustomTagRepositoryの場所が移動されました。");
            }
        }
    }
#endif
}
