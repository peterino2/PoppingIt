using UnityEngine;

public class JukeBox : UpgradeBase
{
    public GameObject jukeboxObject;
    public override void onUpgrade()
    {
        jukeboxObject.SetActive(true);
        AudioSystem.get().startBgm();
    }
}
