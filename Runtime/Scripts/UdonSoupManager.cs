
using UdonSoup.Core;
using UnityEngine;

namespace UdonSoup
{
    public class UdonSoupManager : Base
    {
        [Header("Settings")]
        [SerializeField, Range(1, 128)]
        private int maxCapacity = 32; public int MaxCapacity => maxCapacity;

    }
}