namespace VAR.WebFormsCore.Code;

public class Unit
{
    private readonly int _value;
    private readonly UnitType _unitType;

    public Unit(int value, UnitType type)
    {
        _value = value;
        _unitType = type;
    }

    public override string ToString()
    {
        if (_unitType == UnitType.Pixel) { return $"{_value}px"; }

        if (_unitType == UnitType.Percentage) { return $"{_value}%"; }

        return string.Empty;
    }
}

public enum UnitType
{
    Pixel,
    Percentage,
}