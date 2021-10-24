using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Injustice.CharacterBlueprints;

public class CollectionManager : MonoBehaviour
{
    public string menuSceneName = "gameMenu";

    public GameObject characterSlotPrefab;
    public Transform characterSlotParent;

    Collection collection;

    CharacterSlot[] slots;

    List<Character> characters;

    IComparer<Character>[] allComparers = new IComparer<Character>[] 
    {
        new CompareByName(), new CompareByLabel(), new CompareByLevel(), new CompareByHealth(),
        new CompareByDamage(), new CompareByCategory(Category.Bronze),
        new CompareByCategory(Category.Silver), new CompareByCategory(Category.Gold)
    };

    IComparer<Character> currentComparer;

    void Awake()
    {
        collection = Collection.instance;

        currentComparer = allComparers[0];
    }

    void Start()
    {
        characters = collection.characters;

        characters.Sort(currentComparer);

        GenerateUI();

        UpdateUI();
    }

    public void Reverse()
    {
        characters.Reverse();

        UpdateUI();
    }

    public void OnComparerSelect(int index)
    {
        currentComparer = allComparers[index];

        characters.Sort(currentComparer);

        UpdateUI();
    }

    void GenerateUI()
    {
        slots = new CharacterSlot[characters.Count];

        for(int i = 0; i < slots.Length; i++)
        {
            var clone = Instantiate(characterSlotPrefab, characterSlotParent.position, characterSlotParent.rotation, characterSlotParent);

            var slot = clone.GetComponent<CharacterSlot>();

            slot.Apply(characters[i]);

            slots[i] = slot;
        }
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].Apply(characters[i]);
        }
    }

    #region Comparers

    class CompareByName : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            return x.blueprint.name.CompareTo(y.blueprint.name);
        }
    }

    class CompareByLabel : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            return x.blueprint.label.CompareTo(y.blueprint.label);
        }
    }

    class CompareByLevel : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            return x.level.CompareTo(y.level);
        }
    }

    class CompareByHealth : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            return x.startHealth.CompareTo(y.startHealth);
        }
    }

    class CompareByDamage : IComparer<Character>
    {
        public int Compare(Character x, Character y)
        {
            return x.damage.CompareTo(y.damage);
        }
    }

    class CompareByCategory : IComparer<Character>
    {
        public Injustice.CharacterBlueprints.Category category;

        public CompareByCategory(Injustice.CharacterBlueprints.Category category)
        {
            this.category = category;
        }

        public int Compare(Character x, Character y)
        {
            bool xIsCategory = x.blueprint.category == category;
            bool yIsCategory = y.blueprint.category == category;

            if (xIsCategory == yIsCategory) return 0;

            if (xIsCategory) return 1;

            return -1;
        }
    }

    #endregion

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
