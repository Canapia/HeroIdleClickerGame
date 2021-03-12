using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class AdventureSceneManager : MonoBehaviour
{
    public List<PrefabContentListForm_Ability> Group_Ability = new List<PrefabContentListForm_Ability>();
    public List<PrefabContentListForm_Skill> Group_Skill = new List<PrefabContentListForm_Skill>();

    public CustomTabGroup Tab_Group;

    private static AdventureSceneManager m_Instance;
    public static AdventureSceneManager Instance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);

        if (m_Instance == null)
        {
            m_Instance = GetComponent<AdventureSceneManager>();
        }
        else
        {
            Destroy(m_Instance);
        }

        Group_Ability.Clear();
        Group_Skill.Clear();
    }

    public void RenewalTabUI()
    {
        if(Group_Ability.Count > 0)
        {
            foreach(PrefabContentListForm_Ability ability in Group_Ability)
            {
                ability.Renewal_Button_Upgrade();
            }
        }

        if(Group_Skill.Count > 0)
        {
            foreach (PrefabContentListForm_Skill skill in Group_Skill)
            {
                skill.Renewal_Button_Upgrade();
            }
        }
    }
}
