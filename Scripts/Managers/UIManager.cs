using UnityEngine;
using UnityEngine.UI;

public class UIManager:Singleton<UIManager>{
    public GameObject thermometer;//温度計
    
    [SerializeField]Slider _thermometerSlider;//温度計のスライダ

    protected override void Awake(){
        Debugger.Log("Thermometer Awake()");
        base.Awake();
    }

    //スライダを更新
    public void UpdateSlider(float newTemp){
        _thermometerSlider.value=newTemp;
    }
}