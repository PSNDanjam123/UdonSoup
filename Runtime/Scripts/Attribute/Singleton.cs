using System;
using UnityAttribute = System.Attribute;

namespace UdonSoup.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Singleton : UnityAttribute
    {
    }
}