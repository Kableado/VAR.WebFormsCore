function RunChat(cfg) {
    cfg.divChat = GetElement(cfg.divChat);
    cfg.hidIDMessage = GetElement(cfg.hidIDMessage);
    cfg.hidUserName = GetElement(cfg.hidUserName);
    cfg.hidLastUser = GetElement(cfg.hidLastUser);

    var CreateMessageDOM = function (message, selfMessage, showUserName) {
        var divMessageRow = document.createElement("DIV");
        if (selfMessage) {
            divMessageRow.className = "selfMessageRow";
        } else {
            divMessageRow.className = "messageRow";
        }

        var divMessage = document.createElement("DIV");
        divMessage.className = "message";
        divMessageRow.appendChild(divMessage);

        if (showUserName) {
            var divUser = document.createElement("DIV");
            divUser.className = "user";
            divUser.innerHTML = escapeHTML(message.UserName);
            divMessage.appendChild(divUser);
        }

        var text = message.Text;

        var divText = document.createElement("DIV");
        divText.className = "text";
        divText.innerHTML = escapeHTML(text);
        divMessage.appendChild(divText);

        divMessage.title = new Date(message.Date);

        return divMessageRow;
    };

    var RequestChatData = function () {
        var requestUrl = cfg.ServiceUrl + "?idBoard=" + cfg.IDBoard + "&idMessage=" + cfg.hidIDMessage.value;
        var ReciveChatData = function (responseText) {

            recvMsgs = JSON.parse(responseText);
            if (recvMsgs) {
                var idMessage = parseInt(cfg.hidIDMessage.value);
                var frag = document.createDocumentFragment();
                for (var i = 0, n = recvMsgs.length; i < n; i++) {
                    var msg = recvMsgs[i];
                    if (idMessage < msg.IDMessage) {
                        cfg.hidIDMessage.value = msg.IDMessage;
                        idMessage = msg.IDMessage;
                        var elemMessage = CreateMessageDOM(msg,
                            (msg.UserName == cfg.hidUserName.value),
                            (cfg.hidLastUser.value !== msg.UserName));
                        cfg.hidLastUser.value = msg.UserName;
                        frag.appendChild(elemMessage);
                    }
                }
                cfg.divChat.appendChild(frag);
                cfg.divChat.scrollTop = cfg.divChat.scrollHeight;
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

function SendChat(cfg) {
    cfg.txtText = GetElement(cfg.txtText);
    cfg.hidUserName = GetElement(cfg.hidUserName);

    if (cfg.txtText.value.trim() == "") {
        return;
    }

    var data = {
        "text": cfg.txtText.value,
        "idBoard": cfg.IDBoard,
        "userName": cfg.hidUserName.value
    };
    cfg.txtText.value = "";
    SendData(cfg.ServiceUrl, data, null, null);
    cfg.txtText.focus();
}
