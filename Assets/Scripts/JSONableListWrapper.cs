using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class JSONableListWrapper<T>
{
    public List<T> list;
    public JSONableListWrapper(List<T> list)
    {
        this.list = list;
    }
}
