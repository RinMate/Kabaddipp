using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class JsonSkillConverter
{
    private static JsonSkillConverter _singleInstance = new JsonSkillConverter();

    public static JsonSkillConverter GetInstance()
    {
        return _singleInstance;
    }

    private JsonSkillConverter()
    {
        //TODO: initialization
    }
}