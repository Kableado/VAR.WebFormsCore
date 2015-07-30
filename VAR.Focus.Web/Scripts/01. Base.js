
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


