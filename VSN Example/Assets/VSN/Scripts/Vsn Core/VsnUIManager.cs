using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Command;
using TMPro;
using TMPro.Examples;
using DG.Tweening;

public class VsnUIManager : MonoBehaviour {

  public static VsnUIManager instance;

  public GameObject vsnMessagePanel;
  public TextMeshProUGUI vsnMessageText;
  public TextMeshProUGUI vsnMessageTitle;
  public Image vsnMessageTitlePanel;
  public Button screenButton;
  public Image choicesPanel;
  public RectTransform charactersPanel;
  public Image backgroundImage;
  public Button[] choicesButtons;
  public Text[] choicesTexts;

  public GameObject vsnCharacterPrefab;

  public bool isTextAppearing;

  public int charsToShowPerSecond = 30;

  private List<VsnCharacter> characters;

  void Awake() {
    if(instance == null) {
      instance = this;
    }

    screenButton.onClick.AddListener(OnScreenButtonClick);
    characters = new List<VsnCharacter>();
  }

  public void SetMessagePanel(bool value) {
    vsnMessagePanel.SetActive(value);
  }

  public void SetText(string msg) {
    if(string.IsNullOrEmpty(vsnMessageTitle.text)) {
      vsnMessageTitlePanel.gameObject.SetActive(false);
    } else {
      vsnMessageTitlePanel.gameObject.SetActive(true);
    }
    vsnMessageText.text = msg;
    vsnMessageText.GetComponent<TextConsoleSimulator>().StartShowingCharacters();
  }

  public void SetTextTitle(string messageTitle) {
    vsnMessageTitle.text = messageTitle;
  }

  void OnScreenButtonClick() {
    if(isTextAppearing) {
      isTextAppearing = false;
      vsnMessageText.GetComponent<TextConsoleSimulator>().FinishShowingCharacters();
    } else {
      if(VsnController.instance.state == ExecutionState.WAITINGINPUT) {
        VsnController.instance.state = ExecutionState.PLAYING;
        SetMessagePanel(false);
      }
    }
  }

  private void AddChoiceButtonListener(Button button, string label) {
    button.onClick.AddListener(() => {
      VsnCommand command = new GotoCommand();
      List<VsnArgument> arguments = new List<VsnArgument>();
      arguments.Add(new VsnString(label));

      command.InjectArguments(arguments);
      SetChoicesPanel(false, 0);
      command.Execute();
      VsnController.instance.state = ExecutionState.PLAYING;
    });
  }

  public void SetChoicesPanel(bool enable, int numberOfChoices) {
    choicesPanel.gameObject.SetActive(enable);

    if(enable) {
      for(int i = 0; i < choicesButtons.Length; i++) {
        bool willSetActive = (i < numberOfChoices);
        choicesButtons[i].gameObject.SetActive(willSetActive);
      }
    }
  }

  public void SetChoicesTexts(string[] choices) {
    for(int i = 0; i < choices.Length; i++) {
      if(choicesTexts[i].gameObject.activeInHierarchy) {
        choicesTexts[i].text = choices[i];
      }
    }
  }

  public void SetChoicesLabels(string[] labels) {
    for(int i = 0; i < labels.Length; i++) {
      AddChoiceButtonListener(choicesButtons[i], labels[i]);
    }
  }

  public void CreateNewCharacter(Sprite characterSprite, string characterFilename, string characterLabel) {
    GameObject vsnCharacterObject = Instantiate(vsnCharacterPrefab, charactersPanel.transform) as GameObject;
    vsnCharacterObject.transform.localScale = Vector3.one;
    VsnCharacter vsnCharacter = vsnCharacterObject.GetComponent<VsnCharacter>();

    Vector2 newPosition = Vector2.zero;
    vsnCharacter.GetComponent<RectTransform>().anchoredPosition = newPosition;

    vsnCharacter.GetComponent<Image>().sprite = characterSprite;
    vsnCharacter.label = characterLabel;
    vsnCharacter.characterFilename = characterFilename;

    characters.Add(vsnCharacter);
  }

  public void MoveCharacterX(string characterLabel, float position, float duration) {
    float screenPosition = GetCharacterScreenPositionX(position);
    VsnCharacter character = FindCharacterByLabel(characterLabel);

    if(character != null) {
      Vector2 newPosition = new Vector2(screenPosition, character.GetComponent<RectTransform>().anchoredPosition.y);
      if(duration != 0) {
        character.GetComponent<RectTransform>().DOAnchorPos(newPosition, duration);
      } else {
        character.GetComponent<RectTransform>().anchoredPosition = newPosition;
      }
    }
  }

  public void MoveCharacterY(string characterLabel, float position, float duration) {
    float screenPosition = GetCharacterScreenPositionY(position);
    VsnCharacter character = FindCharacterByLabel(characterLabel);

    if(character != null) {
      Vector2 newPosition = new Vector2(character.GetComponent<RectTransform>().anchoredPosition.x, screenPosition);
      if(duration != 0) {
        character.GetComponent<RectTransform>().DOAnchorPos(newPosition, duration);
      } else {
        character.GetComponent<RectTransform>().anchoredPosition = newPosition;
      }
    }

  }

  public void SetCharacterAlpha(string characterLabel, float alphaValue, float duration) {
    VsnCharacter character = FindCharacterByLabel(characterLabel);

    if(character != null) {
      Image characterImage = character.GetComponent<Image>();
      if(duration != 0) {
        characterImage.DOFade(alphaValue, duration);
      } else {
        characterImage.color = new Color(characterImage.color.r,
                                         characterImage.color.g,
                                         characterImage.color.b, alphaValue);
      }
    }
  }

  private float GetCharacterScreenPositionX(float normalizedPositionX) {
    float zeroPoint = -charactersPanel.rect.width/2f;
    float onePoint = charactersPanel.rect.width/2f;
    float totalSize = onePoint - zeroPoint;

//    if(normalizedPositionX < 0f)
//      return zeroPoint;
//    else if(normalizedPositionX > 1f)
//      return onePoint;

    float finalPositionX = zeroPoint + normalizedPositionX * totalSize;
    return finalPositionX;
  }

  private float GetCharacterScreenPositionY(float normalizedPositionY) {
    int maxPoint = 500;
    int minPoint = 200;
    int totalPoints = Mathf.Abs(maxPoint) + Mathf.Abs(minPoint);

    if(normalizedPositionY < 0f)
      return minPoint;
    else if(normalizedPositionY > 1f)
      return maxPoint;

    float finalPositionY = normalizedPositionY * totalPoints;
    Debug.Log("Final Y: " + finalPositionY);
    return finalPositionY;
  }

  private VsnCharacter FindCharacterByLabel(string characterLabel) {
    foreach(VsnCharacter character in characters) {
      if(character.label == characterLabel) {
        return character;
      }
    }
    return null;
  }

  public void FlipCharacterSprite(string characterLabel) {
    VsnCharacter character = FindCharacterByLabel(characterLabel);

    Vector3 localScale = character.transform.localScale;
    character.transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
  }


  public void ResetAllCharacters() {
    foreach(VsnCharacter character in characters) {
      Destroy(character.gameObject);
    }
    characters.Clear();
  }

  public void SetBackground(Sprite backgroundSprite) {
    backgroundImage.sprite = backgroundSprite;
    backgroundImage.gameObject.SetActive(true);
  }

  public void ResetBackground() {
    backgroundImage.gameObject.SetActive(false);
  }
}

