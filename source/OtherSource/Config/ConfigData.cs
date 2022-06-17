#region �w�b�_�R�����g
// ConfigData.cs
// �ݒ�f�[�^
//
// 2021/04/01 : �O��D�l
#endregion


using UnityEngine;


namespace Mikami
{
	[CreateAssetMenu(menuName = "ScriptableObject/ConfigData", fileName = "ConfigData")]
	public class ConfigData : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField, Range(0f, 1f)] private float initialMasterVolume;
		[SerializeField, Range(0f, 1f)] private float initialBGMVolume;
		[SerializeField, Range(0f, 1f)] private float initialSEVolume;

		[System.NonSerialized] public float masterVolume;
		[System.NonSerialized] public float bgmVolume;
		[System.NonSerialized] public float seVolume;

		public float InitialMasterVolume { get => initialMasterVolume; private set => initialMasterVolume = value; }
		public float InitialBGMVolume	 { get => initialBGMVolume; private set => initialBGMVolume = value; }
		public float InitialSEVolume	{ get => initialSEVolume; private set => initialSEVolume = value; }

		public void OnAfterDeserialize()
		{
			masterVolume = InitialMasterVolume;
			bgmVolume = InitialBGMVolume;
			seVolume = InitialSEVolume;
		}

		public void OnBeforeSerialize()
		{

		}
	}
}