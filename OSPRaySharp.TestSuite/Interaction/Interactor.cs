using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Interaction
{
    public enum MouseEventType
    {
        DblClick,
        Down,
        Up,
        Move,
        Wheel,
    }

    // mouse event data
    public class MouseEvent
    {
        public MouseEventType EventType { get; set; }
        public int Button { get; set; }
        public bool ShiftKey { get; set; }
        public bool CtrlKey { get; set; }
        public bool AltKey { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Delta { get; set; }
    };

    public interface Interactor
    {

        void Refocus();

        void Reset();

        void InjectMouseEvent(MouseEvent mouseEvent);
    }
}
