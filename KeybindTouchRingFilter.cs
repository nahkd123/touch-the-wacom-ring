using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Platform.Keyboard;

namespace nahkd123.TouchTheWacomRing
{
    [PluginName("Touch Ring Keybind")]
    public class KeybindTouchRingFilter : BaseTouchRingFilter
    {
        [Resolved]
        public required IVirtualKeyboard Keyboard { get; set; }

        [Property("Ring left spin key"), DefaultPropertyValue("LeftBracket"), ToolTip("The key to press when spinning the ring to the left")]
        public string LeftKey { get; set; } = "LeftBracket";
        [Property("Ring right spin key"), DefaultPropertyValue("RightBracket"), ToolTip("The key to press when spinning the ring to the right")]
        public string RightKey { get; set; } = "RightBracket";
        [Property("Pressing mode"), DefaultPropertyValue("Unit"), PropertyValidated(nameof(ValidPressingModes)), ToolTip("How the plugin press the keys")]
        public string PressingMode { get; set; } = "Unit";

        public override void ProcessTouchRing(bool scrolling, int absolute, int relative)
        {
            if (!scrolling) return;
            if (!Enum.TryParse<PressMode>(PressingMode, true, out var pressingMode)) return;
            var keystrokes = pressingMode == PressMode.Unit ? relative : Math.Sign(relative);
            var key = keystrokes > 0 ? LeftKey : RightKey;
            var nextDirection = keystrokes < 0 ? 1 : -1;

            while (keystrokes != 0)
            {
                Keyboard.Press(key);
                Keyboard.Release(key);
                keystrokes += nextDirection;
            }
        }

        private static IEnumerable<string>? validPressingModes;
        public static IEnumerable<string> ValidPressingModes => validPressingModes ??= Enum.GetValues<PressMode>().Select(Enum.GetName)!;
    }

    public enum PressMode
    {
        Report,
        Unit
    }
}
