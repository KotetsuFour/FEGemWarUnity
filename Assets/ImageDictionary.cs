using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDictionary : MonoBehaviour
{
    public List<string> keys;
    public List<Sprite> images;
    private static Dictionary<string, Sprite> dict;

    void Awake()
    {
        dict = new Dictionary<string, Sprite>();
        for (int q = 0; q < keys.Count; q++)
        {
            dict.Add(keys[q], images[q]);
        }
    }
    public static Sprite getImage(string key)
    {
        return dict[key];
    }
}
