using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public Text nameText, labelText, healthText, damageText;
    public Image iconImage;

    public Character character { get; private set; }

    public void Apply(Character character)
    {
        this.character = character;

        nameText.text = character.blueprint.name;
        labelText.text = character.blueprint.label;

        healthText.text = character.startHealth.ToString();
        damageText.text = character.damage.ToString();

        iconImage.sprite = character.blueprint.icon;
    }
}
