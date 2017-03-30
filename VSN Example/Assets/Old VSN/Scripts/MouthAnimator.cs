using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouthAnimator : MonoBehaviour {
	
	public Sprite[] anim;
	public float clock;
	public int index;
	public bool is_animating;
	private float mouthFrameTime = 0.13f;
  private int numFrames = 6;
	
	void Start () {
		ResetAnim();
	}

	void Update () {
		if( is_animating ){
			clock += Time.deltaTime;

			if( clock>mouthFrameTime ){
				index+=1;
        index = CapInt(index, numFrames);
				GetComponent<Image>().sprite = anim[index];
//				GetComponent<Image>().SetNativeSize();
				clock -= mouthFrameTime;
			}
		}
	}

	public void SetAnim(Sprite[] new_anim){
//		ResetAnim();
		anim = new_anim;

//		if( anim==null ){
//			canAnimate = false;
//			gameObject.SetActive(false);
//		}
//		else{
//			StartAnim();
//		}
		if(anim!=null){
			GetComponent<Image>().sprite = anim[0];
//			GetComponent<Image>().SetNativeSize();
			gameObject.SetActive(true);
		}else{
			gameObject.SetActive(false);
		}
	}

	public void StartAnim(){
		//character.mouthObject.SetActive(true);

		ResetAnim();

		if(anim!=null){
			is_animating = true;
			GetComponent<Image>().sprite = anim[0];
//			GetComponent<Image>().SetNativeSize();
		}else{
			is_animating = false;
		}
	}

	public void StopAnim(){
		//character.mouthObject.SetActive(false);

		if(is_animating){
			GetComponent<Image>().sprite = anim[0];
//			GetComponent<Image>().SetNativeSize();
			is_animating = false;
		}else{
			// do nothing
		}
	}

	void ResetAnim(){
		index = 0;
		clock = 0f;
	}

	int CapInt(int num, int capAt){
		if( num>=capAt )
			return 0;
		else
			return num;
	}

}
