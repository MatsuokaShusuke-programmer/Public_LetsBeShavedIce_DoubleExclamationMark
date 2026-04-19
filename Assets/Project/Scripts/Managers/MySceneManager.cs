using UnityEngine.SceneManagement;

public class MySceneManager:Singleton<MySceneManager>{
    public enum SceneType{
        Init,//初期値
        Title,
        Stage,
        Other//その他
    }
    
    public int titleSceneNum;
    public int[] stageSceneNums;
    
    [System.NonSerialized]public SceneType currentSceneType;
    [System.NonSerialized]public SceneType previousSceneType=SceneType.Init;
    [System.NonSerialized]public int currentSceneNum;
    [System.NonSerialized]public int previousSceneNum;//前のシーン

    protected override void Awake(){
        base.Awake();
        
        currentSceneNum=SceneManager.GetActiveScene().buildIndex;
        UpdateCurrentSceneType();//現在のシーンタイプを更新
    }

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){}

    //シーン遷移
    public void ChangeScene(int nextSceneNum){
        SceneManager.LoadScene(nextSceneNum);   //シーン遷移
        previousSceneNum   =currentSceneNum;    //_previousSceneNum(前のシーン)を更新
        currentSceneNum    =nextSceneNum;       //_currentSceneNumを更新
        UpdateCurrentSceneType();               //現在のシーンタイプを更新
        //前のシーンタイプと現在のシーンタイプが同じでないとき、BGMを切り換えて流す
        if (currentSceneType!=previousSceneType&&
            (currentSceneType!=AudioManager.ins.sceneTypeForOtherBGM||
             previousSceneType!=SceneType.Other)&&
            (currentSceneType!=SceneType.Other||
             previousSceneType!=AudioManager.ins.sceneTypeForOtherBGM)){
            /*
            !a*!(b*c+d*e)
            =!a*!(b*c)*!(d*e)
            =!a+(!b+!c)*(!d+!e)
            */
            Debugger.Log("aaa");
            AudioManager.ins.PlayBGM();
        }

        //ステージにシーンのとき温度計をアクティブにし、それ以外では非アクティブにする
        if (currentSceneType==SceneType.Stage){
            UIManager.ins.thermometer.SetActive(true);
        }
        else{
            UIManager.ins.thermometer.SetActive(false);
        }
    }
    
    //前のシーンに戻る
    public void ReturnToPreviousScene(){
        ChangeScene(previousSceneNum);
    }
    
    //次のシーンへ移動
    //ステージ1->クリア(現在)ならステージ2へ
    public void NextScene(){
        ChangeScene(previousSceneNum + 1);
    }
    
    //現在のシーンがどのタイプかを返すメソッド
    void UpdateCurrentSceneType(){
        previousSceneType = currentSceneType;//前のシーンタイプを更新
        
        //タイトルシーンのとき、現在のシーンタイプをタイトルにする
        if (titleSceneNum==currentSceneNum){
            currentSceneType = SceneType.Title;
            return;
        }
        
        //ステージシーンのとき、現在のシーンタイプをステージにする
        foreach (int sceneNum in stageSceneNums){
            if (sceneNum==currentSceneNum){
                currentSceneType = SceneType.Stage;
                return;
            }
        }
        
        //どれにも当てはまらないとき、その他にする
        currentSceneType = SceneType.Other;
    }
}