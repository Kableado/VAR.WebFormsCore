
function SendRequest(url, data, onData, onError) {
    var xhr = new XMLHttpRequest();
    if (data) {
        url += "?" + GetDataQueryString(data);
    }
    xhr.open("GET", url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                if (onData) {
                    onData(xhr.responseText);
                }
            } else {
                if (onError) {
                    onError();
                }
            }
        }
    }
    xhr.send(null);
}

function GetDataQueryString(data) {
    var queryString = "";
    for (var property in data) {
        if (data.hasOwnProperty(property)) {
            var value = data[property];
            queryString += (queryString.length > 0 ? "&" : "")
                + fixedEncodeURIComponent(property) + "="
                + fixedEncodeURIComponent(String(value));
        }
    }
    return queryString;
}

function SendData(url, data, onData, onError) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                if (onData) {
                    onData(xhr.responseText);
                }
            } else {
                if (onError) {
                    onError();
                }
            }
        }
    }
    xhr.setRequestHeader('Content-Type',
		'application/x-www-form-urlencoded');
    xhr.send(GetDataQueryString(data));
}

function GetFormQueryString(idForm) {
    var form = document.getElementById(idForm);
    var queryString = "";
    if (!form)
        return null;

    function appendVal(name, value) {
        queryString += (queryString.length > 0 ? "&" : "")
			+ fixedEncodeURIComponent(name) + "="
			+ fixedEncodeURIComponent(value ? value : "");
    }

    var elements = form.elements;
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        var elemType = element.type.toUpperCase();
        var elemName = element.name;

        if (elemName) {
            if (
				elemType.indexOf("TEXT") != -1 ||
				elemType.indexOf("TEXTAREA") != -1 ||
				elemType.indexOf("PASSWORD") != -1 ||
				elemType.indexOf("BUTTON") != -1 ||
				elemType.indexOf("HIDDEN") != -1 ||
				elemType.indexOf("SUBMIT") != -1 ||
				elemType.indexOf("IMAGE") != -1
			) {
                appendVal(elemName, element.value);
            } else if (elemType.indexOf("CHECKBOX") != -1 && element.checked) {
                appendVal(elemName, element.value ? element.value : "On");
            } else if (elemType.indexOf("RADIO") != -1 && element.checked) {
                appendVal(elemName, element.value);
            } else if (elemType.indexOf("SELECT") != -1) {
                for (var j = 0; j < element.options.length; j++) {
                    var option = element.options[j];
                    if (option.selected) {
                        appendVal(elemName,
                            option.value ? option.value : option.text);
                    }
                }
            }
        }
    }

    return queryString;
}

function SendForm(url, idForm, onData, onError) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                if (onData) {
                    onData(xhr.responseText);
                }
            } else {
                if (onError) {
                    onError();
                }
            }
        }
    }
    xhr.setRequestHeader('Content-Type',
		'application/x-www-form-urlencoded');
    xhr.send(GetFormQueryString(idForm));
}
