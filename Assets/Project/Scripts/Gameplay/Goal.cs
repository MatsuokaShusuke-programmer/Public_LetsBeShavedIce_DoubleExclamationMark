using UnityEngine;

public class Goal:MonoBehaviour{
    void OnTriggerEnter2D(Collider2D other){
        //当たったのがプレイヤのとき
        if (other.gameObject.CompareTag("Player")){
            Debugger.Log("Goal");

            GameManager.ins.Clear();//クリアシーンへ
        }
    }
}