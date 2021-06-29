namespace VAR.WebFormsCore.Code
{
    public class Unit
    {
        private int _value;
        private UnitType _unitType;

        public Unit(int value, UnitType type)
        {
            _value = value;
            _unitType = type;
        }

        public override string ToString()
        {
            if (_unitType == UnitType.Pixel)
            {
                return string.Format("{0}px", _value);
            }

            if (_unitType == UnitType.Percentaje)
            {
                return string.Format("{0}%", _value);
            }

            return string.Empty;
        }
    }

    public enum UnitType
    {
        Pixel,
        Percentaje,
    }
}
