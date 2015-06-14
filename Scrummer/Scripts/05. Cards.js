var Card = function (cfg, idCard, title, body, x, y) {
    this.cfg = cfg;
    this.IDCard = idCard;
    this.Title = title;
    this.Body = body;
    this.X = x;
    this.Y = y;

    // Create DOM
    this.container = null;
    this.divCard = document.createElement("div");
    this.divCard.className = "divCard";
    this.divCard.style.left = x + "px";
    this.divCard.style.top = y + "px";

    this.divTitle = document.createElement("div");
    this.divCard.appendChild(this.divTitle);
    this.divTitle.className = "divTitle";
    this.divTitle.innerHTML = title;

    this.divBody = document.createElement("div");
    this.divCard.appendChild(this.divBody);
    this.divBody.className = "divBody";
    this.divBody.innerHTML = body;

    this.divOverlay = document.createElement("div");
    this.divCard.appendChild(this.divOverlay);
    this.divOverlay.className = "divOverlay";

    // Bind mouse event handlers
    this.bindedMouseDown = Card.prototype.MouseDown.bind(this);
    this.bindedMouseMove = Card.prototype.MouseMove.bind(this);
    this.bindedMouseUp = Card.prototype.MouseUp.bind(this);
    this.divOverlay.addEventListener("mousedown", this.bindedMouseDown, false);

    // Bind touch event handlers
    this.bindedTouchStart = Card.prototype.TouchStart.bind(this);
    this.bindedTouchMove = Card.prototype.TouchMove.bind(this);
    this.bindedTouchEnd = Card.prototype.TouchEnd.bind(this);
    this.divOverlay.addEventListener("touchstart", this.bindedTouchStart, false);

    // temporal variables for dragging and editing
    this.offsetX = 0;
    this.offsetY = 0;
    this.newX = this.X;
    this.newY = this.Y;
    this.newTitle = this.Title;
    this.newBody = this.Body;
};
Card.prototype = {
    InsertInContainer: function (container) {
        this.container = container;
        this.container.appendChild(this.divCard);
    },
    RemoveFromContainer: function(){
        this.container.removeChild(this.divCard);
    },
    Move: function (x, y) {
        this.X = x;
        this.Y = y;
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
    },
    Edit: function (title, body) {
        this.Title = title;
        this.Body = body;
        this.divTitle.innerHTML = this.Title;
        this.divBody.innerHTML = this.Body;
    },
    Reset: function () {
        this.newX = this.X;
        this.newY = this.Y;
        this.newTitle = this.Title;
        this.newBody = this.Body;
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.divTitle.innerHTML = this.Title;
        this.divBody.innerHTML = this.Body;
    },
    SetNew: function () {
        this.X = this.newX;
        this.Y = this.newY;
        this.Title = this.newTitle;
        this.Body = this.newBody;
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.divTitle.innerHTML = this.Title;
        this.divBody.innerHTML = this.Body;
    },
    OnMove: function () {
        if (this.X != this.newX || this.Y != this.newY) {
            var card = this;
            if (this.cfg.Connected == false) {
                card.Reset();
            }
            var data = {
                "IDBoard": this.cfg.IDBoard,
                "Command": "Move",
                "IDCard": this.IDCard,
                "X": this.newX,
                "Y": this.newY,
                "TimeStamp": new Date().getTime()
            };
            SendData(this.cfg.ServiceUrl, data,
                function (responseText) {
                    try {
                        var recvData = JSON.parse(responseText);
                        if (recvData && recvData instanceof Object && recvData.IsOK == true) {
                            card.SetNew();
                        } else {
                            card.Reset();
                        }
                    } catch (e) { }
                }, function () {
                    card.Reset();
                });
        }
    },
    GetRelativePosToContainer: function (pos) {
        var tempElem = this.container;
        var relPos = { x: pos.x, y: pos.y };
        while (tempElem) {
            relPos.x -= tempElem.offsetLeft;
            relPos.y -= tempElem.offsetTop;
            tempElem = tempElem.offsetParent;
        }
        return relPos;
    },
    MouseDown: function (evt) {
        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("mouseup", this.bindedMouseUp, false);
        document.addEventListener("mousemove", this.bindedMouseMove, false);

        evt.preventDefault();
        return false;
    },
    MouseMove: function (evt) {
        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        evt.preventDefault();
        return false;
    },
    MouseUp: function (evt) {
        document.removeEventListener("mouseup", this.bindedMouseUp, false);
        document.removeEventListener("mousemove", this.bindedMouseMove, false);

        this.OnMove();

        evt.preventDefault();
        return false;
    },
    TouchStart: function (evt) {
        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("touchend", this.bindedTouchEnd, false);
        document.addEventListener("touchcancel", this.bindedTouchEnd, false);
        document.addEventListener("touchmove", this.bindedTouchMove, false);

        evt.preventDefault();
        return false;
    },
    TouchMove: function (evt) {
        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        evt.preventDefault();
        return false;
    },
    TouchEnd: function (evt) {
        document.removeEventListener("touchend", this.bindedTouchEnd, false);
        document.removeEventListener("touchcancel", this.bindedTouchEnd, false);
        document.removeEventListener("touchmove", this.bindedTouchMove, false);

        this.OnMove(); 

        evt.preventDefault();
        return false;
    }
};

