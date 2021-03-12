using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using Spine.Unity;

public class ResourceManager : MonoBehaviour
{
    private static readonly string Img_Error = "NoImageFound";

    public static Sprite LoadSprite(string _name)
    {
        try
        {
            Sprite spr = (Sprite)LoadObject(@"Images\" + _name);

            return spr;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 이미지\n" + e);
        }

        return null;
    }

    public static Sprite LoadIconImage(string _name)
    {
        try
        {
            //Sprite spr = (Sprite)LoadObject(@"Images\Icon\" + _name);
            Texture2D txt2D = (Texture2D)LoadObject(@"Images\Icon\" + _name);
            Sprite spr = Sprite.Create(txt2D, new Rect(0f, 0f, txt2D.width, txt2D.height),
                new Vector2(0.5f, 0.5f), 100.0f);

            return spr;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 이미지\n" + e);
        }
        catch(NullReferenceException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 이미지\n" + e);
        }

        return null;
    }

    public static GameObject LoadPrefab(string _name)
    {

        //GameObject obj = (GameObject)LoadObject(@"Prefabs\" + _name);
        GameObject obj = (GameObject)LoadObject(@"Prefabs\" + _name);

        return obj;
    }

    public static void CreatePrefab(string _name, Transform _transform)
    {
        try
        {
            GameObject obj = Instantiate(LoadPrefab(_name));
            //obj.transform.parent = _transform;
            obj.transform.SetParent(_transform, false);
            obj.transform.position = _transform.position;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 프리팹\n" + e);
        }
    }

    public static GameObject GetOBJCreatePrefab(string _name, Transform _transform)
    {
        try
        {
            GameObject obj = Instantiate(LoadPrefab(_name));
            //obj.transform.parent = _transform;
            obj.transform.SetParent(_transform, false);
            obj.transform.position = _transform.position;

            return obj;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 프리팹\n" + e);
        }

        return null;
    }

    public static GameObject GetOBJSetPositionPrefab(string _name, float x, float y, float z, Transform _transform)
    {
        try
        {
            Vector3 position = new Vector3(x, y, z);

            GameObject obj = Instantiate(LoadPrefab(_name));
            //obj.transform.parent = _transform;
            obj.transform.SetParent(_transform);
            obj.transform.position = position;

            return obj;
        }
        catch (ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 프리팹\n" + e);
        }

        return null;
    }

    public static SkeletonDataAsset GetSkeletonDataAsset(string _name)
    {
        try
        {
            SkeletonDataAsset asset = (SkeletonDataAsset)LoadObject(@"SpineResource\Skeleton\" + _name);

            return asset;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않는 SkeletonDataAsset");
        }

        return null;
    }

    public static SkeletonGraphic GetSkeletonGraphic(string _name)
    {
        try
        {
            SkeletonGraphic graphic = new SkeletonGraphic();
            graphic.skeletonDataAsset = GetSkeletonDataAsset(_name);
            //AssetDatabase.CreateAsset(graphic, @"Assets\Resources\SpineResource\Skeleton\" + _name + ".asset");
                
            return graphic;
        }
        catch (ArgumentException e)
        {
            Debug.LogError(_name + "은 존재하지 않음\n" + e);
        }

        return null;
    }

    public static SkeletonGraphic CreateSkeletonGraphic(string _name, Transform _trans)
    {
        try
        {
            GameObject obj = Instantiate(LoadPrefab(@"Prefab_Monster\" + _name));
            SkeletonGraphic graphic = obj.GetComponent<SkeletonGraphic>();
            //graphic.skeletonDataAsset = (SkeletonDataAsset)LoadObject(@"SpineResource\Monster\Slime\Monster_Slime_SkeletonData");
            obj.transform.SetParent(_trans);
            obj.transform.position = _trans.position;

            return graphic;
        }
        catch(ArgumentException e)
        {
            Debug.LogError(_name + "은 존재 하지 않는 애니메이션 프리팹\n" + e);
        }

        return null;
    }

    private static UnityEngine.Object LoadObject(string _name)
    {
        //UnityEngine.Object obj = Resources.Load(@"Assets\Resources\" + _name) as UnityEngine.Object;
        UnityEngine.Object obj = Resources.Load(_name) as UnityEngine.Object;
        //UnityEngine.Object obj = Resources.Load<UnityEngine.Object>(_name);

        return obj;
    }
}
