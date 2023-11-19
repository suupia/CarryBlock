using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
#nullable enable

namespace Projects.Utility.Scripts
{
    /// <summary>
    /// NetworkButtonsをコピーして作成した
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct LocalButtons
    {
        [FieldOffset(0)] int _bits;

        public int Bits => this._bits;

        public LocalButtons(int buttons) => this._bits = buttons;

        public bool IsSet(int button) => (this._bits & 1 << button) != 0;

        public void SetDown(int button) => this._bits |= 1 << button;

        public void SetUp(int button) => this._bits &= ~(1 << button);

        public void Set(int button, bool state)
        {
            if (state)
                this.SetDown(button);
            else
                this.SetUp(button);
        }

        public void SetAllUp() => this._bits = 0;

        public void SetAllDown() => this._bits = -1;

        public bool IsSet<T>(T button) where T : unmanaged, Enum => this.IsSet(UnsafeUtility.EnumToInt<T>(button));

        public void SetDown<T>(T button) where T : unmanaged, Enum => this.SetDown(UnsafeUtility.EnumToInt<T>(button));

        public void SetUp<T>(T button) where T : unmanaged, Enum => this.SetDown(UnsafeUtility.EnumToInt<T>(button));

        public void Set<T>(T button, bool state) where T : unmanaged, Enum =>
            this.Set(UnsafeUtility.EnumToInt<T>(button), state);

        public (LocalButtons, LocalButtons) GetPressedOrReleased(LocalButtons previous) =>
            (this.GetPressed(previous), this.GetReleased(previous));

        public LocalButtons GetPressed(LocalButtons previous)
        {
            previous._bits = (previous._bits ^ this._bits) & this._bits;
            return previous;
        }

        public LocalButtons GetReleased(LocalButtons previous)
        {
            previous._bits = (previous._bits ^ this._bits) & previous._bits;
            return previous;
        }

        public bool WasPressed(LocalButtons previous, int button) => this.GetPressed(previous).IsSet(button);

        public bool WasReleased(LocalButtons previous, int button) => this.GetReleased(previous).IsSet(button);

        public bool WasPressed<T>(LocalButtons previous, T button) where T : unmanaged, Enum =>
            this.GetPressed(previous).IsSet<T>(button);

        public bool WasReleased<T>(LocalButtons previous, T button) where T : unmanaged, Enum =>
            this.GetReleased(previous).IsSet<T>(button);
    }
}