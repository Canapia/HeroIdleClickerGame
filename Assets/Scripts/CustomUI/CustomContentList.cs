using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum eTabKind
{
    Ability = 0,
    Skill
}

public class CustomContentList : MonoBehaviour
{
    public ScrollRect SR_Scroll;
    public GameObject Obj_Content;

    public eTabKind m_TabKind;

    public List<object> m_ContentList;

    //public UnityEvent AllButtonEvent = null;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ListUpate());
        m_ContentList = new List<object>();
    }

    public void RenewalUI()
    {
        if(MainController.Instance == null)
        {
            Debug.LogError("MainController 없음!");

            return;
        }

        if (m_TabKind == eTabKind.Ability)
        {
            for (int i = 0; i < (int)eHeroAbilityKind.END; i++)
            {
                GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_ContentListForm_Ability", Obj_Content.transform);
                PrefabContentListForm_Ability form = obj.GetComponent<PrefabContentListForm_Ability>();
                m_ContentList.Add(form);
                AdventureSceneManager.Instance.Group_Ability.Add(form);

                form.Initialize((eHeroAbilityKind)i);
            }
        }
        if (m_TabKind == eTabKind.Skill)
        {
            for (int i = 0; i < (int)eHeroSkillKind.END; i++)
            {
                if (MainController.Instance.UserInfo.GetUserIsUnlockSkill((eHeroSkillKind)i) == true)
                {
                    GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_ContentListForm_Skill", Obj_Content.transform);
                    PrefabContentListForm_Skill form = obj.GetComponent<PrefabContentListForm_Skill>();
                    m_ContentList.Add(form);
                    AdventureSceneManager.Instance.Group_Skill.Add(form);

                    form.Initialize((eHeroSkillKind)i);
                }
            }
        }

        /*
        if (m_TabKind == eTabKind.Ability)
        {
            for (int i = 0; i < ((int)eHeroInfoKind.HeroCriLevel + 1); i++)
            {
                //ResourceManager.CreatePrefab("Prefab_ContentListForm", Obj_Content.transform);
                GameObject obj = Instantiate(ResourceManager.LoadPrefab("Prefab_ContentListForm"));
                PrefabContentListForm form = obj.GetComponent<PrefabContentListForm>();
                //obj.transform.parent = Obj_Content.transform;
                obj.transform.SetParent(Obj_Content.transform);
                obj.transform.position = Obj_Content.transform.position;

                form.SettingUI((eHeroInfoKind)i);
            }
        }
        else if(m_TabKind == eTabKind.Skill)
        {
            for (int i = (int)eHeroInfoKind.Skill_Smash; i < ((int)eHeroInfoKind.END); i++)
            {
                //ResourceManager.CreatePrefab("Prefab_ContentListForm", Obj_Content.transform);
                GameObject obj = Instantiate(ResourceManager.LoadPrefab("Prefab_ContentListForm"));
                PrefabContentListForm form = obj.GetComponent<PrefabContentListForm>();
                //obj.transform.parent = Obj_Content.transform;
                obj.transform.SetParent(Obj_Content.transform);
                obj.transform.position = Obj_Content.transform.position;

                form.SettingUI((eHeroInfoKind)i);
            }
        }
        */
    }
}
