using UnityEngine;

public class JukeBox : UpgradeBase
{
    public override void onUpgrade()
    {
        AudioSystem.get().startBgm();
    }
}
