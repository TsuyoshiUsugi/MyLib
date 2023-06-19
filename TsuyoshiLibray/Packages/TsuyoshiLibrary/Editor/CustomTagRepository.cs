using System.Linq;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "TsuyoshiLibrary/Tag/CustomTagRepository")]
/// <summary>
/// CustomTag �̃}�X�^�[���|�W�g��
/// ��ȏ㑶�ݏo���Ȃ��悤��singleton�ȃN���X�ɂȂ��Ă���
/// </summary>
public class CustomTagRepository : ScriptableObject
{
    [SerializeField, Header("���̃t�@�C����Path")] string _assetDataPath = string.Empty;
    static string _staticAssetDataPath;

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
                        _staticAssetDataPath);
            }
#endif
            return _repository;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// ���ݓo�^����Ă���S�Ẵ^�O������Ă���
    /// </summary>
    /// <param name="currentText"></param>
    /// <returns></returns>
    public static List<string> GetCandidates(string currentText)
    {
        return Instance.GetAllTags().Where(t => t.IndexOf(currentText) != -1).ToList();
    }

    [ExecuteAlways] 
    public void ExecuteAlways()
    {
        _staticAssetDataPath = UnityEngine.Application.dataPath + System.IO.Path.GetFileName(UnityEngine.Application.dataPath);
        Debug.Log(_staticAssetDataPath);
    }

#endif

    [SerializeField, Header("�����Ń^�O����ύX���Ă��g���Ă�^�O�܂ŕύX�ł��܂���B�ύX�͎��ȐӔC�ŁB")]
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
