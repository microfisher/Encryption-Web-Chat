var Messager = function (options) {
    var _this = this;
    var _timer = null;
    var _countDown = 3;
    var _enableTimer = 0;

    this.scroll = function () {
        $(".messages").animate({ scrollTop: $(".messages").prop("scrollHeight") }, 300);
    };

    this.bindMessage = function (data) {
        var model = {
            guid: data.Id,
            side: (data.FromUser.UserName.toLowerCase() === "wesley" ? "right" : "left"),
            message: data.MessageContent,
            createdTime: data.CreatedTime,
            readStatus: data.ReadStatus,
            sequence: data.Sequence
        };
        _this.listMessage(model);
    };


    this.count = function () {
        $(".messages").children(".message").each(function () {
            var id = $(this).attr("id");
            var countDown = parseInt($(this).attr("countDown"));
            var readStatus = parseInt($(this).attr("readStatus"));
            if (countDown > 0) {
                $(this).attr("countDown", (countDown - 1))
                $(this).children(".avatar").html((countDown - 1));
            } else if (readStatus == 0) {
                $.ajax({
                    url: "/Messager/UpdateStatus",
                    method: "GET",
                    dataType: "JSON",
                    data: { guid: id },
                    success: function (data) {

                    }
                });
                $(this).attr("readStatus", 1);
                $(this).children(".text_wrapper").children().hide();
                if ($(this).hasClass("left")) {
                    $(this).children(".avatar").html("^_^");
                } else {
                    $(this).children(".avatar").html("*_*");
                }
                $(this).children(".text_wrapper").on("mouseover", function () {
                    $(this).parent().children(".text_wrapper").children().show();
                }).on("mouseout", function () {
                    $(this).parent().children(".text_wrapper").children().hide();
                });
            }
        });
    };


    this.start = function () {
        var hub = $.connection.chatHub;
        hub.client.OnMessage = function (data) {

            _this.bindMessage(data);
            _this.scroll();
        };

        $.connection.hub.logging = false;
        $.connection.hub.start();

        _this.getMessage();
        _timer = setInterval(function () {
            _this.count();
        }, 1000);

        $(".message_input").focus();

    };

    this.getMessage = function () {
        $.ajax({
            url: "/Messager/GetMessageList",
            method: "GET",
            dataType: "JSON",
            success: function (data) {
                $.each(data, function (index) {
                    _this.bindMessage(data[index]);
                });
            },
            complete: function () {
                _this.scroll();
            }
        });
    };

    this.clearMessage = function () {
        if (confirm("确认要清除聊天记录吗？")) {
            $.ajax({
                url: "/Messager/ClearMessage",
                method: "GET",
                dataType: "JSON",
                success: function (data) {
                    $('.messages').empty();
                }
            });
        }
    };

    this.sendMessage = function () {
        var content = $(".message_input").val();
        if (content.length > 0) {
            $.ajax({
                url: "/Messager/SendMessage",
                method: "POST",
                data: {
                    content: content
                }
            });
        }
        $(".message_input").val("").focus();
        _this.scroll();
    };

    this.listMessage = function (data) {

        var templete = $($(".message_template").clone().html());
        templete.addClass(data.side).find(".text").html(data.message);
        templete.attr("id", data.guid);
        if (data.readStatus == 0) {
            templete.attr("countDown", _countDown);
        } else {
            templete.attr("countDown", 0);
            if (data.side == "left") {
                templete.children(".avatar").html("^_^");
            } else {
                templete.children(".avatar").html("*_*");
            }

            templete.children(".text_wrapper").children().hide();
            templete.children(".text_wrapper").on("mouseover", function () {
                $(this).parent().children(".text_wrapper").children().show();
            }).on("mouseout", function () {
                $(this).parent().children(".text_wrapper").children().hide();
            });
        }
        templete.attr("readStatus", data.readStatus);

        $(".messages").append(templete);

        setTimeout(function () {
            templete.addClass("appeared");
        }, 0);
    };

};