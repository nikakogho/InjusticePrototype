using System.Collections.Generic;
using Injustice.CharacterBlueprints;
using UnityEngine;

public class Collection
{
    public static Collection instance;

    public List<Character> characters { get; private set; }
    
    public Collection(List<Character> characters)
    {
        this.characters = characters;

        instance = this;
    }

    public Character this[CharacterBlueprint blueprint]
    {
        get
        {
            foreach(var character in characters)
            {
                if (character.blueprint == blueprint) return character;
            }

            return null;
        }
    }

    public void AddCharacter(CharacterBlueprint blueprint)
    {
        if(this[blueprint] != null)
        {
            Debug.LogError("Such Character Already Exists!");
            return;
        }

        Character character = new Character(blueprint);

        characters.Add(character);
    }
}
