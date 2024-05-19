using System;
using UnityEngine;

namespace Script.Interaction
{
    public class Sliceable : MonoBehaviour
    {
        public Action<Slicer> OnSlice;
        
        public void Notify(Slicer slicer)
        {
            OnSlice?.Invoke(slicer);
        }
    }
}
