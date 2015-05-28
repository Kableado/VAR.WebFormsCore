function RunChat(serviceUrl, divContainer, idBoard, hidIDMessage, hidUserName, hidLastUser) {
    divContainer = GetElement(divContainer);
    hidIDMessage = GetElement(hidIDMessage);
    hidUserName = GetElement(hidUserName);
    hidLastUser = GetElement(hidLastUser);

    var CreateMessageDOM = function (message, selfMessage, hidLastUser) {
        var divMessageRow = document.createElement("DIV");
        if (selfMessage) {
            divMessageRow.className = "selfMessageRow";
        } else {
            divMessageRow.className = "messageRow";
        }

        var divMessage = document.createElement("DIV");
        divMessage.className = "message";
        divMessageRow.appendChild(divMessage);

        if (hidLastUser.value !== message.UserName) {
            var divUser = document.createElement("DIV");
            divUser.className = "user";
            divUser.innerHTML = escapeHTML(message.UserName);
            divMessage.appendChild(divUser);
            hidLastUser.value = message.UserName;
        }

        var text = message.Text;

        var divText = document.createElement("DIV");
        divText.className = "text";
        divText.innerHTML = escapeHTML(text);
        divMessage.appendChild(divText);

        return divMessageRow;
    };

    var RequestChatData = function () {
        var requestUrl = serviceUrl + "?idBoard=" + idBoard + "&idMessage=" + hidIDMessage.value;
        var ReciveChatData = function (responseText) {

            recvMsgs = JSON.parse(responseText);
            if (recvMsgs) {
                var idMessage = parseInt(hidIDMessage.value);
                var frag = document.createDocumentFragment();
                for (var i = 0, n = recvMsgs.length; i < n; i++) {
                    var msg = recvMsgs[i];
                    if (idMessage < msg.IDMessage) {
                        hidIDMessage.value = msg.IDMessage;
                        idMessage = msg.IDMessage;
                        var elemMessage = CreateMessageDOM(msg, (msg.UserName == hidUserName.value), hidLastUser);
                        frag.appendChild(elemMessage);
                    }
                }
                divContainer.appendChild(frag);
                divContainer.scrollTop = divContainer.scrollHeight;
            }

            // Reset pool
            window.setTimeout(function () {
                RequestChatData();
            }, 20);
        };
        var ErrorChatData = function () {

            // Retry
            window.setTimeout(function () {
                RequestChatData();
            }, 5000);
        };

        // Pool data
        SendRequest(requestUrl, ReciveChatData, ErrorChatData);
    };
    RequestChatData();
}

function SendChat(serviceUrl, txtText, idBoard, hidUserName) {
    txtText = GetElement(txtText);
    hidUserName = GetElement(hidUserName);

    if (txtText.value.trim() == "") {
        return;
    }

    var data = {
        "text": txtText.value,
        "idBoard": idBoard,
        "userName": hidUserName.value
    };
    txtText.value = "";
    SendData(serviceUrl, data, null, null);
    txtText.focus();
}