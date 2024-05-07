using UnityEngine;

namespace Script.Environment.Optimizer
{
    public class MarkerObject
    {
        public readonly GameObject GameObject;
        public readonly Renderer Renderer;

        public MarkerObject(GameObject gameObject, Renderer renderer)
        {
            GameObject = gameObject;
            Renderer = renderer;
        }
    }
}