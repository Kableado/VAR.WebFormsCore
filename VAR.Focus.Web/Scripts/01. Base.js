
////////////////////////
//  GetElement
//
function GetElement(element) {
    if (typeof element == "string") {
        element = document.getElementById(element);
    }
    return element;
}

////////////////////////
//  ElementAddClass
//
function ElementAddClass(element, classname) {
    element = GetElement(element);
    if (!element) { return; }
    var cn = element.className;
    if (cn.indexOf(classname) != -1) {
        return;
    }
    if (cn != '') {
        classname = ' ' + classname;
    }
    element.className = cn + classname;
}

////////////////////////
//  ElementRemoveClass
//
function ElementRemoveClass(element, className) {
    element = GetElement(element);
    if (!element) { return; }
    var regex = new RegExp('(?:^|\\s)' + className + '(?!\\S)');
    if (regex.test(element.className)) {
        element.className = element.className.replace(regex, '');
    }
}

////////////////////////
//  ElementToggleClass
//
function ElementToggleClass(element, className) {
    element = GetElement(element);
    if (!element) { return; }
    var regex = new RegExp('(?:^|\\s)' + className + '(?!\\S)');
    if (regex.test(element.className)) {
        element.className = element.className.replace(regex, '');
        return true;
    } else {
        element.className = element.className + ' ' + className;
        return false;
    }
}


function escapeHTML(s) {
    return s.replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}

function fixedEncodeURIComponent(str) {
    return encodeURIComponent(str).replace(/[!'()*]/g, function (c) {
        return '%' + c.charCodeAt(0).toString(16);
    });
}


////////////////////////
// localStorage polyfill
//
if (!window.localStorage) {
    window.localStorage = {
        getItem: function (sKey) {
            if (!sKey || !this.hasOwnProperty(sKey)) { return null; }
            return unescape(document.cookie.replace(new RegExp("(?:^|.*;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*((?:[^;](?!;))*[^;]?).*"), "$1"));
        },
        key: function (nKeyId) { return unescape(document.cookie.replace(/\s*\=(?:.(?!;))*$/, "").split(/\s*\=(?:[^;](?!;))*[^;]?;\s*/)[nKeyId]); },
        setItem: function (sKey, sValue) {
            if (!sKey) { return; }
            document.cookie = escape(sKey) + "=" + escape(sValue) + "; path=/";
            this.length = document.cookie.match(/\=/g).length;
        },
        length: 0,
        removeItem: function (sKey) {
            if (!sKey || !this.hasOwnProperty(sKey)) { return; }
            var sExpDate = new Date();
            sExpDate.setDate(sExpDate.getDate() - 1);
            document.cookie = escape(sKey) + "=; expires=" + sExpDate.toGMTString() + "; path=/";
            this.length--;
        },
        hasOwnProperty: function (sKey) { return (new RegExp("(?:^|;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie); }
    };
    window.localStorage.length = (document.cookie.match(/\=/g) || window.localStorage).length;
}
