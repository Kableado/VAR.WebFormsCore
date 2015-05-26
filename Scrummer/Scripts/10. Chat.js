function RunChat(divContainer, idBoard, hidIDMessage) {
    divContainer = GetElement(divContainer);
    hidIDMessage = GetElement(hidIDMessage);

    var CreateMessageDOM = function (message) {
        var divMessageRow = document.createElement("DIV");
        divMessageRow.className = "messageRow";

        var divMessage = document.createElement("DIV");
        divMessage.className = "message";
        divMessageRow.appendChild(divMessage);

        var divUser = document.createElement("DIV");
        divUser.className = "user";
        divUser.innerHTML = escapeHTML(message.UserName);
        divMessage.appendChild(divUser);

        var text = message.Text;

        var divText = document.createElement("DIV");
        divText.className = "text";
        divText.innerHTML = escapeHTML(text);
        divMessage.appendChild(divText);

        return divMessageRow;
    };

    var RequestChatData = function () {
        var requestUrl = "ChatHandler?idBoard=" + idBoard + "&idMessage=" + hidIDMessage.value;
        var ReciveChatData = function (responseText) {

            recvMsgs = JSON.parse(responseText);
            if (recvMsgs) {
                var idMessage = parseInt(hidIDMessage.value);
                for (var i = 0, n = recvMsgs.length; i < n; i++) {
                    var msg = recvMsgs[i];
                    if (idMessage < msg.IDMessage) {
                        hidIDMessage.value = msg.IDMessage;
                        idMessage = msg.IDMessage;
                        var elemMessage = CreateMessageDOM(msg);
                        divContainer.appendChild(elemMessage);
                    }
                }
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

function SendChat(txtText, idBoard) {
    txtText = GetElement(txtText);
    var data = {
        "text": txtText.value,
        "idBoard": idBoard,
        "userName": "VAR"
    };
    txtText.value = "";
    SendData("ChatHandler", data, null, null);
}