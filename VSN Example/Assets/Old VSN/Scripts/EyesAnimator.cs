using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EyesAnimator : MonoBehaviour {

  public Sprite[] anim;
  public float clock;
  
  void Start () {
    ResetAnim();
  }
  
  void Update () {

    if(clock > 0)
      clock -= Time.deltaTime;
    else {
      StartCoroutine("Blink");
      ResetAnim();
    }
  }
  
  IEnumerator Blink(){
    float blink_time = 0.15f;
    GetComponent<Image>().sprite = anim[0];
    yield return new WaitForSeconds(blink_time);
    GetComponent<Image>().sprite = anim[1];
  }

  
  public void SetAnim(Sprite[] new_anim){
    anim = new_anim;

    if(anim!=null){
      GetComponent<Image>().sprite = anim[1];
      //      GetComponent<Image>().SetNativeSize();
      gameObject.SetActive(true);
    }else{
      gameObject.SetActive(false);
    }
  }
  
  public void StartAnim(){
    
    ResetAnim();
    
    if(anim!=null){
      GetComponent<Image>().sprite = anim[1];
      //      GetComponent<Image>().SetNativeSize();
    }
  }
  
  public void StopAnim(){
    
    if(anim!=null){
      GetComponent<Image>().sprite = anim[1];
//      GetComponent<Image>().SetNativeSize();
    }
  }
  
  void ResetAnim(){
    clock = Random.Range(4, 9);
  }
}
