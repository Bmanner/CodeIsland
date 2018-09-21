using UnityEngine;

public class ScriptInfo
{
    /*
    public ScriptInfo(direction dir, ifType ifType, GameObject obj = null)
    {
        clonedBtn = obj;
        direction = dir;
        this.ifType = ifType;
        endIf = false;
    }*/

    public ScriptInfo(direction dir, ifType ifType, bool endIf, GameObject obj = null)
    {
        clonedBtn = obj;
        direction = dir;
        this.ifType = ifType;
        this.endIf = endIf;
    }

    public GameObject clonedBtn;
    public direction direction;
    public ifType ifType;
    public bool endIf;

    void SetInfo(direction dir, ifType ifType, GameObject obj = null)
    {
        clonedBtn = obj;
        direction = dir;
        this.ifType = ifType;
    }

    public void SetDirection(direction dir) { direction = dir; }
    public void SetIfType(ifType type) { ifType = type; }
    public void SetObject(GameObject obj) { clonedBtn = obj; }
}

public class UserScriptInfo : ScriptInfo
{
    public UserScriptInfo(direction dir, ifType ifType, int n, GameObject obj = null, GameObject belongingIfBlock = null) : base(dir, ifType, false, obj)
    {
        index = n;
        this.belongingIfBlock = belongingIfBlock;
    }

    public UserScriptInfo(direction dir, ifType ifType, bool endIf, int n, GameObject obj = null, GameObject belongingIfBlock = null) : base(dir, ifType, endIf, obj)
    {
        index = n;
        this.belongingIfBlock = belongingIfBlock;
    }

    public int index;
    public GameObject belongingIfBlock;
}
