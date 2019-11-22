using System;
using System.Collections;
using System.Runtime.InteropServices;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.Shared;
using static BladeFlurryCharges.WinApiMouse;

namespace BladeFlurryCharges
{
    public class BladeFlurryChargesCore : BaseSettingsPlugin<BladeFlurryChargesSettings>
    {
        private Coroutine _mainWork { get; set; }
        public override bool Initialise()
        {
            Name = "Blade Flurry Charges";

            _mainWork = new Coroutine(() => RealeaseCharge(), new WaitTime(Settings.TimeCheckCharges), this, "BladeFlurry Realease");
            Core.ParallelRunner.Run(_mainWork);

            Settings.TimeCheckCharges.OnValueChanged += (sender, b) =>
            {
                UpdateCoroutineWaitRender();
            };

            return base.Initialise();
        }

        private void UpdateCoroutineWaitRender()
        {
            _mainWork.UpdateCondtion(new WaitTime(Settings.TimeCheckCharges));
        }

        IEnumerator RealeaseCharge()
        {
            while (true)
            {
                var buffs = GameController.EntityListWrapper.Player.GetComponent<Life>().Buffs;
                if (buffs.Exists(b => b.Name == "charged_attack" && b.Charges == 6))
                {
                    if (!Settings.LeftClick)
                        yield return  MouseTools.MouseLeftClickEvent();
                    else
                        yield return  MouseTools.MouseRightClickEvent();
                }
                yield return new WaitTime(Settings.TimeCheckCharges);
            }
        }
        
        public override void Render()
        {
            return;
        }
    }
   
    internal static class MouseTools
    { 
        public static IEnumerator MouseLeftClickEvent()
        {
            MouseEvent(MouseEventFlags.LeftDown);
            yield return new WaitTime(50);
            MouseEvent(MouseEventFlags.LeftUp);
        }

        public static IEnumerator MouseRightClickEvent()
        {
            
            MouseEvent(MouseEventFlags.RightDown);
            yield return new WaitTime(50);
            MouseEvent(MouseEventFlags.RightUp);
        }

        private static Point GetCursorPosition()
        {
            Point currentMousePoint;
            return GetCursorPos(out currentMousePoint) ? new Point(currentMousePoint.X, currentMousePoint.Y) : new Point(0, 0);
        }

        private static void MouseEvent(MouseEventFlags value)
        {
            var position = GetCursorPosition();

            mouse_event
                ((int)value,
                    position.X,
                    position.Y,
                    0,
                    0)
                ;
        }
    }

    public static class WinApiMouse
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpMousePoint);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        #region Structs/Enums

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

        }

        #endregion
    }
}





