using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eDamageState
{
    Damage = 0,
    Critical,
    Gold,
    Heal,

    END
}

public class UIDamage : MonoBehaviour
{
    public Text Txt_Damage;
    public GameObject Obj_Gold;
    public GameObject Obj_Critical;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Initailize(eDamageState _state, int _number)
    {
        switch(_state)
        {
            case eDamageState.Damage:
                Obj_Gold.SetActive(false);
                Obj_Critical.SetActive(false);
                Txt_Damage.text = string.Format("-{0}", _number.ToString());
                Txt_Damage.color = new Color(0.823f, 0.333f, 0.313f);
                break;

            case eDamageState.Critical:
                Obj_Gold.SetActive(false);
                Obj_Critical.SetActive(true);
                Txt_Damage.text = string.Format("-{0}", _number.ToString());
                Txt_Damage.color = new Color(0.823f, 0.333f, 0.313f);
                break;

            case eDamageState.Gold:
                Obj_Gold.SetActive(true);
                Obj_Critical.SetActive(false);
                Txt_Damage.text = string.Format("{0}", _number.ToString());
                Txt_Damage.color = new Color(1f, 0.556f, 0.27f);
                break;

            case eDamageState.Heal:
                Obj_Gold.SetActive(false);
                Obj_Critical.SetActive(false);
                Txt_Damage.text = string.Format("+{0}", _number.ToString());
                Txt_Damage.color = Color.green;
                break;
        }

        StartCoroutine(DestroyDamage());
    }

    IEnumerator DestroyDamage()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
