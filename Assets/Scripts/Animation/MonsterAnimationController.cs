using System.Collections;
using System.Collections.Generic;

using Spine;
using Spine.Unity;

public class MonsterAnimationController : SpineAnimationManager
{
    public void SetSkin(string _name)
    {
        //m_UIAnimation.Skeleton.SetSkin(_name);

        Skeleton skeleton = m_UIAnimation.Skeleton;
        SkeletonData skelData = skeleton.Data;

        var skin = skelData.FindSkin(_name);

        Skin newSkin = new Skin("Monster Skin");
        newSkin.AddSkin(skin);

        skeleton.SetSkin(newSkin);
        skeleton.SetSlotsToSetupPose();
    }

    public void SetSkeletonDataAsset(string _name)
    {
        m_UIAnimation.skeletonDataAsset = ResourceManager.GetSkeletonDataAsset(_name);
    }

    public void SetSekeltonUIAnimation(SkeletonGraphic _graphic)
    {
        m_UIAnimation = _graphic;
    }
}
