using UnityEngine;
using System.Collections;
using TMPro;

public class TextConsoleSimulator : MonoBehaviour{
	private TMP_Text m_TextComponent;
	private bool hasTextChanged;

	void Awake (){
		m_TextComponent = gameObject.GetComponent<TMP_Text> ();
	}
		
	void OnEnable (){
		// Subscribe to event fired when text object has been regenerated.
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add (ON_TEXT_CHANGED);
	}

	void OnDisable (){
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove (ON_TEXT_CHANGED);
	}


	// Event received when the text object has changed.
	void ON_TEXT_CHANGED (Object obj){
		hasTextChanged = true;
	}


	/// <summary>
	/// Method revealing the text one character at a time.
	/// </summary>
	/// <returns></returns>
	public IEnumerator RevealCharacters (){
		TMP_Text textComponent = m_TextComponent;			
		textComponent.ForceMeshUpdate ();

		TMP_TextInfo textInfo = textComponent.textInfo;

		int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
		int visibleCount = 0;

		while (true) {
			if (hasTextChanged) {
				totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
				hasTextChanged = false; 
			}

			if (visibleCount > totalVisibleCharacters) {
				break;
				//yield return new WaitForSeconds(1.0f);
				//visibleCount = 0;
			}

			textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

			visibleCount += 1;

			yield return new WaitForSeconds (0.0f);
		}
	}



}
