using UnityEngine;
public enum CharacterType
{
    Male,
    Female
}
public class UI_GenderChoice : MonoBehaviour
{
    public GameObject MalePrefab;
    public GameObject FemalePrefab;

    private void Start()
    {
        MalePrefab.SetActive(false);
        FemalePrefab.SetActive(true);
    }

    public void OnClickCharacterType(CharacterType characterType)
    {
        MalePrefab.SetActive(characterType == CharacterType.Male);
        FemalePrefab.SetActive(characterType == CharacterType.Female);
        
        PlayerPrefs.SetInt("SelectedCharacterType", (int)characterType);
    }
    
    public void OnClickMaleChoiceButton()
    {
        OnClickCharacterType(CharacterType.Male);
    }

    public void OnClickFemaleChoiceButton()
    {
        OnClickCharacterType(CharacterType.Female);
    }
}
