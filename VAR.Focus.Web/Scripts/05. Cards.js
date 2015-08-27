﻿
var CosineInterpolation = function (f) {
    return 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
};

var Toolbox = function (cfg, container) {
    this.cfg = cfg;
    this.X = 0;
    this.Y = 0;
    this.container = container;

    // Restore saved position
    this.StrKeyPosition = "Toolbox_" + cfg.IDBoard + "_Position";
    try {
        var pos = window.localStorage.getItem(this.StrKeyPosition);
        if (pos && (typeof pos === "string")) {
            pos = JSON.parse(pos);
            this.X = parseInt(pos.X);
            this.Y = parseInt(pos.Y);
        }
    } catch (e) {
        window.localStorage.removeItem(this.StrKeyPosition);
    }

    // Create DOM
    this.divToolbox = document.createElement("div");
    this.divToolbox.className = "divToolbox";
    this.divToolbox.style.left = this.X + "px";
    this.divToolbox.style.top = this.Y + "px";

    this.divTitle = document.createElement("div");
    this.divToolbox.appendChild(this.divTitle);
    this.divTitle.className = "divTitle";
    this.divTitle.innerHTML = cfg.Texts.Toolbox;

    this.btnAdd = document.createElement("button");
    this.divToolbox.appendChild(this.btnAdd);
    this.btnAdd.className = "btnToolbox";
    this.btnAdd.innerHTML = cfg.Texts.AddCard;
    this.btnAdd.addEventListener("click", Toolbox.prototype.btnAdd_Click.bind(this), false);

    this.divOverlay = document.createElement("div");
    this.divToolbox.appendChild(this.divOverlay);
    this.divOverlay.className = "divOverlay";

    this.container.appendChild(this.divToolbox);

    // Bind mouse event handlers
    this.bindedMouseDown = Toolbox.prototype.MouseDown.bind(this);
    this.bindedMouseMove = Toolbox.prototype.MouseMove.bind(this);
    this.bindedMouseUp = Toolbox.prototype.MouseUp.bind(this);
    this.divOverlay.addEventListener("mousedown", this.bindedMouseDown, false);

    // Bind touch event handlers
    this.bindedTouchStart = Toolbox.prototype.TouchStart.bind(this);
    this.bindedTouchMove = Toolbox.prototype.TouchMove.bind(this);
    this.bindedTouchEnd = Toolbox.prototype.TouchEnd.bind(this);
    this.divOverlay.addEventListener("touchstart", this.bindedTouchStart, false);

    // temporal variables for dragging, editing and deleting
    this.offsetX = 0;
    this.offsetY = 0;
};
Toolbox.prototype = {
    ReInsertOnContainer: function () {
        this.container.removeChild(this.divToolbox);
        this.container.appendChild(this.divToolbox);
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
    SetPosition: function(x, y){
        this.X = x;
        this.Y = y;
        this.divToolbox.style.left = x + "px";
        this.divToolbox.style.top = y + "px";
    },
    SavePosition: function(){
        var pos = { X: this.X, Y: this.Y };
        window.localStorage.setItem(this.StrKeyPosition, JSON.stringify(pos));
    },
    MouseDown: function (evt) {
        evt.preventDefault();
        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.offsetX = pos.x - this.divToolbox.offsetLeft;
        this.offsetY = pos.y - this.divToolbox.offsetTop;
        document.addEventListener("mouseup", this.bindedMouseUp, false);
        document.addEventListener("mousemove", this.bindedMouseMove, false);
        return false;
    },
    MouseMove: function (evt) {
        evt.preventDefault();
        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.SetPosition(parseInt(pos.x - this.offsetX), parseInt(pos.y - this.offsetY));
        return false;
    },
    MouseUp: function (evt) {
        evt.preventDefault();
        this.SavePosition();
        document.removeEventListener("mouseup", this.bindedMouseUp, false);
        document.removeEventListener("mousemove", this.bindedMouseMove, false);
        return false;
    },
    TouchStart: function (evt) {
        evt.preventDefault();
        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.offsetX = pos.x - this.divToolbox.offsetLeft;
        this.offsetY = pos.y - this.divToolbox.offsetTop;
        document.addEventListener("touchend", this.bindedTouchEnd, false);
        document.addEventListener("touchcancel", this.bindedTouchEnd, false);
        document.addEventListener("touchmove", this.bindedTouchMove, false);
        return false;
    },
    TouchMove: function (evt) {
        evt.preventDefault();
        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.SetPosition(parseInt(pos.x - this.offsetX), parseInt(pos.y - this.offsetY));
        return false;
    },
    TouchEnd: function (evt) {
        evt.preventDefault();
        this.SavePosition();
        document.removeEventListener("touchend", this.bindedTouchEnd, false);
        document.removeEventListener("touchcancel", this.bindedTouchEnd, false);
        document.removeEventListener("touchmove", this.bindedTouchMove, false);
        return false;
    },
    btnAdd_Click: function (evt) {
        evt.preventDefault();
        var pos = { x: 0, y: 0 };
        pos.x += this.divToolbox.offsetLeft;
        pos.y += this.divToolbox.offsetTop + this.divToolbox.offsetHeight;
        var card = new Card(this.cfg, 0, "", "", pos.x, pos.y);
        card.InsertOnContainer(this.cfg.divBoard);
        card.EnterEditionMode();
        return false;
    }
};

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
    this.divTitle.innerHTML = this.FilterText(title);

    this.divBody = document.createElement("div");
    this.divCard.appendChild(this.divBody);
    this.divBody.className = "divBody";
    this.divBody.innerHTML = this.FilterText(body);

    this.divOverlay = document.createElement("div");
    this.divCard.appendChild(this.divOverlay);
    this.divOverlay.className = "divOverlay";

    this.btnEdit = document.createElement("button");
    this.divCard.appendChild(this.btnEdit);
    this.btnEdit.className = "btnCard btnEdit";
    this.btnEdit.innerHTML = "E";
    this.btnEdit.addEventListener("click", Card.prototype.btnEdit_Click.bind(this), false);

    this.btnDelete = document.createElement("button");
    this.divCard.appendChild(this.btnDelete);
    this.btnDelete.className = "btnCard btnDelete";
    this.btnDelete.innerHTML = "X";
    this.btnDelete.addEventListener("click", Card.prototype.btnDelete_Click.bind(this), false);

    this.txtTitle = document.createElement("input");
    this.txtTitle.className = "txtTitle";

    this.txtBody = document.createElement("textarea");
    this.txtBody.className = "txtBody";

    this.btnAcceptEdit = document.createElement("button");
    this.btnAcceptEdit.className = "btnCard";
    this.btnAcceptEdit.innerHTML = this.cfg.Texts.Accept;
    this.btnAcceptEdit.addEventListener("click", Card.prototype.btnAcceptEdit_Click.bind(this), false);

    this.btnCancelEdit = document.createElement("button");
    this.btnCancelEdit.className = "btnCard";
    this.btnCancelEdit.innerHTML = this.cfg.Texts.Cancel;
    this.btnCancelEdit.addEventListener("click", Card.prototype.btnCancelEdit_Click.bind(this), false);

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

    // Temporal variables for dragging, editing and deleting
    this.offsetX = 0;
    this.offsetY = 0;
    this.newX = this.X;
    this.newY = this.Y;
    this.newTitle = this.Title;
    this.newBody = this.Body;
    this.Editing = false;

    // Selfinsert
    if (this.IDCard > 0) {
        this.cfg.Cards.push(this);
    }
    this.InsertOnContainer(this.cfg.divBoard);
};
Card.prototype = {
    FilterText: function (text) {
        text = text.split("  ").join(" &nbsp;");
        text = text.split("&nbsp; &nbsp;").join("&nbsp;&nbsp;&nbsp;");
        text = text.split("\n").join("<br />");
        return text;
    },
    InsertOnContainer: function (container) {
        this.container = container;
        this.container.appendChild(this.divCard);
        this.cfg.Toolbox.ReInsertOnContainer();
    },
    RemoveFromContainer: function(){
        this.container.removeChild(this.divCard);
    },
    MoveFrame: function () {
        if (this.animData) {
            var f = ((+new Date()) - this.animData.startTime) / this.animData.time;
            if (f < 1.0) {
                f = CosineInterpolation(f);
                f = CosineInterpolation(f);
                f = CosineInterpolation(f);
                var f2 = 1 - f;
                var x = this.animData.X1 * f + this.animData.X0 * f2;
                var y = this.animData.Y1 * f + this.animData.Y0 * f2;
                this.divCard.style.left = x + "px";
                this.divCard.style.top = y + "px";
                this.animData.animationID = window.setTimeout(this.bindedMoveFrame, 16);
                return;
            }
        }
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.animData = null;
    },
    Move: function (x, y) {
        this.OnMoveStart();
        this.X = x;
        this.Y = y;
        this.newX = x;
        this.newY = y;
        this.animData = {
            X0: parseInt(this.divCard.style.left),
            Y0: parseInt(this.divCard.style.top),
            X1: x,
            Y1: y,
            time: this.cfg.TimeMoveAnimation,
            startTime: +new Date(),
            animationID: 0
        };
        this.bindedMoveFrame = Card.prototype.MoveFrame.bind(this);
        this.animData.animationID = window.setTimeout(this.bindedMoveFrame, 16);
    },
    Edit: function (title, body) {
        if (this.Editing) {
            this.ExitEditionMode();
        }
        this.Title = title;
        this.Body = body;
        this.newTitle = title;
        this.newBody = body;
        this.divTitle.innerHTML = this.FilterText(this.Title);
        this.divBody.innerHTML = this.FilterText(this.Body);
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);
    },
    Reset: function () {
        this.newX = this.X;
        this.newY = this.Y;
        this.newTitle = this.Title;
        this.newBody = this.Body;
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.divTitle.innerHTML = this.FilterText(this.Title);
        this.divBody.innerHTML = this.FilterText(this.Body);
    },
    SetNew: function () {
        this.X = this.newX;
        this.Y = this.newY;
        this.Title = this.newTitle;
        this.Body = this.newBody;
        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.divTitle.innerHTML = this.FilterText(this.Title);
        this.divBody.innerHTML = this.FilterText(this.Body);
    },
    Hide: function () {
        this.divCard.style.display = "none";
    },
    Show: function () {
        this.divCard.style.display = "";
    },
    OnMoveStart: function () {
        if (this.animData) {
            window.clearTimeout(this.animData.animationID);
            this.animData = null;
        }
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);
    },
    OnMove: function () {
        var card = this;
        if (this.cfg.Connected == false) {
            card.Reset();
            return;
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
    },
    OnEdit: function () {
        if (this.Title != this.newTitle || this.Body != this.newBody) {
            var card = this;
            if (this.cfg.Connected == false) {
                card.Reset();
                return;
            }
            var data = {
                "IDBoard": this.cfg.IDBoard,
                "Command": "Edit",
                "IDCard": this.IDCard,
                "Title": this.newTitle,
                "Body": this.newBody,
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
    OnDelete: function () {
        var card = this;
        this.Hide();
        if (this.cfg.Connected == false) {
            this.Show();
            return;
        }
        if (this.IDCard == 0) {
            this.RemoveFromContainer();
            this.cfg.RemoveCardByID(card.IDCard);
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "Delete",
            "IDCard": this.IDCard,
            "TimeStamp": new Date().getTime()
        };
        SendData(this.cfg.ServiceUrl, data,
            function (responseText) {
                try {
                    var recvData = JSON.parse(responseText);
                    if (recvData && recvData instanceof Object && recvData.IsOK == true) {
                        card.RemoveFromContainer();
                        if (card.IDCard > 0) {
                            card.cfg.RemoveCardByID(card.IDCard);
                        }
                    } else {
                        card.Show();
                    }
                } catch (e) { }
            }, function () {
                card.Show();
            });
    },
    OnCreate: function () {
        var card = this;
        if (this.cfg.Connected == false) {
            card.OnDelete();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "Create",
            "X": this.X,
            "Y": this.Y,
            "Title": this.Title,
            "Body": this.Body,
            "TimeStamp": new Date().getTime()
        };
        SendData(this.cfg.ServiceUrl, data,
            function (responseText) {
                try {
                    var recvData = JSON.parse(responseText);
                    if (recvData && recvData instanceof Object && recvData.IsOK == true) {
                        //card.IDCard = parseInt(recvData.ReturnValue);
                        card.OnDelete();
                    } else {
                        card.OnDelete();
                    }
                } catch (e) { }
            }, function () {
                card.OnDelete();
            });
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
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("mouseup", this.bindedMouseUp, false);
        document.addEventListener("mousemove", this.bindedMouseMove, false);

        return false;
    },
    MouseMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        return false;
    },
    MouseUp: function (evt) {
        evt.preventDefault();

        document.removeEventListener("mouseup", this.bindedMouseUp, false);
        document.removeEventListener("mousemove", this.bindedMouseMove, false);

        this.OnMove();

        return false;
    },
    TouchStart: function (evt) {
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("touchend", this.bindedTouchEnd, false);
        document.addEventListener("touchcancel", this.bindedTouchEnd, false);
        document.addEventListener("touchmove", this.bindedTouchMove, false);

        return false;
    },
    TouchMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        return false;
    },
    TouchEnd: function (evt) {
        evt.preventDefault();

        document.removeEventListener("touchend", this.bindedTouchEnd, false);
        document.removeEventListener("touchcancel", this.bindedTouchEnd, false);
        document.removeEventListener("touchmove", this.bindedTouchMove, false);

        this.OnMove(); 

        return false;
    },
    EnterEditionMode: function(){
        this.divTitle.innerHTML = "";
        this.txtTitle.value = this.Title;
        this.divTitle.appendChild(this.txtTitle);

        this.divBody.innerHTML = "";
        this.txtBody.value = this.Body;
        this.divBody.appendChild(this.txtBody);
        this.divBody.appendChild(document.createElement("br"));
        this.divBody.appendChild(this.btnAcceptEdit);
        this.divBody.appendChild(this.btnCancelEdit);

        this.divOverlay.style.display = "none";
        this.Editing = true;
    },
    ExitEditionMode: function(){
        this.divTitle.removeChild(this.txtTitle);
        this.divBody.removeChild(this.txtBody);
        this.divBody.removeChild(this.btnAcceptEdit);
        this.divBody.removeChild(this.btnCancelEdit);
        this.divOverlay.style.display = "";
        this.Editing = false;
    },
    btnEdit_Click: function (evt) {
        evt.preventDefault();
        if (this.Editing == false) {
            this.EnterEditionMode();
        } else {
            this.ExitEditionMode();
            this.Reset();
        }
        return false;
    },
    btnAcceptEdit_Click: function (evt) {
        evt.preventDefault();
        this.newTitle = this.txtTitle.value;
        this.newBody = this.txtBody.value;
        this.ExitEditionMode();
        this.divTitle.innerHTML = this.FilterText(this.newTitle);
        this.divBody.innerHTML = this.FilterText(this.newBody);
        if (this.IDCard > 0) {
            this.OnEdit();
        } else {
            this.Title = this.newTitle;
            this.Body = this.newBody;
            this.OnCreate();
        }
        return false;
    },
    btnCancelEdit_Click: function (evt) {
        evt.preventDefault();
        this.ExitEditionMode();
        this.Reset();
        if (this.IDCard == 0) {
            this.OnDelete();
        }
        return false;
    },
    btnDelete_Click: function (evt) {
        evt.preventDefault();
        if (this.IDCard==0 || confirm(this.cfg.Texts.ConfirmDelete)) {
            this.OnDelete();
        }
        return false;
    }
};

function RunCardBoard(cfg) {
    cfg.divBoard = GetElement(cfg.divBoard);

    cfg.Toolbox = new Toolbox(cfg, cfg.divBoard);

    cfg.Connected = false;

    cfg.Cards = [];

    cfg.GetCardByID = function (idCard) {
        for (var i = 0, n = this.Cards.length; i < n; i++) {
            var card = this.Cards[i];
            if (card.IDCard == idCard) {
                return card;
            }
        }
        return null;
    };
    cfg.RemoveCardByID = function (idCard) {
        for (var i = 0, n = this.Cards.length; i < n; i++) {
            var card = this.Cards[i];
            if (card.IDCard == idCard) {
                this.Cards.splice(i, 1);
            }
        }
        return false;
    }

    var ProcessCardCreateEvent = function(cardEvent){
        var card = new Card(cfg, cardEvent.IDCard, cardEvent.Title, cardEvent.Body, cardEvent.X, cardEvent.Y);
    };

    var ProcessCardMoveEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card == null) { return; }
        card.Move(cardEvent.X, cardEvent.Y);
    };

    var ProcessCardEditEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card == null) { return; }
        card.Edit(cardEvent.Title, cardEvent.Body);
    };

    var ProcessCardDeleteEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
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
            }, cfg.TimeRefresh);
        };
        var ErrorCardEventData = function () {
            cfg.Connected = false;

            // Retry
            window.setTimeout(function () {
                RequestCardEventData();
            }, cfg.TimeRefreshDisconnected);
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