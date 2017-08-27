using System;
using System.Runtime.InteropServices;
using System.Threading;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using static BladeFlurryCharges.WinApiMouse;

namespace BladeFlurryCharges
{
    public class BladeFlurryCharges : BaseSettingsPlugin<BladeFlurryChargesSettings>
    {
        public override void Render()
        {
            if (Settings.Enable)
            {
                var buffs = GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Life>().Buffs;
                if (buffs.Exists(b => b.Name == "charged_attack" && b.Charges == 6))
                {
                    if (!Settings.LeftClick)
                        MouseTools.MouseLeftClickEvent();
                    else
                        MouseTools.MouseRightClickEvent();
                }
            }
        }
    }

    internal static class MouseTools
    {
        public static void MouseLeftClickEvent()
        {
            MouseEvent(MouseEventFlags.LeftDown);
            Thread.Sleep(70);
            MouseEvent(MouseEventFlags.LeftUp);
        }

        public static void MouseRightClickEvent()
        {
            MouseEvent(MouseEventFlags.RightDown);
            Thread.Sleep(70);
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





