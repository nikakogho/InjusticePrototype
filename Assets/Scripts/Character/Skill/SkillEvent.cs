[System.Serializable]
public class SkillEvent
{
    public string name;

    public SkillMethod[] methods;

    public void Invoke(Player player)
    {
        foreach(var method in methods)
        {
            var obj = method.script;

            var type = obj.GetClass();

            var info = type.GetMethod(method.MethodName);

            info.Invoke(null, new object[] { player });
        }
    }
}
