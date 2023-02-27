using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.Tilemaps
{
    [CreateAssetMenu(fileName = "new tileset", menuName = "new tilsset", order = 3)]
    public class tileset : ScriptableObject
    {
        [OnlyAllow(typeof(tiledata), typeof(tiledata))]
        public Object[] tiles;
    }
}