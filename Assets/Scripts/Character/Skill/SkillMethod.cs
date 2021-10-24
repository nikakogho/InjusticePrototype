using UnityEditor;

[System.Serializable]
public class SkillMethod
{
    public MonoScript script;

    public string MethodName { get { return possibleMethods[methodIndex]; } }

    public int methodIndex;
    public string[] possibleMethods;
    
    public MonoScript oldScript = null;
}
