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
        OXTPressAction? BedroomLastPressAction { get; set; }
    }

    public class HomeStateProvider : IHomeStateProvider
    {
        private Locked<IReadOnlyList<string>> _lightRelays = new Locked<IReadOnlyList<string>>(new List<string>());

        private Locked<OXTPressAction?> _corridorLastPressAction = new Locked<OXTPressAction?>(null);
        private Locked<OXTPressAction?> _bedroomLastPressAction = new Locked<OXTPressAction?>(null);

        public IReadOnlyList<string> LightRelays
        {
            get
            {
                return _lightRelays.Value;
            }
            set
            {
                _lightRelays.Value = value;
            }
        }

        public OXTPressAction? CorridorLastPressAction
        {
            get
            {
                return _corridorLastPressAction.Value;
            }
            set
            {
                _corridorLastPressAction.Value = value;
            }
        }

        public OXTPressAction? BedroomLastPressAction
        {
            get
            {
                return _bedroomLastPressAction.Value;
            }
            set
            {
                _bedroomLastPressAction.Value = value;
            }
        }
    }
}