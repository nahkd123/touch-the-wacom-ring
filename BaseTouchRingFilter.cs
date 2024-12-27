using OpenTabletDriver.Configurations.Parsers.Wacom.IntuosV2;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace nahkd123.TouchTheWacomRing
{
    public abstract class BaseTouchRingFilter : IPositionedPipelineElement<IDeviceReport>
    {
        public virtual PipelinePosition Position { get; } = PipelinePosition.Raw;
        public event Action<IDeviceReport>? Emit;
        public bool LastScrolling { get; private set; } = false;
        public int LastAbsolutePosition { get; private set; } = 0;

        public virtual void Consume(IDeviceReport value)    
        {
            if (value is IntuosV2AuxReport)
            {
                var ringValue = value.Raw[4];
                var scrolling = ringValue != 0x7Fu;
                var absolute = scrolling ? ringValue - 0x80 : -1; // 72 segments

                if (LastScrolling && scrolling)
                {
                    var delta = absolute - LastAbsolutePosition;
                    var relative = delta <= -36 ? (72 + delta) : delta >= 36 ? (delta - 72) : delta;
                    ProcessTouchRing(true, absolute, relative);
                }
                else if (!scrolling)
                {
                    ProcessTouchRing(false, LastAbsolutePosition, 0);
                }

                LastScrolling = scrolling;
                LastAbsolutePosition = absolute;
            }

            Emit?.Invoke(value);
        }

        public virtual void ProcessTouchRing(bool scrolling, int absolute, int relative) { }
    }
}
