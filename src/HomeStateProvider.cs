using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models.MQTT.Enums;

namespace src
{
    public interface IHomeStateProvider
    {
        IReadOnlyList<string> LightRelays { get; set; }
        OXTPressAction? CorridorLastPressAction { get; set; }
        State<bool> CorridorLongPressed { get; set; }
        OXTPressAction? BedroomLastPressAction { get; set; }
        State<bool> DoorOpened { get; set; }
    }

    public class HomeStateProvider : IHomeStateProvider
    {
        private Locked<IReadOnlyList<string>> _lightRelays = new Locked<IReadOnlyList<string>>(new List<string>());
        private Locked<OXTPressAction?> _corridorLastPressAction = new Locked<OXTPressAction?>(null);
        private State<bool> _corridorLongPressed = new State<bool>(false);
        private Locked<OXTPressAction?> _bedroomLastPressAction = new Locked<OXTPressAction?>(null);
        private State<bool> _doorOpened = new State<bool>(false);

        public IReadOnlyList<string> LightRelays
        {
            get => _lightRelays.Value;
            set => _lightRelays.Value = value;
        }

        public OXTPressAction? CorridorLastPressAction
        {
            get => _corridorLastPressAction.Value;
            set => _corridorLastPressAction.Value = value;
        }

        public State<bool> CorridorLongPressed
        {
            get => _corridorLongPressed;
            set => _corridorLongPressed.Value = value.Value;
        }

        public OXTPressAction? BedroomLastPressAction
        {
            get => _bedroomLastPressAction.Value;
            set => _bedroomLastPressAction.Value = value;
        }

        public State<bool> DoorOpened
        {
            get => _doorOpened;
            set => _doorOpened.Value = value.Value;
        }
    }
}