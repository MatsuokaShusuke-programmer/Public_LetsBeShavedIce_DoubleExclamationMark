using UnityEngine;
using System.Diagnostics;

//デバッグ用
public static class Debugger{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message){
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message){
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object message){
        UnityEngine.Debug.LogError(message);
    }
}

public class GameManager:Singleton<GameManager>{
    [SerializeField]
    enum GameScenes{ }

    [SerializeField]int _gameOverSceneNum;
    [SerializeField]int _clearSceneNum;
    [SerializeField]int _allClearSceneNum;
    [SerializeField]int _endStageSceneNum;

    protected override void Awake(){
        base.Awake();
    }

    void Update(){
        DebugLoadCleaScene();
    }

    public void GameOver(){
        MySceneManager.ins.ChangeScene(_gameOverSceneNum);
        Destroy(Player.ins.gameObject);
    }

    public void Clear(){
        //最後のステージでないとき
        if (MySceneManager.ins.currentSceneNum!=_endStageSceneNum){
            MySceneManager.ins.ChangeScene(_clearSceneNum);
            Destroy(Player.ins.gameObject);
        }
        else{
            MySceneManager.ins.ChangeScene(_allClearSceneNum);
            Destroy(Player.ins.gameObject);
        }
    }

    //ゲーム終了
    public void Quit(){
//UnityEditorでプレイしているとき
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;//ゲーム終了
#else
        Application.Quit();
#endif
    }

    //Debug用
    //cを押したときクリアシーンへ
    void DebugLoadCleaScene(){
        if (Input.GetKeyDown(KeyCode.C)){
            Clear();
        }
    }
}