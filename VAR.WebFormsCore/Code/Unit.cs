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
    }

    public enum UnitType
    {
        Pixel,
        Percentaje,
    }
}
