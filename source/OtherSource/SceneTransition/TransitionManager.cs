#region �w�b�_�R�����g
// TransitioManager.cs
// �V�[���J�ڊǗ��N���X�B�Q�lURL��https://qiita.com/toRisouP/items/1713d2addf6f5dc9f9b8
//
// 2021.03.18 : �O��D�l
#endregion


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mizuno;


namespace Mikami
{
    /// <summary>
    /// �V�[���J�ڊǗ��N���X
    /// </summary>
    public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
    {
        #region �ϐ��錾��
        [SerializeField] Slider bar;
        [SerializeField] RectTransform wheelO;
        [SerializeField] RectTransform pendulumE;
        [SerializeField] RectTransform reachOnE;
        [SerializeField] GameObject loadingPanel;
        private bool autoStartFade = false;
        private Fade fadeComponent;
        public delegate void OnTransitionEventHandler();
        public event OnTransitionEventHandler OnAllSceneLoaded;
        public event OnTransitionEventHandler OnTransitionAnimationStart;
        public event OnTransitionEventHandler OnTransitionAnimationFinished;
        private AsyncOperation loadAsyncOperation;
        #endregion
        

        #region �v���p�e�B
        public bool IsRunning { get; private set; }
        public Scenes CurrentGameScene { get; private set; }
        #endregion


        #region �p�u���b�N�֐�������
        /// <summary>
        /// �V�[���J�ڎ��A�����t�F�[�h��false�ɂ����ꍇ�ɌĂяo���K�v������
        /// </summary>
        public void Open()
        {
            autoStartFade = true;
        }


		/// <summary>
		/// �V�[���J�ڂ����s����
		/// </summary>
		/// <param name="nextScene">���̃V�[��</param>
		/// <param name="data">���̃V�[���ֈ����p���f�[�^</param>
		/// <param name="additiveLoadScenes">�ǉ����[�h����V�[��</param>
		/// <param name="autoMove">�g�����W�V�����̎����J�ڂ��s����</param>
		public void StartTransaction(
			Scenes nextScene,
			//SceneDataPack data,
			Scenes[] additiveLoadScenes = null,
			bool autoMove = true
			)
		{
			if (IsRunning) return;
			StartCoroutine(TransitionCoroutine(nextScene, /*data,*/ additiveLoadScenes, autoMove));
		}
        #endregion


        #region �v���C�x�[�g�֐�������
        private void Start()
        {
            IsRunning = false;
            CurrentGameScene = Scenes.Manager;
            fadeComponent = GetComponent<Fade>();
            fadeComponent.Image.raycastTarget = false;//�^�b�`�C�x���g���W�G�Ńu���N���Ȃ�
            //����������ɃV�[���J�ڊ����ʒm�𔭍s����(�f�o�b�O�ŔC�ӂ̃V�[������Q�[�����J�n�ł���悤��)
            OnAllSceneLoaded?.Invoke();
        }


        #region �R���[�`���֐�
        /// <summary>
        /// �V�[���J�ڏ����̖{��
        /// </summary>
        private IEnumerator TransitionCoroutine(
            Scenes nextScene,
            //SceneDataPack data,
            Scenes[] additiveLoadScenes,
            bool autoMove
            )
        {
            if (IsRunning) yield break;
            //�����J�n�t���O�Z�b�g
            IsRunning = true;

            //�g�����W�V�����J�n�i�W�G�ŉ�ʂ��B���j
            autoStartFade = autoMove;
            fadeComponent.Image.raycastTarget = true;
            fadeComponent.FadeIn(1f);
            SoundManager.Instance.StopBGMWithFade(1f);
            //�g�����W�V�����A�j���[�V�������I������̂�҂�
            yield return new WaitWhile(() => fadeComponent.IsRunning);


            // �W�G���O�����[�hUI��ʂ�������
            fadeComponent.FadeOut(1f);
            loadingPanel.SetActive(true);
            //�g�����W�V�����A�j���[�V�������I������̂�҂�
            yield return new WaitWhile(() => fadeComponent.IsRunning);

            // ���������v���O���X�o�[(���Ԃ𔺂킹�Ă�����ł͏I��邽��
            float progress = 0f;
            while(progress < 0.99f)
            {
                yield return null;
                bar.value = progress;
                wheelO.rotation = Quaternion.Euler(0f, 0f, -Mathf.Repeat(progress * 360, 360));
                pendulumE.rotation = Quaternion.Euler(0f, 0f, 0f + 10f * Mathf.Cos(Mathf.PI * 2f * progress));
                reachOnE.rotation = Quaternion.Euler(0f, 0f, 30f + 10f * Mathf.Cos(Mathf.PI * 2f * progress));
                progress += 0.01f;
            }

            //���C���ƂȂ�V�[����Single�œǂݍ���
            loadAsyncOperation = SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);
            
            while(!loadAsyncOperation.isDone)
            {
                yield return null;
                //bar.value = loadAsyncOperation.progress;
                //wheelO.rotation = Quaternion.Euler(0f, 0f, Mathf.Repeat(bar.value * 1800, 360));
            }
            bar.value = 1f;
            wheelO.rotation = Quaternion.Euler(0f, 0f, 0f);

            //yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            //�ǉ��V�[��������ꍇ�͈ꏏ�ɓǂݍ���
            //if (additiveLoadScenes != null)
            //{
            //    yield return additiveLoadScenes.Select(scene =>
            //        SceneManager.LoadSceneAsync("Scenes/" + scene.ToString(), LoadSceneMode.Additive)
            //        .AsObservable()).WhenAll().ToYieldInstruction();
            //}
            //yield return null;

            //�g���ĂȂ����\�[�X�̉����GC�����s
            Resources.UnloadUnusedAssets();
            GC.Collect();
            yield return null;
            
            CurrentGameScene = nextScene;
            //�V�[�����[�h�̊����ʒm�𔭍s
            OnAllSceneLoaded?.Invoke();
            
            if (!autoMove)
            {
                //�����J�ڂ��Ȃ��ݒ�̏ꍇ�̓t���O��true�ɕω�����܂őҋ@
                yield return new WaitUntil(() => autoStartFade);
            }

            fadeComponent.FadeIn(1f);
            yield return new WaitWhile(() => fadeComponent.IsRunning);
            loadingPanel.SetActive(false);

            //�W�G���J�����̃A�j���[�V�����J�n
            autoStartFade = false;
            fadeComponent.FadeOut(1f);
            OnTransitionAnimationStart?.Invoke();
            //�W�G���J������̂�҂�
            yield return new WaitWhile(() => fadeComponent.IsRunning);
            fadeComponent.Image.raycastTarget = false;

            bar.value = 0f;

            //�g�����W�V�������S�Ċ����������Ƃ�ʒm
            OnTransitionAnimationFinished?.Invoke();
            
            //�I��
            IsRunning = false;
        }
        #endregion
        #endregion
    }
}