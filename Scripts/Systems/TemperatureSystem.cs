using System;
using System.Collections;
using UnityEngine;

public class TemperatureSystem:Singleton<TemperatureSystem>{
    public float temperatureControl{get;private set;}=60;//温度の調整
    
    // [NonSerialized]public int heatingOrCoolingNum;  //加熱のもしくは冷却中の処理の数
    [NonSerialized]public float temperature;        //温度

    [SerializeField]float _defaultTemperature=30f;//初期の温度

    [SerializeField]float _stopSecond;//処理を止める時間(s)

    int _temperatureZoneCount=0;
    
    bool _canBringTheTemperatureBackUp=true;//温度をもとにもどせる

    protected override void Awake(){
        base.Awake();
    }

    // Start is called before the first frame update
    void Start(){
        temperature=_defaultTemperature;//温度の初期化
        UIManager.ins.UpdateSlider(temperature);
    }

    void Update(){
        ReturnToTemperature();
        
        // Debugger.Log("温度:"+temperature);
    }

    //加熱もしくは冷却中でない場合、デフォルトに戻す
    void ReturnToTemperature(){
        //ヒータなどのコライダの中にいないかつ、温度をデフォルトに戻せるかつ、
        //温度がデフォルトとだいたい同じでないとき
        if(_temperatureZoneCount==0&&_canBringTheTemperatureBackUp&&
           !Mathf.Approximately(temperature,_defaultTemperature)){
            // Debugger.Log("デフォルトに戻す");
            //一定間隔で温度をデフォルトに戻す
            StartCoroutine(ReturnToTemperatureCoroutine());
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        //触れたオブジェクトがヒータなどの場合
        if (other.CompareTag("AnythingThatChangesTemperature")){
            _temperatureZoneCount++;//ヒータなどのコライダに入った
            // Debugger.Log("ヒータなどに入った");
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //離れたオブジェクトがヒータなどの場合
        if (other.CompareTag("AnythingThatChangesTemperature")){
            _temperatureZoneCount--;//ヒータなどのコライダから出た
        }
    }
    
    //一定間隔で温度をデフォルトにもどす
    IEnumerator ReturnToTemperatureCoroutine(){
        _canBringTheTemperatureBackUp=false;                //処理が出来ないようにする
        yield return TemperatureChange(_defaultTemperature);//温度を変える
        _canBringTheTemperatureBackUp=true;                 //処理ができるようにする
    }

    //温度を変える
    public IEnumerator TemperatureChange(float tempTemperature){
        temperature=(temperature+tempTemperature)/2f;   //温度の変更
        UIManager.ins.UpdateSlider(temperature);        //スライダの更新

        yield return new WaitForSeconds(_stopSecond);
    }
}