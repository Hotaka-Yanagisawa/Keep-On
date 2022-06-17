// SingletonMonoBehaviour.cs
// �V���O���g�����N���X�B�Q�l��URL��https://qiita.com/okuhiiro/items/3d69c602b8538c04a479
//
// 2021.03.15 : �O��D�l


using System;
using UnityEngine;


/// <summary>
/// �V���O���g��������N���X
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;


    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " ���A�^�b�`���Ă���GameObject�͂���܂���");
                }
            }

            return instance;
        }
    }


    virtual protected void Awake()
    {
        // ����GameObject�ɃA�^�b�`����Ă��邩���ׂ�.
        // �A�^�b�`����Ă���ꍇ�͔j������.
        if (this != Instance)
        {
            Destroy(this);
            Debug.LogError(
                typeof(T) +
                " �͊��ɑ���GameObject�ɃA�^�b�`����Ă��邽�߁A�R���|�[�l���g��j�����܂���." +
                " �A�^�b�`����Ă���GameObject�� " + Instance.gameObject.name + " �ł�.");
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}
