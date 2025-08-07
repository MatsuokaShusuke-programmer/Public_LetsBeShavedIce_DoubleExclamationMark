using UnityEngine;
using UnityEngine.Audio;

public class AudioManager:Singleton<AudioManager>{
    //その他シーンタイプ用のBGMに対応するシーンタイプ
    public MySceneManager.SceneType sceneTypeForOtherBGM;
    
    [SerializeField]AudioClip[] _bgmAudioClips; 
    [SerializeField]AudioClip[] _seAudioClips;  
    
    [SerializeField]AudioSource _bgmAudioSource;
    [SerializeField]AudioSource _seAudioSource; 
    
    [SerializeField]AudioMixerGroup _bgmAudioMixerGroup;
    [SerializeField]AudioMixerGroup _seAudioMixerGroup;

    //タイトルシーンのBGMの_bgmAudioClipsの要素番号
    [SerializeField]int _titleSceneBGMNum;
    //ステージシーンのBGMの_bgmAudioClipsの要素番号
    [SerializeField]int _stageSceneBGMNum;
    [SerializeField]int _defaultBGMNum;//デフォルトのBGMの_bgmAudioClipsの要素番号
    
    [SerializeField]float[] _bgmVolumes ={1f};
    [SerializeField]float[] _seVolumes  ={1f};
    
    // Start is called before the first frame update
    void Start(){
        PlayBGM();
    }

    // Update is called once per frame
    void Update(){ }
    
    //BGMの再生
    public void PlayBGM(){
        Debugger.Log("PlayBGM");
        UpdateBGMClip();
        _bgmAudioSource.Play();
    }

    //SEを鳴らす
    public void PlaySE(){
        _seAudioSource.PlayOneShot(_seAudioClips[0]);
    }

    //現在のシーンタイプによってBGMのオーディオクリップを変える
    void UpdateBGMClip(){
        Debugger.Log(MySceneManager.ins.previousSceneType.ToString());
        
        //前のシーンと現在のシーンのタイプが同じのときリターン
        if (MySceneManager.ins.currentSceneType==MySceneManager.ins.previousSceneType)return;
        //片方がその他シーンタイプで、もう片方がその他シーンタイプのBGMに対応するシーンタイプのときリターン
        if (MySceneManager.ins.currentSceneType==sceneTypeForOtherBGM&&
            MySceneManager.ins.previousSceneType==MySceneManager.SceneType.Other||
            MySceneManager.ins.currentSceneType==MySceneManager.SceneType.Other&&
            MySceneManager.ins.previousSceneType==sceneTypeForOtherBGM){
            Debugger.Log("その他==ステージ");
            return;
        }

        switch(MySceneManager.ins.currentSceneType){
            //タイトルシーン
            case MySceneManager.SceneType.Title:
                _bgmAudioSource.clip=_bgmAudioClips[_titleSceneBGMNum];
                _bgmAudioSource.volume=_bgmVolumes[_titleSceneBGMNum];
                break;

            //ステージ
            case MySceneManager.SceneType.Stage:
                _bgmAudioSource.clip=_bgmAudioClips[_stageSceneBGMNum];
                _bgmAudioSource.volume=_bgmVolumes[_stageSceneBGMNum];
                break;

            default:
                _bgmAudioSource.clip=_bgmAudioClips[_defaultBGMNum];
                _bgmAudioSource.volume=_bgmVolumes[_defaultBGMNum];
                break;
        }
    }
}