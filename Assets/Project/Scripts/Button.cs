using UnityEngine;

public class Button:MonoBehaviour{
    void Start(){
        //MySceneManagerがないとき、警告
        if (!MySceneManager.ins){
            Debugger.LogWarning("MySceneManager instance is null");
        }
    }
    //ボタンを押されたとき、任意のシーンへ行く
    public void OnClickButtonChangeScene(int sceneNum){
        MySceneManager.ins.ChangeScene(sceneNum);
    }
    
    //コンティニュボタンを押されたとき、1つ前のシーンに戻る
    public void OnClickContinueButton(){
        MySceneManager.ins.ReturnToPreviousScene();
    }
    
    //NextButtonを押されたとき、次のステージへ行く
    public void OnClickNextButton(){
        MySceneManager.ins.NextScene();
    }

    //QuitButtonを押されたとき、ゲーム終了
    public void OnClickQuitButton(){
        GameManager.ins.Quit();
    }
}