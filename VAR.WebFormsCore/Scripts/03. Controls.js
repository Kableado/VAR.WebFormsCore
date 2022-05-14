////////////////////////
//  CTextBox_SetText
//
var CTextBox_SetText = function (id, text) {
    var element = document.getElementById(id);
    element.value = text;
};

////////////////////////
//  CTextBox_Multiline_KeyDown
//
var CTextBox_Multiline_KeyDown = function (e) {
    if (e.keyCode === 9 || e.which === 9) {
        e.preventDefault();
        var s = this.selectionStart;
        this.value = this.value.substring(0, this.selectionStart) + "\t" + this.value.substring(this.selectionEnd);
        this.selectionEnd = s + 1;
    }
};

////////////////////////
//  CTextBox_Multiline_SaveSizeData
//
var CTextBox_Multiline_SaveSizeData = function (textArea) {
    var hidSizeData = document.getElementById(textArea.cfg.hidSize);
    hidSizeData.value = JSON.stringify(textArea.cfg.size);
};

////////////////////////
//  CTextBox_Multiline_RestoreSizeData
//
var CTextBox_Multiline_RestoreSizeData = function (textArea) {
    var hidSizeData = document.getElementById(textArea.cfg.hidSize);

    if (hidSizeData.value !== "") {
        textArea.cfg.size = JSON.parse(hidSizeData.value);
        if (textArea.cfg.size.width !== null) {
            textArea.style.width = textArea.cfg.size.width + "px";
        }
        if (textArea.cfg.size.height !== null) {
            textArea.style.height = textArea.cfg.size.height + "px";
        }
        if (textArea.cfg.size.scrollTop !== null) {
            textArea.scrollTop = textArea.cfg.size.scrollTop;
        }
    }
    textArea.cfg.size = { height: textArea.offsetHeight, width: textArea.offsetWidth, scrollTop: textArea.scrollTop };
};

////////////////////////
//  CTextBox_Multiline_MouseUp
//
var CTextBox_Multiline_MouseUp = function (e) {
    var textArea = e.target;
    var newSize = { height: textArea.offsetHeight, width: textArea.offsetWidth, scrollTop: textArea.scrollTop };
    if (textArea.cfg.size.height !== newSize.height) {
        textArea.cfg.size = newSize;
        CTextBox_Multiline_SaveSizeData(textArea);
    }
};

////////////////////////
//  CTextBox_Multiline_Scrolled
//
var CTextBox_Multiline_Scrolled = function (e) {
    var textArea = e.target;
    textArea.cfg.size = { height: textArea.offsetHeight, width: textArea.offsetWidth, scrollTop: textArea.scrollTop };
    CTextBox_Multiline_SaveSizeData(textArea);
};

////////////////////////
//  CTextBox_Multiline_Init
//
var CTextBox_Multiline_Init = function (cfg) {
    var textArea = document.getElementById(cfg.txtContent);
    textArea.cfg = cfg;

    textArea.onkeydown = CTextBox_Multiline_KeyDown;
    if (cfg.keepSize) {
        CTextBox_Multiline_RestoreSizeData(textArea);
        textArea.onmouseup = CTextBox_Multiline_MouseUp;
        textArea.onscroll = CTextBox_Multiline_Scrolled;
    }
};
