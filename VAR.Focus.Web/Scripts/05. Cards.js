

var Toolbox = function (cfg, container) {
    this.cfg = cfg;
    this.X = 0;
    this.Y = 0;
    this.container = container;

    // Restore saved position
    this.StrKeyPosition = "Toolbox_" + cfg.IDBoard + "_Position";
    try {
        var pos = window.localStorage.getItem(this.StrKeyPosition);
        if (pos && typeof pos === "string") {
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

    this.btnAddCard = document.createElement("button");
    this.divToolbox.appendChild(this.btnAddCard);
    this.btnAddCard.className = "btnToolbox";
    this.btnAddCard.innerHTML = cfg.Texts.AddCard;
    this.btnAddCard.addEventListener("click", Toolbox.prototype.btnAddCard_Click.bind(this), false);

    this.btnAddRegion = document.createElement("button");
    this.divToolbox.appendChild(this.btnAddRegion);
    this.btnAddRegion.className = "btnToolbox";
    this.btnAddRegion.innerHTML = cfg.Texts.AddRegion;
    this.btnAddRegion.addEventListener("click", Toolbox.prototype.btnAddRegion_Click.bind(this), false);

    this.btnEditBoard = document.createElement("button");
    this.divToolbox.appendChild(this.btnEditBoard);
    this.btnEditBoard.className = "btnToolbox";
    this.btnEditBoard.innerHTML = cfg.Texts.EditBoard;
    this.btnEditBoard.addEventListener("click", Toolbox.prototype.btnEditBoard_Click.bind(this), false);

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
    SetPosition: function (x, y) {
        if (x < 0) { x = 0; }
        if (y < 0) { y = 0; }
        this.X = x;
        this.Y = y;
        this.divToolbox.style.left = x + "px";
        this.divToolbox.style.top = y + "px";
    },
    SavePosition: function () {
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
    btnAddCard_Click: function (evt) {
        evt.preventDefault();
        var pos = { x: 0, y: 0 };
        pos.x += this.divToolbox.offsetLeft;
        pos.y += this.divToolbox.offsetTop + this.divToolbox.offsetHeight;
        var card = new Card(this.cfg, 0, "", "", pos.x, pos.y, this.cfg.DefaultCardWidth, this.cfg.DefaultCardHeight);
        card.InsertOnContainer(this.cfg.divBoard);
        card.EnterEditionMode();
        return false;
    },
    btnAddRegion_Click: function (evt) {
        evt.preventDefault();
        var pos = { x: 0, y: 0 };
        pos.x += this.divToolbox.offsetLeft;
        pos.y += this.divToolbox.offsetTop + this.divToolbox.offsetHeight;
        var region = new Region(this.cfg, 0, "", pos.x, pos.y, this.cfg.DefaultRegionWidth, this.cfg.DefaultRegionHeight);
        region.InsertOnContainer(this.cfg.divBoard);
        region.EnterEditionMode();
        return false;
    },
    btnEditBoard_Click: function (evt) {
        evt.preventDefault();
        window.location.href = this.cfg.EditBoardUrl;
        return false;
    },
    empty: null
};

var Card = function (cfg, idCard, title, body, x, y, width, height, locked) {
    this.cfg = cfg;
    this.IDCard = idCard;
    this.Title = title;
    this.Body = body;
    this.X = x;
    this.Y = y;
    this.Width = width;
    this.Height = height;
    this.Locked = locked;

    // Create DOM
    this.container = null;
    this.divCard = document.createElement("div");
    this.divCard.className = "divCard";
    this.divCard.style.left = x + "px";
    this.divCard.style.top = y + "px";
    this.divCard.style.width = width + "px";
    this.divCard.style.height = height + "px";

    this.divTitle = document.createElement("div");
    this.divCard.appendChild(this.divTitle);
    this.divTitle.className = "divTitle";

    this.txtTitle = document.createElement("input");
    this.txtTitle.className = "txtTitle";
    this.txtTitle.value = this.Title;
    this.divTitle.appendChild(this.txtTitle);

    this.divBody = document.createElement("div");
    this.divCard.appendChild(this.divBody);
    this.divBody.className = "divBody";

    this.txtBody = document.createElement("textarea");
    this.txtBody.className = "txtBody";
    this.txtBody.value = this.Body;
    this.divBody.appendChild(this.txtBody);

    this.divOverlay = document.createElement("div");
    this.divCard.appendChild(this.divOverlay);
    this.divOverlay.className = "divOverlay";
    this.divOverlay_MouseDownBinded = Card.prototype.divOverlay_MouseDown.bind(this);
    this.divOverlay_MouseMoveBinded = Card.prototype.divOverlay_MouseMove.bind(this);
    this.divOverlay_MouseUpBinded = Card.prototype.divOverlay_MouseUp.bind(this);
    this.divOverlay.addEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
    this.divOverlay_TouchStartBinded = Card.prototype.divOverlay_TouchStart.bind(this);
    this.divOverlay_TouchMoveBinded = Card.prototype.divOverlay_TouchMove.bind(this);
    this.divOverlay_TouchEndBinded = Card.prototype.divOverlay_TouchEnd.bind(this);
    this.divOverlay.addEventListener("touchstart", this.divOverlay_TouchStartBinded, false);

    this.btnEdit = document.createElement("button");
    this.divCard.appendChild(this.btnEdit);
    this.btnEdit.className = "btnCard btnEdit";
    this.btnEdit.innerHTML = "&#x270E;"; // Unicode "Lower Right Pencil"
    this.btnEdit.addEventListener("click", Card.prototype.btnEdit_Click.bind(this), false);

    this.btnDelete = document.createElement("button");
    this.divCard.appendChild(this.btnDelete);
    this.btnDelete.className = "btnCard btnDelete";
    this.btnDelete.innerHTML = "X"; 
    this.btnDelete.addEventListener("click", Card.prototype.btnDelete_Click.bind(this), false);

    this.btnLock = document.createElement("button");
    this.divCard.appendChild(this.btnLock);
    this.btnLock.className = "btnCard btnLock";
    this.btnLock.innerHTML = "&#x1F512;"; // Unicode "Lock"
    this.btnLock.addEventListener("click", Region.prototype.btnLock_Click.bind(this), false);

    this.btnUnlock = document.createElement("button");
    this.divCard.appendChild(this.btnUnlock);
    this.btnUnlock.className = "btnCard btnUnlock";
    this.btnUnlock.innerHTML = "&#x1F513;"; // Unicode "Open Lock"
    this.btnUnlock.style.display = "none";
    this.btnUnlock.addEventListener("click", Region.prototype.btnUnlock_Click.bind(this), false);

    this.divResize = document.createElement("div");
    this.divCard.appendChild(this.divResize);
    this.divResize.className = "divResize";
    this.divResize_MouseDownBinded = Card.prototype.divResize_MouseDown.bind(this);
    this.divResize_MouseMoveBinded = Card.prototype.divResize_MouseMove.bind(this);
    this.divResize_MouseUpBinded = Card.prototype.divResize_MouseUp.bind(this);
    this.divResize.addEventListener("mousedown", this.divResize_MouseDownBinded, false);
    this.divResize_TouchStartBinded = Card.prototype.divResize_TouchStart.bind(this);
    this.divResize_TouchMoveBinded = Card.prototype.divResize_TouchMove.bind(this);
    this.divResize_TouchEndBinded = Card.prototype.divResize_TouchEnd.bind(this);
    this.divResize.addEventListener("touchstart", this.divResize_TouchStartBinded, false);

    // Temporal variables for actions
    this.offsetX = 0;
    this.offsetY = 0;
    this.newX = this.X;
    this.newY = this.Y;
    this.newWidth = this.Width;
    this.newHeight = this.Height;
    this.newTitle = this.Title;
    this.newBody = this.Body;
    this.newLocked = locked;
    this.Editing = false;

    // SelfInsert
    if (this.IDCard > 0) {
        this.cfg.Cards.push(this);
    }
    this.InsertOnContainer(this.cfg.divBoard);

    this.SetLock(this.Locked);
};
Card.prototype = {
    FilterText: function (text) {
        text = text.split("  ").join(" &nbsp;");
        text = text.split("&nbsp; &nbsp;").join("&nbsp;&nbsp;&nbsp;");
        text = text.split("\n").join("<br />");
        return text;
    },
    CosineInterpolation: function (f) {
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        return f;
    },
    InsertOnContainer: function (container) {
        this.container = container;
        this.container.appendChild(this.divCard);
        this.cfg.Toolbox.ReInsertOnContainer();
    },
    RemoveFromContainer: function () {
        this.container.removeChild(this.divCard);
    },
    MoveFrame: function () {
        if (this.animData) {
            var f = (+new Date() - this.animData.startTime) / this.animData.time;
            if (f < 1.0) {
                f = this.CosineInterpolation(f);
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
    ResizeFrame: function () {
        if (this.animData) {
            var f = (+new Date() - this.animData.startTime) / this.animData.time;
            if (f < 1.0) {
                f = this.CosineInterpolation(f);
                var f2 = 1 - f;
                var width = this.animData.Width1 * f + this.animData.Width0 * f2;
                var height = this.animData.Height1 * f + this.animData.Height0 * f2;
                this.divCard.style.width = width + "px";
                this.divCard.style.height = height + "px";
                this.animData.animationID = window.setTimeout(this.bindedResizeFrame, 16);
                return;
            }
        }
        this.divCard.style.width = this.Width + "px";
        this.divCard.style.height = this.Height + "px";
        this.animData = null;
    },
    Move: function (x, y) {
        if (x < 0) { x = 0; }
        if (y < 0) { y = 0; }
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
    Resize: function (width, height) {
        if (width < 100) { x = 100; }
        if (height < 100) { y = 100; }
        this.OnResizeStart();
        this.Width = width;
        this.Height = height;
        this.newWidth = width;
        this.newHeight = height;
        this.animData = {
            Width0: parseInt(this.divCard.style.width),
            Height0: parseInt(this.divCard.style.height),
            Width1: width,
            Height1: height,
            time: this.cfg.TimeMoveAnimation,
            startTime: +new Date(),
            animationID: 0
        };
        this.bindedResizeFrame = Card.prototype.ResizeFrame.bind(this);
        this.animData.animationID = window.setTimeout(this.bindedResizeFrame, 16);
    },
    Edit: function (title, body) {
        if (this.Editing) {
            this.ExitEditionMode();
        }
        this.Title = title;
        this.Body = body;
        this.newTitle = title;
        this.newBody = body;
        this.txtTitle.value = this.Title;
        this.txtBody.value = this.Body;
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);
    },
    Lock: function (locked) {
        this.newLocked = locked
        this.SetLock(locked);
    },
    SetLock: function (locked) {
        if (locked) {
            this.btnEdit.style.display = "none";
            this.btnDelete.style.display = "none";
            this.btnLock.style.display = "none";
            this.btnUnlock.style.display = "";
            this.divOverlay.removeEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
            this.divOverlay.removeEventListener("touchstart", this.divOverlay_TouchStartBinded, false);
            this.divResize.style.display = "none";
        } else {
            this.btnEdit.style.display = "";
            this.btnDelete.style.display = "";
            this.btnLock.style.display = "";
            this.btnUnlock.style.display = "none";
            this.divOverlay.addEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
            this.divOverlay.addEventListener("touchstart", this.divOverlay_TouchStartBinded, false);
            this.divResize.style.display = "";
        }
    },
    Reset: function () {
        this.newX = this.X;
        this.newY = this.Y;
        this.newWidth = this.Width;
        this.newHeight = this.Height;
        this.newTitle = this.Title;
        this.newBody = this.Body;
        this.newLocked = this.Locked;

        this.divCard.style.left = this.X + "px";
        this.divCard.style.top = this.Y + "px";
        this.txtTitle.value = this.Title;
        this.SetLock(this.Locked);
        this.txtBody.value = this.Body;
    },
    SetNew: function () {
        this.X = this.newX;
        this.Y = this.newY;
        this.Width = this.newWidth;
        this.Height = this.newHeight;
        this.Title = this.newTitle;
        this.Body = this.newBody;
        this.Locked = this.newLocked;
        this.Reset();
    },
    Hide: function () {
        this.divCard.style.display = "none";
    },
    Show: function () {
        this.divCard.style.display = "";
    },
    SendEvent: function (eventData) {
        var card = this;
        SendData(this.cfg.ServiceUrl, eventData,
            function (responseText) {
                try {
                    var recvData = JSON.parse(responseText);
                    if (recvData && recvData instanceof Object && recvData.IsOK === true) {
                        card.SetNew();
                    } else {
                        card.Reset();
                    }
                } catch (e) { /* Empty */ }
            }, function () {
                card.Reset();
            });
    },
    OnCreate: function () {
        if (this.cfg.Connected === false) {
            this.OnDelete();
            return;
        }
        this.RemoveFromContainer();
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "CardCreate",
            "X": this.X,
            "Y": this.Y,
            "Width": this.Width,
            "Height": this.Height,
            "Title": this.Title,
            "Body": this.Body,
            "Locked": this.Locked ? 1 : 0,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
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
        if (this.cfg.Connected === false) {
            this.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "CardMove",
            "IDCard": this.IDCard,
            "X": this.newX,
            "Y": this.newY,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnResizeStart: function () {
        if (this.animData) {
            window.clearTimeout(this.animData.animationID);
            this.animData = null;
        }
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);
    },
    OnResize: function () {
        if (this.cfg.Connected === false) {
            this.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "CardResize",
            "IDCard": this.IDCard,
            "Width": this.newWidth,
            "Height": this.newHeight,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnEdit: function () {
        if (this.Title !== this.newTitle || this.Body !== this.newBody) {
            if (this.cfg.Connected === false) {
                this.Reset();
                return;
            }
            var data = {
                "IDBoard": this.cfg.IDBoard,
                "Command": "CardEdit",
                "IDCard": this.IDCard,
                "Title": this.newTitle,
                "Body": this.newBody,
                "TimeStamp": new Date().getTime()
            };
            this.SendEvent(data);
        }
    },
    OnLocked: function () {
        if (this.cfg.Connected === false) {
            this.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "CardLock",
            "IDCard": this.IDCard,
            "Locked": this.newLocked ? 1 : 0,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnDelete: function () {
        this.Hide();
        if (this.cfg.Connected === false) {
            this.Show();
            return;
        }
        if (this.IDCard === 0) {
            this.RemoveFromContainer();
            this.cfg.RemoveCardByID(this.IDCard);
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "CardDelete",
            "IDCard": this.IDCard,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
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
    divOverlay_MouseDown: function (evt) {
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("mouseup", this.divOverlay_MouseUpBinded, false);
        document.addEventListener("mousemove", this.divOverlay_MouseMoveBinded, false);

        return false;
    },
    divOverlay_MouseMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        if (this.newX < 0) { this.newX = 0; }
        if (this.newY < 0) { this.newY = 0; }
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        return false;
    },
    divOverlay_MouseUp: function (evt) {
        evt.preventDefault();

        document.removeEventListener("mouseup", this.divOverlay_MouseUpBinded, false);
        document.removeEventListener("mousemove", this.divOverlay_MouseMoveBinded, false);

        this.OnMove();

        return false;
    },
    divOverlay_TouchStart: function (evt) {
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.offsetX = pos.x - this.divCard.offsetLeft;
        this.offsetY = pos.y - this.divCard.offsetTop;

        document.addEventListener("touchend", this.divOverlay_TouchEndBinded, false);
        document.addEventListener("touchcancel", this.divOverlay_TouchEndBinded, false);
        document.addEventListener("touchmove", this.divOverlay_TouchMoveBinded, false);

        return false;
    },
    divOverlay_TouchMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        if (this.newX < 0) { this.newX = 0; }
        if (this.newY < 0) { this.newY = 0; }
        this.divCard.style.left = this.newX + "px";
        this.divCard.style.top = this.newY + "px";

        return false;
    },
    divOverlay_TouchEnd: function (evt) {
        evt.preventDefault();

        document.removeEventListener("touchend", this.divOverlay_TouchEndBinded, false);
        document.removeEventListener("touchcancel", this.divOverlay_TouchEndBinded, false);
        document.removeEventListener("touchmove", this.divOverlay_TouchMoveBinded, false);

        this.OnMove();

        return false;
    },
    divResize_MouseDown: function (evt) {
        evt.preventDefault();

        this.OnResizeStart();

        this.offsetX = evt.clientX;
        this.offsetY = evt.clientY;

        document.addEventListener("mouseup", this.divResize_MouseUpBinded, false);
        document.addEventListener("mousemove", this.divResize_MouseMoveBinded, false);

        return false;
    },
    divResize_MouseMove: function (evt) {
        evt.preventDefault();

        this.newWidth = this.Width + (evt.clientX - this.offsetX);
        this.newHeight = this.Height + (evt.clientY - this.offsetY);
        if (this.newWidth < 100) { this.newWidth = 100; }
        if (this.newHeight < 100) { this.newHeight = 100; }
        this.divCard.style.width = this.newWidth + "px";
        this.divCard.style.height = this.newHeight + "px";

        return false;
    },
    divResize_MouseUp: function (evt) {
        evt.preventDefault();

        document.removeEventListener("mouseup", this.divResize_MouseUpBinded, false);
        document.removeEventListener("mousemove", this.divResize_MouseMoveBinded, false);

        this.OnResize();

        return false;
    },
    divResize_TouchStart: function (evt) {
        evt.preventDefault();

        this.OnResizeStart();

        this.offsetX = evt.touches[0].clientX;
        this.offsetY = evt.touches[0].clientY;

        document.addEventListener("touchend", this.divResize_TouchEndBinded, false);
        document.addEventListener("touchcancel", this.divResize_TouchEndBinded, false);
        document.addEventListener("touchmove", this.divResize_TouchMoveBinded, false);

        return false;
    },
    divResize_TouchMove: function (evt) {
        evt.preventDefault();

        this.newWidth = this.Width + (evt.touches[0].clientX - this.offsetX);
        this.newHeight = this.Height + (evt.touches[0].clientY - this.offsetY);
        if (this.newWidth < 100) { this.newWidth = 100; }
        if (this.newHeight < 100) { this.newHeight = 100; }
        this.divCard.style.width = this.newWidth + "px";
        this.divCard.style.height = this.newHeight + "px";

        return false;
    },
    divResize_TouchEnd: function (evt) {
        evt.preventDefault();

        document.removeEventListener("touchend", this.divResize_TouchEndBinded, false);
        document.removeEventListener("touchcancel", this.divResize_TouchEndBinded, false);
        document.removeEventListener("touchmove", this.divResize_TouchMoveBinded, false);

        this.OnResize();

        return false;
    },
    EnterEditionMode: function () {
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);

        this.txtTitle.value = this.Title;
        this.txtBody.value = this.Body;

        this.divOverlay.style.display = "none";
        this.divResize.style.display = "none";
        this.Editing = true;

        this.divEditBackground = document.createElement("div");
        this.divEditBackground.className = "divEditBackground";
        this.divEditBackground.addEventListener("click", Card.prototype.btnEdit_Click.bind(this), false);
        this.divCard.parentElement.insertBefore(this.divEditBackground, this.divCard);

    },
    ExitEditionMode: function () {
        this.divOverlay.style.display = "";
        this.divResize.style.display = "";
        this.Editing = false;
        this.divEditBackground.className = ""; // Needed to remove "position: fixed" that causes to be not found on parentElement.
        this.divEditBackground.parentElement.removeChild(this.divEditBackground);
    },
    btnEdit_Click: function (evt) {
        evt.preventDefault();
        if (this.Editing === false) {
            this.EnterEditionMode();
        } else {
            this.newTitle = this.txtTitle.value;
            this.newBody = this.txtBody.value;
            this.ExitEditionMode();
            this.txtTitle.value = this.newTitle;
            this.txtBody.value = this.newBody;
            if (this.IDCard > 0) {
                this.OnEdit();
            } else {
                this.Title = this.newTitle;
                this.Body = this.newBody;
                this.OnCreate();
            }
        }
        return false;
    },
    btnDelete_Click: function (evt) {
        evt.preventDefault();
        if (this.Editing === true) {
            this.ExitEditionMode();
            this.Reset();
            if (this.IDCard > 0) {
                return false;
            }
        }
        if (this.IDCard === 0 || confirm(this.cfg.Texts.ConfirmDelete)) {
            this.OnDelete();
        }
        return false;
    },
    btnLock_Click: function (evt) {
        evt.preventDefault();
        this.Lock(true);
        this.OnLocked();
        return false;
    },
    btnUnlock_Click: function (evt) {
        evt.preventDefault();
        this.Lock(false);
        this.OnLocked();
        return false;
    },
    empty: null
};

var Region = function (cfg, idRegion, title, x, y, width, height, locked) {
    this.cfg = cfg;
    this.IDRegion = idRegion;
    this.Title = title;
    this.X = x;
    this.Y = y;
    this.Width = width;
    this.Height = height;
    this.Locked = locked;

    // Create DOM
    this.container = null;
    this.divRegion = document.createElement("div");
    this.divRegion.className = "divRegion";
    this.divRegion.style.left = x + "px";
    this.divRegion.style.top = y + "px";
    this.divRegion.style.width = width + "px";
    this.divRegion.style.height = height + "px";

    this.divTitle = document.createElement("div");
    this.divRegion.appendChild(this.divTitle);
    this.divTitle.className = "divTitle";

    this.txtTitle = document.createElement("input");
    this.txtTitle.className = "txtTitle";
    this.txtTitle.value = this.Title;
    this.divTitle.appendChild(this.txtTitle);

    this.divOverlay = document.createElement("div");
    this.divRegion.appendChild(this.divOverlay);
    this.divOverlay.className = "divOverlay";
    this.divOverlay_MouseDownBinded = Region.prototype.divOverlay_MouseDown.bind(this);
    this.divOverlay_MouseMoveBinded = Region.prototype.divOverlay_MouseMove.bind(this);
    this.divOverlay_MouseUpBinded = Region.prototype.divOverlay_MouseUp.bind(this);
    this.divOverlay.addEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
    this.divOverlay_TouchStartBinded = Region.prototype.divOverlay_TouchStart.bind(this);
    this.divOverlay_TouchMoveBinded = Region.prototype.divOverlay_TouchMove.bind(this);
    this.divOverlay_TouchEndBinded = Region.prototype.divOverlay_TouchEnd.bind(this);
    this.divOverlay.addEventListener("touchstart", this.divOverlay_TouchStartBinded, false);

    this.btnEdit = document.createElement("button");
    this.divRegion.appendChild(this.btnEdit);
    this.btnEdit.className = "btnRegion btnEdit";
    this.btnEdit.innerHTML = "&#x270E;"; // Unicode "Lower Right Pencil"
    this.btnEdit.addEventListener("click", Region.prototype.btnEdit_Click.bind(this), false);

    this.btnDelete = document.createElement("button");
    this.divRegion.appendChild(this.btnDelete);
    this.btnDelete.className = "btnRegion btnDelete";
    this.btnDelete.innerHTML = "X"; 
    this.btnDelete.addEventListener("click", Region.prototype.btnDelete_Click.bind(this), false);
    
    this.btnLock = document.createElement("button");
    this.divRegion.appendChild(this.btnLock);
    this.btnLock.className = "btnRegion btnLock";
    this.btnLock.innerHTML = "&#x1F512;"; // Unicode "Lock"
    this.btnLock.addEventListener("click", Region.prototype.btnLock_Click.bind(this), false);

    this.btnUnlock = document.createElement("button");
    this.divRegion.appendChild(this.btnUnlock);
    this.btnUnlock.className = "btnRegion btnUnlock";
    this.btnUnlock.innerHTML = "&#x1F513;"; // Unicode "Open Lock"
    this.btnUnlock.style.display = "none";
    this.btnUnlock.addEventListener("click", Region.prototype.btnUnlock_Click.bind(this), false);

    this.divResize = document.createElement("div");
    this.divRegion.appendChild(this.divResize);
    this.divResize.className = "divResize";
    this.divResize_MouseDownBinded = Region.prototype.divResize_MouseDown.bind(this);
    this.divResize_MouseMoveBinded = Region.prototype.divResize_MouseMove.bind(this);
    this.divResize_MouseUpBinded = Region.prototype.divResize_MouseUp.bind(this);
    this.divResize.addEventListener("mousedown", this.divResize_MouseDownBinded, false);
    this.divResize_TouchStartBinded = Region.prototype.divResize_TouchStart.bind(this);
    this.divResize_TouchMoveBinded = Region.prototype.divResize_TouchMove.bind(this);
    this.divResize_TouchEndBinded = Region.prototype.divResize_TouchEnd.bind(this);
    this.divResize.addEventListener("touchstart", this.divResize_TouchStartBinded, false);

    // Temporal variables for actions
    this.offsetX = 0;
    this.offsetY = 0;
    this.newX = this.X;
    this.newY = this.Y;
    this.newWidth = this.Width;
    this.newHeight = this.Height;
    this.newTitle = this.Title;
    this.newLocked = this.Locked;
    this.Editing = false;

    // Selfinsert
    if (this.IDRegion > 0) {
        this.cfg.Regions.push(this);
    }
    this.InsertOnContainer(this.cfg.divBoard);

    this.SetLock(this.Locked);
};
Region.prototype = {
    FilterText: function (text) {
        text = text.split("  ").join(" &nbsp;");
        text = text.split("&nbsp; &nbsp;").join("&nbsp;&nbsp;&nbsp;");
        text = text.split("\n").join("<br />");
        return text;
    },
    CosineInterpolation: function (f) {
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        f = 1.0 - (Math.cos(f * Math.PI) + 1.0) / 2.0;
        return f;
    },
    InsertOnContainer: function (container) {
        this.container = container;
        this.container.insertBefore(this.divRegion, this.container.firstChild);
    },
    RemoveFromContainer: function () {
        this.container.removeChild(this.divRegion);
    },
    MoveFrame: function () {
        if (this.animData) {
            var f = (+new Date() - this.animData.startTime) / this.animData.time;
            if (f < 1.0) {
                f = this.CosineInterpolation(f);
                var f2 = 1 - f;
                var x = this.animData.X1 * f + this.animData.X0 * f2;
                var y = this.animData.Y1 * f + this.animData.Y0 * f2;
                this.divRegion.style.left = x + "px";
                this.divRegion.style.top = y + "px";
                this.animData.animationID = window.setTimeout(this.bindedMoveFrame, 16);
                return;
            }
        }
        this.divRegion.style.left = this.X + "px";
        this.divRegion.style.top = this.Y + "px";
        this.animData = null;
    },
    ResizeFrame: function () {
        if (this.animData) {
            var f = (+new Date() - this.animData.startTime) / this.animData.time;
            if (f < 1.0) {
                f = this.CosineInterpolation(f);
                var f2 = 1 - f;
                var width = this.animData.Width1 * f + this.animData.Width0 * f2;
                var height = this.animData.Height1 * f + this.animData.Height0 * f2;
                this.divRegion.style.width = width + "px";
                this.divRegion.style.height = height + "px";
                this.animData.animationID = window.setTimeout(this.bindedResizeFrame, 16);
                return;
            }
        }
        this.divRegion.style.width = this.Width + "px";
        this.divRegion.style.height = this.Height + "px";
        this.animData = null;
    },
    Move: function (x, y) {
        if (x < 0) { x = 0; }
        if (y < 0) { y = 0; }
        this.OnMoveStart();
        this.X = x;
        this.Y = y;
        this.newX = x;
        this.newY = y;
        this.animData = {
            X0: parseInt(this.divRegion.style.left),
            Y0: parseInt(this.divRegion.style.top),
            X1: x,
            Y1: y,
            time: this.cfg.TimeMoveAnimation,
            startTime: +new Date(),
            animationID: 0
        };
        this.bindedMoveFrame = Region.prototype.MoveFrame.bind(this);
        this.animData.animationID = window.setTimeout(this.bindedMoveFrame, 16);
    },
    Resize: function (width, height) {
        if (width < 100) { x = 100; }
        if (height < 100) { y = 100; }
        this.OnResizeStart();
        this.Width = width;
        this.Height = height;
        this.newWidth = width;
        this.newHeight = height;
        this.animData = {
            Width0: parseInt(this.divRegion.style.width),
            Height0: parseInt(this.divRegion.style.height),
            Width1: width,
            Height1: height,
            time: this.cfg.TimeMoveAnimation,
            startTime: +new Date(),
            animationID: 0
        };
        this.bindedResizeFrame = Region.prototype.ResizeFrame.bind(this);
        this.animData.animationID = window.setTimeout(this.bindedResizeFrame, 16);
    },
    Edit: function (title) {
        if (this.Editing) {
            this.ExitEditionMode();
        }
        this.Title = title;
        this.newTitle = title;
        this.txtTitle.value = this.Title;
    },
    Lock: function (locked) {
        this.newLocked = locked
        this.SetLock(locked);
    },
    SetLock: function (locked) {
        if (locked) {
            this.btnEdit.style.display = "none";
            this.btnDelete.style.display = "none";
            this.btnLock.style.display = "none";
            this.btnUnlock.style.display = "";
            this.divOverlay.removeEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
            this.divOverlay.removeEventListener("touchstart", this.divOverlay_TouchStartBinded, false);
            this.divResize.style.display = "none";
        } else {
            this.btnEdit.style.display = "";
            this.btnDelete.style.display = "";
            this.btnLock.style.display = "";
            this.btnUnlock.style.display = "none";
            this.divOverlay.addEventListener("mousedown", this.divOverlay_MouseDownBinded, false);
            this.divOverlay.addEventListener("touchstart", this.divOverlay_TouchStartBinded, false);
            this.divResize.style.display = "";
        }
    },
    Reset: function () {
        this.newX = this.X;
        this.newY = this.Y;
        this.newWidth = this.Width;
        this.newHeight = this.Height;
        this.newLocked = this.Locked;
        this.newTitle = this.Title;

        this.divRegion.style.left = this.X + "px";
        this.divRegion.style.top = this.Y + "px";
        this.divRegion.style.width = this.Width + "px";
        this.divRegion.style.height = this.Height + "px";
        this.SetLock(this.Locked);
        this.txtTitle.value = this.Title;
    },
    SetNew: function () {
        this.X = this.newX;
        this.Y = this.newY;
        this.Width = this.newWidth;
        this.Height = this.newHeight;
        this.Locked = this.newLocked;
        this.Title = this.newTitle;
        this.Reset();
    },
    Hide: function () {
        this.divRegion.style.display = "none";
    },
    Show: function () {
        this.divRegion.style.display = "";
    },
    SendEvent: function (eventData) {
        var region = this;
        SendData(this.cfg.ServiceUrl, eventData,
            function (responseText) {
                try {
                    var recvData = JSON.parse(responseText);
                    if (recvData && recvData instanceof Object && recvData.IsOK === true) {
                        region.SetNew();
                    } else {
                        region.Reset();
                    }
                } catch (e) { /* Empty */ }
            }, function () {
                region.Reset();
            });
    },
    OnCreate: function () {
        if (this.cfg.Connected === false) {
            this.OnDelete();
            return;
        }
        this.RemoveFromContainer();
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "RegionCreate",
            "X": this.X,
            "Y": this.Y,
            "Width": this.Width,
            "Height": this.Height,
            "Locked": this.Locked ? 1 : 0,
            "Title": this.Title,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnMoveStart: function () {
        if (this.animData) {
            window.clearTimeout(this.animData.animationID);
            this.animData = null;
        }
    },
    OnMove: function () {
        var Region = this;
        if (this.cfg.Connected === false) {
            Region.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "RegionMove",
            "IDRegion": this.IDRegion,
            "X": this.newX,
            "Y": this.newY,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnResizeStart: function () {
        if (this.animData) {
            window.clearTimeout(this.animData.animationID);
            this.animData = null;
        }
    },
    OnResize: function () {
        if (this.cfg.Connected === false) {
            this.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "RegionResize",
            "IDRegion": this.IDRegion,
            "Width": this.newWidth,
            "Height": this.newHeight,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnEdit: function () {
        if (this.Title !== this.newTitle) {
            if (this.cfg.Connected === false) {
                this.Reset();
                return;
            }
            var data = {
                "IDBoard": this.cfg.IDBoard,
                "Command": "RegionEdit",
                "IDRegion": this.IDRegion,
                "Title": this.newTitle,
                "TimeStamp": new Date().getTime()
            };
            this.SendEvent(data);
        }
    },
    OnLocked: function () {
        if (this.cfg.Connected === false) {
            this.Reset();
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "RegionLock",
            "IDRegion": this.IDRegion,
            "Locked": this.newLocked ? 1 : 0,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
    },
    OnDelete: function () {
        this.Hide();
        if (this.cfg.Connected === false) {
            this.Show();
            return;
        }
        if (this.IDRegion === 0) {
            this.RemoveFromContainer();
            this.cfg.RemoveRegionByID(this.IDRegion);
            return;
        }
        var data = {
            "IDBoard": this.cfg.IDBoard,
            "Command": "RegionDelete",
            "IDRegion": this.IDRegion,
            "TimeStamp": new Date().getTime()
        };
        this.SendEvent(data);
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
    divOverlay_MouseDown: function (evt) {
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.offsetX = pos.x - this.divRegion.offsetLeft;
        this.offsetY = pos.y - this.divRegion.offsetTop;

        document.addEventListener("mouseup", this.divOverlay_MouseUpBinded, false);
        document.addEventListener("mousemove", this.divOverlay_MouseMoveBinded, false);

        return false;
    },
    divOverlay_MouseMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.clientX, y: evt.clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        if (this.newX < 0) { this.newX = 0; }
        if (this.newY < 0) { this.newY = 0; }
        this.divRegion.style.left = this.newX + "px";
        this.divRegion.style.top = this.newY + "px";

        return false;
    },
    divOverlay_MouseUp: function (evt) {
        evt.preventDefault();

        document.removeEventListener("mouseup", this.divOverlay_MouseUpBinded, false);
        document.removeEventListener("mousemove", this.divOverlay_MouseMoveBinded, false);

        this.OnMove();

        return false;
    },
    divOverlay_TouchStart: function (evt) {
        evt.preventDefault();

        this.OnMoveStart();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.offsetX = pos.x - this.divRegion.offsetLeft;
        this.offsetY = pos.y - this.divRegion.offsetTop;

        document.addEventListener("touchend", this.divOverlay_TouchEndBinded, false);
        document.addEventListener("touchcancel", this.divOverlay_TouchEndBinded, false);
        document.addEventListener("touchmove", this.divOverlay_TouchMoveBinded, false);

        return false;
    },
    divOverlay_TouchMove: function (evt) {
        evt.preventDefault();

        var pos = this.GetRelativePosToContainer({ x: evt.touches[0].clientX, y: evt.touches[0].clientY });
        this.newX = parseInt(pos.x - this.offsetX);
        this.newY = parseInt(pos.y - this.offsetY);
        if (this.newX < 0) { this.newX = 0; }
        if (this.newY < 0) { this.newY = 0; }
        this.divRegion.style.left = this.newX + "px";
        this.divRegion.style.top = this.newY + "px";

        return false;
    },
    divOverlay_TouchEnd: function (evt) {
        evt.preventDefault();

        document.removeEventListener("touchend", this.divOverlay_TouchEndBinded, false);
        document.removeEventListener("touchcancel", this.divOverlay_TouchEndBinded, false);
        document.removeEventListener("touchmove", this.divOverlay_TouchMoveBinded, false);

        this.OnMove();

        return false;
    },
    divResize_MouseDown: function (evt) {
        evt.preventDefault();

        this.OnResizeStart();

        this.offsetX = evt.clientX;
        this.offsetY = evt.clientY;

        document.addEventListener("mouseup", this.divResize_MouseUpBinded, false);
        document.addEventListener("mousemove", this.divResize_MouseMoveBinded, false);

        return false;
    },
    divResize_MouseMove: function (evt) {
        evt.preventDefault();

        this.newWidth = this.Width + (evt.clientX - this.offsetX);
        this.newHeight = this.Height + (evt.clientY - this.offsetY);
        if (this.newWidth < 100) { this.newWidth = 100; }
        if (this.newHeight < 100) { this.newHeight = 100; }
        this.divRegion.style.width = this.newWidth + "px";
        this.divRegion.style.height = this.newHeight + "px";

        return false;
    },
    divResize_MouseUp: function (evt) {
        evt.preventDefault();

        document.removeEventListener("mouseup", this.divResize_MouseUpBinded, false);
        document.removeEventListener("mousemove", this.divResize_MouseMoveBinded, false);

        this.OnResize();

        return false;
    },
    divResize_TouchStart: function (evt) {
        evt.preventDefault();

        this.OnResizeStart();

        this.offsetX = evt.touches[0].clientX;
        this.offsetY = evt.touches[0].clientY;

        document.addEventListener("touchend", this.divResize_TouchEndBinded, false);
        document.addEventListener("touchcancel", this.divResize_TouchEndBinded, false);
        document.addEventListener("touchmove", this.divResize_TouchMoveBinded, false);

        return false;
    },
    divResize_TouchMove: function (evt) {
        evt.preventDefault();

        this.newWidth = this.Width + (evt.touches[0].clientX - this.offsetX);
        this.newHeight = this.Height + (evt.touches[0].clientY - this.offsetY);
        if (this.newWidth < 100) { this.newWidth = 100; }
        if (this.newHeight < 100) { this.newHeight = 100; }
        this.divRegion.style.width = this.newWidth + "px";
        this.divRegion.style.height = this.newHeight + "px";

        return false;
    },
    divResize_TouchEnd: function (evt) {
        evt.preventDefault();

        document.removeEventListener("touchend", this.divResize_TouchEndBinded, false);
        document.removeEventListener("touchcancel", this.divResize_TouchEndBinded, false);
        document.removeEventListener("touchmove", this.divResize_TouchMoveBinded, false);

        this.OnResize();

        return false;
    },
    EnterEditionMode: function () {
        this.RemoveFromContainer();
        this.InsertOnContainer(this.cfg.divBoard);

        this.txtTitle.value = this.Title;

        this.divOverlay.style.display = "none";
        this.divResize.style.display = "none";
        this.Editing = true;

        this.divEditBackground = document.createElement("div");
        this.divEditBackground.className = "divEditBackground";
        this.divEditBackground.addEventListener("click", Region.prototype.btnEdit_Click.bind(this), false);
        this.divRegion.parentElement.insertBefore(this.divEditBackground, this.divRegion);

    },
    ExitEditionMode: function () {
        this.divOverlay.style.display = "";
        this.divResize.style.display = "";
        this.Editing = false;
        this.divEditBackground.className = ""; // Needed to remove "position: fixed" that causes to be not found on parentElement.
        this.divEditBackground.parentElement.removeChild(this.divEditBackground);
    },
    btnEdit_Click: function (evt) {
        evt.preventDefault();
        if (this.Editing === false) {
            this.EnterEditionMode();
        } else {
            this.newTitle = this.txtTitle.value;
            this.ExitEditionMode();
            this.txtTitle.value = this.newTitle;
            if (this.IDRegion > 0) {
                this.OnEdit();
            } else {
                this.Title = this.newTitle;
                this.OnCreate();
            }
        }
        return false;
    },
    btnDelete_Click: function (evt) {
        evt.preventDefault();
        if (this.Editing === true) {
            this.ExitEditionMode();
            this.Reset();
            if (this.IDRegion > 0) {
                return false;
            }
        }
        if (this.IDRegion === 0 || confirm(this.cfg.Texts.ConfirmDelete)) {
            this.OnDelete();
        }
        return false;
    },
    btnLock_Click: function (evt) {
        evt.preventDefault();
        this.Lock(true);
        this.OnLocked();
        return false;
    },
    btnUnlock_Click: function (evt) {
        evt.preventDefault();
        this.Lock(false);
        this.OnLocked();
        return false;
    },
    empty: null
};

function RunCardBoard(cfg) {
    cfg.divBoard = GetElement(cfg.divBoard);

    cfg.Toolbox = new Toolbox(cfg, cfg.divBoard);

    cfg.Connected = false;

    cfg.Cards = [];

    cfg.Regions = [];

    cfg.GetCardByID = function (idCard) {
        for (var i = 0, n = this.Cards.length; i < n; i++) {
            var card = this.Cards[i];
            if (card.IDCard === idCard) {
                return card;
            }
        }
        return null;
    };
    cfg.RemoveCardByID = function (idCard) {
        for (var i = 0, n = this.Cards.length; i < n; i++) {
            var card = this.Cards[i];
            if (card.IDCard === idCard) {
                this.Cards.splice(i, 1);
            }
        }
        return false;
    };

    cfg.GetRegionByID = function (idRegion) {
        for (var i = 0, n = this.Regions.length; i < n; i++) {
            var region = this.Regions[i];
            if (region.IDRegion === idRegion) {
                return region;
            }
        }
        return null;
    };
    cfg.RemoveRegionByID = function (idRegion) {
        for (var i = 0, n = this.Regions.length; i < n; i++) {
            var region = this.Regions[i];
            if (region.IDRegion === idRegion) {
                this.Regions.splice(i, 1);
            }
        }
        return false;
    };

    var ProcessCardCreateEvent = function (cardEvent) {
        var card = new Card(cfg, cardEvent.IDCard, cardEvent.Title, cardEvent.Body, cardEvent.X, cardEvent.Y, cardEvent.Width, cardEvent.Height, cardEvent.Locked);
    };

    var ProcessCardMoveEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card === null) { return; }
        card.Move(cardEvent.X, cardEvent.Y);
    };

    var ProcessCardResizeEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card === null) { return; }
        card.Resize(cardEvent.Width, cardEvent.Height);
    };

    var ProcessCardEditEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card === null) { return; }
        card.Edit(cardEvent.Title, cardEvent.Body);
    };

    var ProcessCardLockEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card === null) { return; }
        card.Lock(cardEvent.Locked);
    };

    var ProcessCardDeleteEvent = function (cardEvent) {
        var card = cfg.GetCardByID(cardEvent.IDCard);
        if (card === null) { return; }
        card.RemoveFromContainer(cfg.divBoard);
    };

    var ProcessRegionCreateEvent = function (cardEvent) {
        var region = new Region(cfg, cardEvent.IDRegion, cardEvent.Title, cardEvent.X, cardEvent.Y, cardEvent.Width, cardEvent.Height, cardEvent.Locked);
    };

    var ProcessRegionMoveEvent = function (cardEvent) {
        var region = cfg.GetRegionByID(cardEvent.IDRegion);
        if (region === null) { return; }
        region.Move(cardEvent.X, cardEvent.Y);
    };

    var ProcessRegionResizeEvent = function (cardEvent) {
        var region = cfg.GetRegionByID(cardEvent.IDRegion);
        if (region === null) { return; }
        region.Resize(cardEvent.Width, cardEvent.Height);
    };

    var ProcessRegionEditEvent = function (cardEvent) {
        var region = cfg.GetRegionByID(cardEvent.IDRegion);
        if (region === null) { return; }
        region.Edit(cardEvent.Title);
    };

    var ProcessRegionLockEvent = function (cardEvent) {
        var region = cfg.GetRegionByID(cardEvent.IDRegion);
        if (region === null) { return; }
        region.Lock(cardEvent.Locked);
    };

    var ProcessRegionDeleteEvent = function (cardEvent) {
        var region = cfg.GetRegionByID(cardEvent.IDRegion);
        if (region === null) { return; }
        region.RemoveFromContainer(cfg.divBoard);
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
                    if (cardEvent.EventType === "CardCreate") {
                        ProcessCardCreateEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "CardMove") {
                        ProcessCardMoveEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "CardResize") {
                        ProcessCardResizeEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "CardEdit") {
                        ProcessCardEditEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "CardLock") {
                        ProcessCardLockEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "CardDelete") {
                        ProcessCardDeleteEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionCreate") {
                        ProcessRegionCreateEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionMove") {
                        ProcessRegionMoveEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionResize") {
                        ProcessRegionResizeEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionEdit") {
                        ProcessRegionEditEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionLock") {
                        ProcessRegionLockEvent(cardEvent);
                    }
                    if (cardEvent.EventType === "RegionDelete") {
                        ProcessRegionDeleteEvent(cardEvent);
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
            "TimePoolData": cfg.Connected === false ? "0" : String(cfg.TimePoolData),
            "TimeStamp": new Date().getTime()
        };
        SendRequest(cfg.ServiceUrl, data, ReciveCardEventData, ErrorCardEventData);
    };
    RequestCardEventData();
}