function RunCardBoard(cfg) {
    cfg.divBoard = GetElement(cfg.divBoard);

    cfg.Connected = false;

    cfg.Cards = [];

    var GetCardByID = function (idCard) {
        for (var i = 0, n = cfg.Cards.length; i < n; i++) {
            var card = cfg.Cards[i];
            if (card.IDCard == idCard) {
                return card;
            }
        }
        return null;
    };

    var ProcessCardCreateEvent = function(cardEvent){
        var card = new Card(cfg, cardEvent.IDCard, cardEvent.Title, cardEvent.Body, cardEvent.X, cardEvent.Y);
        cfg.Cards.push(card);
        card.InsertInContainer(cfg.divBoard);
    };

    var ProcessCardMoveEvent = function (cardEvent) {
        var card = GetCardByID(cardEvent.IDCard);
        if (card == null) { return; }
        card.Move(cardEvent.X, cardEvent.Y);
    };

    var ProcessCardEditEvent = function (cardEvent) {
        var card = GetCardByID(cardEvent.IDCard);
        if (card == null) { return; }
        card.Edit(cardEvent.Title, cardEvent.Body);
    };

    var ProcessCardDeleteEvent = function (cardEvent) {
        var card = GetCardByID(cardEvent.IDCard);
        if (card == null) { return; }
        card.RemoveFromContainer(cfg.divBoard);

    };

    var RequestCardEventData = function () {
        var ReciveCardEventData = function (responseText) {
            cfg.Connected = true;
            
            var recvData = JSON.parse(responseText);
            if (recvData && recvData instanceof Array) {
                for (var i = 0, n = recvData.length; i < n; i++) {
                    var cardEvent = recvData[i];
                    if (cardEvent.IDCardEvent > cfg.IDCardEvent) {
                        cfg.IDCardEvent = cardEvent.IDCardEvent;
                    }
                    if (cardEvent.EventType == "CardCreate") {
                        ProcessCardCreateEvent(cardEvent);
                    }
                    if (cardEvent.EventType == "CardMove") {
                        ProcessCardMoveEvent(cardEvent);
                    }
                    if (cardEvent.EventType == "CardEdit") {
                        ProcessCardEditEvent(cardEvent);
                    }
                    if (cardEvent.EventType == "CardDelete") {
                        ProcessCardDeleteEvent(cardEvent);
                    }
                }
            }

            // Reset pool
            window.setTimeout(function () {
                RequestCardEventData();
            }, 20);
        };
        var ErrorCardEventData = function () {
            cfg.Connected = false;

            // Retry
            window.setTimeout(function () {
                RequestCardEventData();
            }, 5000);
        };

        // Pool data
        var data = {
            "IDBoard": cfg.IDBoard,
            "IDCardEvent": cfg.IDCardEvent,
            "TimePoolData": ((cfg.Connected == false) ? "0" : String(cfg.TimePoolData)),
            "TimeStamp": new Date().getTime()
        };
        SendRequest(cfg.ServiceUrl, data, ReciveCardEventData, ErrorCardEventData);
    };
    RequestCardEventData();
}


