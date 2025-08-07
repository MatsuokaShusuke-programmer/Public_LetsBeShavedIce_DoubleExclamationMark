using UnityEngine;

public class CameraManager:MonoBehaviour{
    //上下左右の、カメラに映る座標の限界値
    [SerializeField]float _topLimitPos;
    [SerializeField]float _bottomLimitPos;
    [SerializeField]float _leftLimitPos;
    [SerializeField]float _rightLimitPos;
    
    [SerializeField]bool _isScreenBounded;//領域内にスクリーンを収めるか、カメラを収めるか
    
    Camera      _camera;
    Transform   _cameraTransform;
    Vector3     _pos;       //カメラの座標

    float _halfHeight;  //カメラの縦幅の半分
    float _halfWidth;   //カメラの横幅の半分

    void Awake(){
        _camera=GetComponent<Camera>();//カメラの取得
        _cameraTransform=transform;//カメラのトランスフォームを代入
    }
    
    // Start is called before the first frame update
    void Start(){
        //領域内にスクリーンを収めるとき
        if (_isScreenBounded){
            _halfHeight=_camera.orthographicSize;

            _halfWidth=_halfHeight*_camera.aspect;

            //カメラの動ける範囲を求める
            _topLimitPos-=_halfHeight;
            _bottomLimitPos+=_halfHeight;
            _leftLimitPos+=_halfWidth;
            _rightLimitPos-=_halfWidth;
        }
    }
    
    void LateUpdate(){
        //プレイヤがあるとき
        if (!Player.ins) return;
        
        //カメラの座標を範囲内でプレイヤに合わせる
        _pos=_cameraTransform.position;//カメラの座標を代入

        //プレイヤの座標を代入
        _pos.x=Player.ins.transform.position.x;
        _pos.y=Player.ins.transform.position.y;

        //座標のクランプ
        _pos.x=Mathf.Clamp(_pos.x, _leftLimitPos, _rightLimitPos);
        _pos.y=Mathf.Clamp(_pos.y, _bottomLimitPos, _topLimitPos);

        _cameraTransform.position=_pos;//カメラの移動
    }
}