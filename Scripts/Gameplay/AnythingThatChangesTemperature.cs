using System.Collections;
using UnityEngine;

public class AnythingThatChangesTemperature:MonoBehaviour{
    [SerializeField]int _temperature;//温度
    
    [SerializeField]float _radius=5f;//コライダの半径
    
    CircleCollider2D _circleCollider2D;

    bool _canVaryTheTemperature =true;  //温度を変化させて良いか

    void Awake(){
        _circleCollider2D=GetComponent<CircleCollider2D>();
    }
    
    // Start is called before the first frame update
    void Start(){
        _circleCollider2D.radius = _radius;//コライダの半径を適用
    }

    // Update is called once per frame
    void Update(){ }

    // void OnTriggerEnter2D(Collider2D other){
    //     if (other.gameObject.CompareTag("Player")){
    //         _playerInRange = true;
    //     }
    // }

    void OnTriggerStay2D(Collider2D other){
        //プレイヤが範囲内に入っているときかつ、処理をしてよいどきヒータの温度を使ってプレイヤの温度を変化
        if (other.CompareTag("Player")&&_canVaryTheTemperature){
            StartCoroutine(TemperatureChangeRoutine());
            // Debugger.Log("温度変化");
        }
    }

    // void OnTriggerExit2D(Collider2D other){
    //     if (other.gameObject.CompareTag("Player")){
    //         _playerInRange = false;
    //     }
    // }

    //一定間隔で温度を変更温度変更
    IEnumerator TemperatureChangeRoutine(){
        _canVaryTheTemperature=false;//処理を出来ないようにする

        // TemperatureSystem.ins.heatingOrCoolingNum++;//処理中
        
        //温度を変更
        yield return TemperatureSystem.ins.TemperatureChange(_temperature);
        
        //上の実行終了後に実行
        _canVaryTheTemperature=true;                //処理ができるようにする
        // TemperatureSystem.ins.heatingOrCoolingNum--;//処理終了
        
    }

    //ギズモ(シーンビュー、ゲームビューに表示されるグラフィック)を表示
    void OnDrawGizmos(){
        //ギズモのマトリックスにワールド座標を代入
        Gizmos.matrix=transform.localToWorldMatrix;

        //ギズモの色を設定(シアン)
        Gizmos.color=Color.cyan;
        
        //円を表示
        Gizmos.DrawWireSphere(Vector3.zero,_radius);
    }
}