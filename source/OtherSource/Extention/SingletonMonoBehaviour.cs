// SingletonMonoBehaviour.cs
// シングルトン化クラス。参考元URL→https://qiita.com/okuhiiro/items/3d69c602b8538c04a479
//
// 2021.03.15 : 三上優斗


using System;
using UnityEngine;


/// <summary>
/// シングルトン化するクラス
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
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }


    virtual protected void Awake()
    {
        // 他のGameObjectにアタッチされているか調べる.
        // アタッチされている場合は破棄する.
        if (this != Instance)
        {
            Destroy(this);
            Debug.LogError(
                typeof(T) +
                " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}
