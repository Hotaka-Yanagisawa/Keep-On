#region ヘッダコメント
// FadeImage.cs
// フェード画像設定クラス
//
// 2021.03.23 : 三上優斗
#endregion


using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Mikami
{
    /// <summary>
    /// フェード画像設定クラス
    /// </summary>
    public class FadeImage : UnityEngine.UI.Graphic, IFade
    {
        #region 変数宣言部
        [SerializeField]
        private Texture texture = null;
        [SerializeField]
        private Texture maskTexture = null;
        [SerializeField, Range(0, 1)]
        private float cutoutRange;
        public override Texture mainTexture { get; }
        public float Range
        {
            get
            {
                return cutoutRange;
            }
            set
            {
                cutoutRange = value;
                UpdateMaskCutout(cutoutRange);
            }
        }
        #endregion


        #region パブリック関数実装部
        public void UpdateMainTexture(Texture texture)
        {
            material.SetTexture("_MainTex", texture);
        }

        public void UpdateMaskTexture(Texture texture)
        {
            material.SetTexture("_MaskTex", texture);
            material.SetColor("_Color", color);
        }
        #endregion


        #region プライベート関数実装部
        private void UpdateMaskCutout(float range)
        {
            enabled = true;
            material.SetFloat("_Range", 1 - range);

            if (range <= 0)
            {
                this.enabled = false;
            }
        }
        #endregion


        #region プロテクト関数実装部
        protected override void Start()
        {
            base.Start();
            UpdateMainTexture(texture);
            UpdateMaskTexture(maskTexture);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateMaskCutout(Range);
            UpdateMaskTexture(maskTexture);
            UpdateMainTexture(texture);
        }
#endif
        #endregion
    }
}