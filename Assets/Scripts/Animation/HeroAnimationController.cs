using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine;
using Spine.Unity;

public class HeroAnimationController : SpineAnimationManager
{
    [SpineSkin] private readonly string ArmorSkinName = "Clothes";
    [SpineSkin] private readonly string SwordSkinName = "Sword";

    /// <summary>
    /// 방어구 스킨 변경
    /// </summary>
    /// <param name="_level">방어구 레벨</param>
    /// <returns></returns>
    private string SetArmorSkin(int _level)
    {
        string returnValue;

        switch(_level)
        {
            case 1:
                returnValue = "1ST";
                break;
            case 2:
                returnValue = "2ST";
                break;
            case 3:
                returnValue = "3ST";
                break;
            case 4:
                returnValue = "4ST";
                break;
            case 5:
                returnValue = "5ST";
                break;

            default:
                returnValue = "default";
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// 무기 스킨 변경
    /// </summary>
    /// <param name="_level">무기 레벨</param>
    /// <returns></returns>
    private string SetSwordSkin(int _level)
    {
        string returnValue;

        switch (_level)
        {
            case 1:
                returnValue = "Sword_1ST";
                break;
            case 2:
                returnValue = "Sword_2ST";
                break;
            case 3:
                returnValue = "Sword_3ST";
                break;
            case 4:
                returnValue = "Sword_4ST";
                break;
            case 5:
                returnValue = "Sword_5ST";
                break;

            default:
                returnValue = "default";
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// 전체 스킨 변경
    /// </summary>
    /// <param name="_ArmorLevel">방어구 레벨</param>
    /// <param name="_SwordLevel">무기 레벨</param>
    public void SetSkinCombine(int _ArmorLevel, int _SwordLevel)
    {
        // 두 개 이상의 스킨을 동시에 그리는 방법!!
        // Spine.Skin을 할당해서 skeleton에 주는 방법

        Skeleton skeleton = m_UIAnimation.Skeleton;
        SkeletonData skelData = skeleton.Data;

        //var LevelAmor = skelData.FindSkin(ArmorSkinName);
        //var LevelSword = skelData.FindSkin(SwordSkinName);

        // {0}/{1} - SpineAnimation 내에 스킨이 파일별로 분류가 되있기 때문에
        // {해당 스킨이 든 파일 명} / {해당 스킨} 으로 스킨명을 해주어야 한다.
        var LevelArmor = skelData.FindSkin(string.Format("{0}/{1}", ArmorSkinName, SetArmorSkin(_ArmorLevel)));
        var LevelSword = skelData.FindSkin(string.Format("{0}/{1}", SwordSkinName, SetSwordSkin(_SwordLevel)));

        Skin newSkin = new Skin("Hero Skin");
        newSkin.AddSkin(LevelArmor);
        newSkin.AddSkin(LevelSword);

        skeleton.SetSkin(newSkin);
        skeleton.SetSlotsToSetupPose();
        //m_UIAnimation.Update(0);
    }
}
