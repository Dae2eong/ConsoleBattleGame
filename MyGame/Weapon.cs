using System;

public class Weapon
{
    public int Code { get; }
    public string WpName { get; }
    public int WpStr { get; }
    public int WpDef { get; }
    public int WpMaxDur { get; }
    public int WpMinDur { get; }
    public int WpLevel { get; }
    public int WpGold { get; }
    public string WpType { get; }
    public bool WpEnhanceCheck { get; }
    public int WpEnhanceLevel { get; }
    public bool WpEquip { get; set; }

    public Weapon(int code, string wpName, int wpStr, int wpDef, int wpMaxDur, 
        int wpMinDur, int wpLevel, int wpGold, string wpType, bool wpEnhanceCheck, int wpEnhanceLevel, bool wpEquip)
    {
        Code = code;
        WpName = wpName;
        WpStr = wpStr;
        WpDef = wpDef;
        WpMaxDur = wpMaxDur;
        WpMinDur = wpMinDur;
        WpLevel = wpLevel;
        WpGold = wpGold;
        WpType = wpType;
        WpEnhanceCheck = wpEnhanceCheck;
        WpEnhanceLevel = wpEnhanceLevel;
        WpEquip = wpEquip;
    }
}
