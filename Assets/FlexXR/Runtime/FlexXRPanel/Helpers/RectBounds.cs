using UnityEngine;

namespace FlexXR.Runtime.FlexXRPanel.Helpers
{
    public class RectBoundsFloat
    {
        public float xMin, xMax, yMin, yMax;
        public float Width  => xMax - xMin;
        public float Height => yMax - yMin;
        
        public Vector2 Center => new((xMin + xMax)/2, (yMin + yMax)/2);

        public RectBoundsFloat() { }

        public RectBoundsFloat(Rect rect)
        {
            xMin = rect.xMin;
            xMax = rect.xMax;
            yMin = rect.yMin;
            yMax = rect.yMax;
        }

        public void CopyFrom(RectBoundsFloat from)
        {
            xMin = from.xMin;
            xMax = from.xMax;
            yMin = from.yMin;
            yMax = from.yMax;
        }
        
        public RectBoundsFloat Scale(float factor)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var scaled = new RectBoundsFloat();
            scaled.xMin = factor * xMin;
            scaled.xMax = factor * xMax;
            scaled.yMin = factor * yMin;
            scaled.yMax = factor * yMax;
            return scaled;
        }

        public RectBoundsFloat Limit(RectBoundsFloat limitingBounds)
        {
            var bounds = new RectBoundsFloat();
            bounds.CopyFrom(this);
            
            if (bounds.xMin < limitingBounds.xMin) bounds.xMin = limitingBounds.xMin;
            if (bounds.xMax > limitingBounds.xMax) bounds.xMax = limitingBounds.xMax;
            if (bounds.yMin < limitingBounds.yMin) bounds.yMin = limitingBounds.yMin;
            if (bounds.yMax > limitingBounds.yMax) bounds.yMax = limitingBounds.yMax;
            
            return bounds;
        }
        
        public RectBoundsInt ToRectBoundsInt()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var rectBoundsInt = new RectBoundsInt();
            rectBoundsInt.xMin = Mathf.CeilToInt(xMin);
            rectBoundsInt.xMax = Mathf.CeilToInt(xMax);
            rectBoundsInt.yMin = Mathf.CeilToInt(yMin);
            rectBoundsInt.yMax = Mathf.CeilToInt(yMax);
            return rectBoundsInt;
        }
    }
    // C# doesn't seem to let us use the subtraction operation with a generic class so we need two separate implementations.
    public class RectBoundsInt
    {
        public int xMin, xMax, yMin, yMax;
        public int Width  => xMax - xMin;
        public int Height => yMax - yMin;
        
        public void CopyFrom(RectBoundsInt from)
        {
            xMin = from.xMin;
            xMax = from.xMax;
            yMin = from.yMin;
            yMax = from.yMax;
        }

        public bool Equals(RectBoundsInt other)
        {
            return xMin == other.xMin && xMax == other.xMax && yMin == other.yMin && yMax == other.yMax;
        }
        
        public Vector2 Center
        {
            get
            {
                var xNumerator = xMin + xMax;
                var yNumerator = yMin + yMax;
                
                if (IsOdd(xNumerator)) xNumerator += 1;
                if (IsOdd(yNumerator)) yNumerator += 1;
                
                return new Vector2(
                    // ReSharper disable once PossibleLossOfFraction  // Handled above with IsEven check
                    xNumerator / 2,
                    // ReSharper disable once PossibleLossOfFraction  // Handled above with IsEven check
                    yNumerator / 2);
            }
        }

        private static bool IsOdd(int integer)
        {
            return integer % 2 == 1;
        }
    }
}
