using UnityEngine;
using UnityEngine.InputSystem;

// See https://gist.github.com/bison--/f97644377bd7f09e43d96547f67fda8a

namespace FlexXR.Runtime.Demos
{
    public class FlyCamera : MonoBehaviour
    {

        /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Extended by Mahelita 08-01-18.
        Added up and down movement using e and q, respectively.
        Adjusted parameters to fit my needs.
    Updated to the new Unity input sytem by @bison_42 / bison-- 2022-01-02
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    qe : Move camera down or up, respectively
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height
    */

        public float mainSpeed             = 1.0f;   //regular speed
        public float shiftAdd              = 25f;    //multiplied by how long shift is held.  Basically running
        public float maxShift              = 100.0f; //Maximum speed when holdin gshift
        public float camSens               = 0.1f;  //How sensitive it with mouse
        public bool  rotateOnlyIfMousedown = true;
        public bool  movementStaysFlat;

        private Vector3 _lastMouse = new(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float   _totalRun  = 1.0f;

        void Update()
        {

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _lastMouse = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0); // $CTK reset when we begin
            }

            if (!rotateOnlyIfMousedown ||
                (rotateOnlyIfMousedown && Mouse.current.rightButton.IsPressed()))
            {
                _lastMouse = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0) - _lastMouse;
                _lastMouse = new Vector3(-_lastMouse.y * camSens, _lastMouse.x * camSens, 0);
                var transform1  = transform;
                var eulerAngles = transform1.eulerAngles;
                _lastMouse             = new Vector3(eulerAngles.x + _lastMouse.x, eulerAngles.y + _lastMouse.y, 0);
                eulerAngles            = _lastMouse;
                transform1.eulerAngles = eulerAngles;
                _lastMouse             = Mouse.current.position.ReadValue();
                //Mouse  camera angle done.  
            }

            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Keyboard.current.leftShiftKey.IsPressed())
            {
                _totalRun += Time.deltaTime;
                p         *= (_totalRun * shiftAdd);
                p.x       =  Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y       =  Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z       =  Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                _totalRun =  Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
                p        *= mainSpeed;
            }

            p *= Time.deltaTime;
            var newPosition = transform.position;
            if (Keyboard.current.spaceKey.IsPressed() 
                    || (movementStaysFlat && !(rotateOnlyIfMousedown && Mouse.current.rightButton.IsPressed()))) { //If player wants to move on X and Z axis only
                Transform transform1;
                (transform1 = transform).Translate(p);
                var position = transform1.position;
                newPosition.x       = position.x;
                newPosition.z       = position.z;
                position            = newPosition;
                transform1.position = position;
            }
            else
            {
                transform.Translate(p);
            }

        }

        private Vector3 GetBaseInput()
        { //returns the basic values, if it's 0 than it's not active.
            var pVelocity = new Vector3();
            if (Keyboard.current.wKey.IsPressed())
            {
                pVelocity += new Vector3(0, 0, 1);
            }
            if (Keyboard.current.sKey.IsPressed())
            {
                pVelocity += new Vector3(0, 0, -1);
            }
            if (Keyboard.current.aKey.IsPressed())
            {
                pVelocity += new Vector3(-1, 0, 0);
            }
            if (Keyboard.current.dKey.IsPressed())
            {
                pVelocity += new Vector3(1, 0, 0);
            }
            if (Keyboard.current.qKey.IsPressed())
            {
                pVelocity += new Vector3(0, -1, 0);
            }
            if (Keyboard.current.eKey.IsPressed())
            {
                pVelocity += new Vector3(0, 1, 0);
            }
            return pVelocity;
        }
    }
}