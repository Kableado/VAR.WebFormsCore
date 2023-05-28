using System.Collections.Generic;

namespace VAR.WebFormsCore.Controls;

public class ControlCollection : List<Control>
{
    private readonly Control _parent;
    private int _index;

    public ControlCollection(Control parent) { _parent = parent; }

    public new void Add(Control control)
    {
        control.Page = _parent.Page;
        control.Parent = _parent;
        control.Index = _index;
        _index++;
        base.Add(control);
    }
}