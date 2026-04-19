using UnityEngine;

/*PlayerクラスをSingleton<Playerから継承させる
 これにより、Playerはシングルトンパターンを実装し、
 ゲーム内で唯一のインスタンスを持つことが保証される*/
public class Player:Singleton<Player>{
    [SerializeField]BoxCollider2D _solidBoxCollider2D;  //固体のボックスコライダー
    [SerializeField]BoxCollider2D _liquidBoxCollider2D; //液体のボックスコライダー
    
    [SerializeField]Sprite _solidSprite; //固体
    [SerializeField]Sprite _liquidSprite;//液体
    
    [SerializeField]float   _speed          =10f;
    [SerializeField]float   _dash           =2f;
    [SerializeField]float   _jump           =10f;
    [SerializeField]float   _airMobile      =0.5f;
    [SerializeField]float   _maxHp          =100f;
    [SerializeField]float   _meltingPoint   =50;    //融点
    [SerializeField]float   _boilingPoint   =0;     //沸点
    
    Rigidbody2D     _rb;
    SpriteRenderer  _sr;
    
    float _hp;

    bool _isGround;

    //キーボード入力
    //押された瞬間
    bool _isSpaceDown;
    //押されている
    bool _isLeftShift;

    /*Awakeのメソッドをオーバーライドする
     protectedはこのメソッドがこのクラスとその派生クラスからのみ
     アクセス可能であることを示す
     overrideは基底クラス(Singleton<Player>)の同盟メソッドを上書きすることを示す*/
    protected override void Awake(){
        /*基底クラスのアウェイクメソッドを呼び出す
         base.メソッド名()で親クラスのメソッドを呼び出せます*/
        base.Awake();
        
        _rb=GetComponent<Rigidbody2D>();
        _sr=GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start(){
        _sr.sprite=_solidSprite;//画像を固体にする
        
        //固体のボックスコライダーを有効にし、液体のボックスコライダーを無効にする
        _solidBoxCollider2D.enabled =true;
        _liquidBoxCollider2D.enabled=false;
        
        _hp=_maxHp;
    }

    // Update is called once per frame
    void Update(){
        //キー入力
        //押された瞬間
        _isSpaceDown=Input.GetKeyDown(KeyCode.Space);
        //押されている
        _isLeftShift=Input.GetKey(KeyCode.LeftShift);
        
        Jump();
    }

    void FixedUpdate() {
        Move();
        TheThreePhasesOfMatter();
    }

    //移動
    void Move(){
        float x=Input.GetAxis("Horizontal");//左右の値を取得

        //力を加える
        //ダッシュ時は*dash、空中では*airMobile
        _rb.AddForce(new Vector2(x,0) * (_speed * (_isLeftShift?_dash:1) * (_isGround?1:_airMobile)));
    }
    
    //ジャンプ
    void Jump(){
        //地面についているときにスペースが押されたとき
        if (_isSpaceDown && _isGround){
            _rb.AddForce(new Vector2(0,_jump));
            AudioManager.ins.PlaySE();//SEの再生
        }
    }
    
    //三態のシステム
    void TheThreePhasesOfMatter(){
        //hpを気温を温度を調整する値で割った値分引く
        _hp-=TemperatureSystem.ins.temperature/TemperatureSystem.ins.temperatureControl;
        
        if (_hp <= _boilingPoint){
            //GameOver
            GameManager.ins.GameOver();
            Debugger.Log("Game Over");
        }
        else if (_hp <= _meltingPoint){
            _sr.sprite=_liquidSprite;//液体になる
            //液体のボックスコライダーを有効にし、固体のボックスコライダーを無効にする
            _solidBoxCollider2D.enabled =false;
            _liquidBoxCollider2D.enabled=true;
        }
        else if (_hp >= _meltingPoint){
            _sr.sprite=_solidSprite;//固体になる
            
            //固体のボックスコライダーを有効にし、液体のボックスコライダーを無効にする
            _solidBoxCollider2D.enabled =true;
            _liquidBoxCollider2D.enabled=false;
        }
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        //地面に触れたとき
        if (other.gameObject.CompareTag("Ground"))_isGround = true;
    }

    void OnTriggerEnter2D(Collider2D other){
        //死亡するオブジェクトに触れたとき、GameOver
        if(other.gameObject.CompareTag("Death"))GameManager.ins.GameOver();
    }
    
    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) _isGround = false;
    }
}
