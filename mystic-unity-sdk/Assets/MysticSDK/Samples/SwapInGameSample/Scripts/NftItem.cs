using System;
using UnityEngine;

namespace Samples.SwapInGameSample.Scripts
{
    public class NftItem 
    {
        public Guid Guid;
        public string Title;
        public string ImageUrl;

        public override string ToString()
        {
            return $"NftItem: \t {Guid} \t {Title}";
        }
    }
}