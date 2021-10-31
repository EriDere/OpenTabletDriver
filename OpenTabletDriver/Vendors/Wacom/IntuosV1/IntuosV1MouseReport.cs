using System.Numerics;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Vendors.Wacom.IntuosV1
{
    public struct IntuosV1MouseReport : IMouseReport
    {
        public IntuosV1MouseReport(byte[] report)
        {
            Raw = report;

            Position = new Vector2
            {
                X = (report[3] | report[2] << 8) << 1 | report[9] >> 1 & 1,
                Y = (report[5] | report[4] << 8) << 1 | report[9] & 1
            };

            MouseButtons = new bool[]
            {
                report[8].IsBitSet(0), // primary
                report[8].IsBitSet(2), // secondary
                report[8].IsBitSet(1), // middle
                report[8].IsBitSet(5), // forward
                report[8].IsBitSet(4), // backward
            };

            var scrollAmount = (report[6] << 2) | ((report[7] >> 6) & 0b11);
            Scroll = new Vector2
            {
                    Y = report[8].IsBitSet(3) ? scrollAmount : -scrollAmount
            };
        }

        public byte[] Raw { set; get; }
        public Vector2 Position { set; get; }
        public bool[] MouseButtons { set; get; }
        public Vector2 Scroll { set; get; }
    }
}