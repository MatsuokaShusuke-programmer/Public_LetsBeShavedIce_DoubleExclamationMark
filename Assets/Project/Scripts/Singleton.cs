using UnityEngine;

/*ジェネリックなシングルトンクラスを定義
 <T>はジェネリック型のパラメータで、このクラスを継承する具体的な型を表す
 MonoBehaviourを継承しているので、Unityのコンポーネントとして使用可能
 where T:Singleton<T>は、Tの型制約
 TはSingleton<T>を継承しなければならない*/
public class Singleton<T>:MonoBehaviour where T:Singleton<T>{
    static T _ins;//このクラスの唯一のインスタンスを保持する静的フィ―ルド
    
    /*インスタンスにアクセスするためのプロパティ
     外部からはこのプロパティを通じてのみインスタンスにアクセス可能*/
    public static T ins{get {return _ins;}}
    
    //virtualキーワードにより、派生クラスでオーバーライド可能
    protected virtual void Awake(){
        //インスタンスが存在しているかつ、現在のゲームオブジェクトが存在しているとき
        if (_ins!=null&&gameObject!=null){
            //現在のゲームオブジェクトを破棄(重複を防ぐため)
            Destroy(gameObject);
        }
        else{
            _ins=(T)this;//現在のインスタンスを設定
        }
        //このゲームオブジェクトがをシーン遷移時に破棄されないようにする
        DontDestroyOnLoad(gameObject);
    }
}