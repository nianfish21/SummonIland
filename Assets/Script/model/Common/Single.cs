using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// µ¥Àý
/// </summary>
/// <typeparam name="T"></typeparam>
public class single<T> where T : single<T>, new()
{
    protected static T _Instance;
    public static T ins
    {
        get
        {
            if (_Instance == null)
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                    //  singleMgr.singles.Add(_Instance);
                }
            }
            return _Instance;
        }
    }
}