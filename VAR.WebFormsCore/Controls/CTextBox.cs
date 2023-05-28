using System;
using System.Collections.Generic;
using System.Text;
using VAR.Json;

namespace VAR.WebFormsCore.Controls;

public class CTextBox : Control, INamingContainer, IValidableControl
{
    #region Declarations

    private readonly TextBox _txtContent = new();

    private HiddenField? _hidSize;

    private const string CssClassBase = "textBox";
    private string _cssClassExtra = "";

    private bool _allowEmpty = true;

    private string _placeHolder = string.Empty;

    private bool _markedInvalid;

    private Control? _nextFocusOnEnter;

    private bool _keepSize;

    #endregion Declarations

    #region Properties

    public string CssClassExtra
    {
        get => _cssClassExtra;
        set => _cssClassExtra = value;
    }

    public bool AllowEmpty
    {
        get => _allowEmpty;
        set => _allowEmpty = value;
    }

    public string PlaceHolder
    {
        get => _placeHolder;
        set => _placeHolder = value;
    }

    public bool MarkedInvalid
    {
        get => _markedInvalid;
        set => _markedInvalid = value;
    }

    public Control? NextFocusOnEnter
    {
        get => _nextFocusOnEnter;
        set => _nextFocusOnEnter = value;
    }

    public bool KeepSize
    {
        get => _keepSize;
        set => _keepSize = value;
    }

    public string Text
    {
        get => _txtContent.Text;
        set => _txtContent.Text = value;
    }

    public TextBoxMode TextMode
    {
        get => _txtContent.TextMode;
        set => _txtContent.TextMode = value;
    }

    #endregion Properties

    #region Control life cycle

    public CTextBox()
    {
        Init += CTextBox_Init;
        PreRender += CTextBox_PreRender;
    }

    private void CTextBox_Init(object? sender, EventArgs e)
    {
        Controls.Add(_txtContent);

        if (TextMode == TextBoxMode.MultiLine)
        {
            if (_keepSize)
            {
                _hidSize = new HiddenField();
                Controls.Add(_hidSize);
            }

            string strCfgName = $"{ClientID}_cfg";
            Dictionary<string, object> cfg = new()
            {
                {"txtContent", _txtContent.ClientID}, {"hidSize", _hidSize?.ClientID ?? string.Empty}, {"keepSize", _keepSize},
            };
            StringBuilder sbCfg = new StringBuilder();
            sbCfg.AppendFormat("<script>\n");
            sbCfg.Append($"var {strCfgName} = {JsonWriter.WriteObject(cfg)};\n");
            sbCfg.Append($"CTextBox_Multiline_Init({strCfgName});\n");
            sbCfg.AppendFormat("</script>\n");
            LiteralControl liScript = new LiteralControl(sbCfg.ToString());
            Controls.Add(liScript);
        }
    }

    private void CTextBox_PreRender(object? sender, EventArgs e)
    {
        _txtContent.CssClass = CssClassBase;
        if (string.IsNullOrEmpty(_cssClassExtra) == false)
        {
            _txtContent.CssClass = $"{CssClassBase} {_cssClassExtra}";
        }

        if (Page?.IsPostBack == true && (_allowEmpty == false && IsEmpty()) || _markedInvalid)
        {
            _txtContent.CssClass += " textBoxInvalid";
        }

        _txtContent.Attributes.Add("onchange", "ElementRemoveClass(this, 'textBoxInvalid');");

        if (string.IsNullOrEmpty(_placeHolder) == false)
        {
            _txtContent.Attributes.Add("placeholder", _placeHolder);
        }

        if (_nextFocusOnEnter != null)
        {
            _txtContent.Attributes.Add(
                "onkeydown",
                $"if(event.keyCode==13){{document.getElementById('{_nextFocusOnEnter.ClientID}').focus(); return false;}}"
            );
        }
    }

    #endregion Control life cycle

    #region Public methods

    private bool IsEmpty() { return string.IsNullOrEmpty(_txtContent.Text); }

    public bool IsValid() { return _allowEmpty || (string.IsNullOrEmpty(_txtContent.Text) == false); }

    public int? GetClientsideHeight()
    {
        if (string.IsNullOrEmpty(_hidSize?.Value)) { return null; }

        JsonParser jsonParser = new JsonParser();
        Dictionary<string, object>? sizeObj = jsonParser.Parse(_hidSize?.Value) as Dictionary<string, object>;
        if (sizeObj == null) { return null; }

        if (sizeObj.ContainsKey("height") == false) { return null; }

        return (int) sizeObj["height"];
    }

    public void SetClientsideHeight(int? height)
    {
        if (height == null)
        {
            if (_hidSize != null)
            {
                _hidSize.Value = string.Empty;
            }
            return;
        }

        Dictionary<string, object?>? sizeObj = null;
        if (string.IsNullOrEmpty(_hidSize?.Value) == false)
        {
            JsonParser jsonParser = new JsonParser();
            sizeObj = jsonParser.Parse(_hidSize?.Value) as Dictionary<string, object?>;
        }
        sizeObj ??= new Dictionary<string, object?> { { "height", height }, { "width", null }, { "scrollTop", null }, };

        if (_hidSize != null)
        {
            _hidSize.Value = JsonWriter.WriteObject(sizeObj);
        }
    }

    #endregion Public methods
}