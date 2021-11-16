using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Finite
{
    public class FiniteMapGenerationSettings
    {
        public int Width;
        public int Height;

        public FiniteMapGenerationSettings(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
