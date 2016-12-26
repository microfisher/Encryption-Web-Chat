(function () {

    var Message = function (options) {

        var _this = this;

        var _timeCounter = 0;

        var _enableTimer = 1;

        var _totalSecond = 10;

        this.getCurrentTime = function () {
            $.ajax({
                url: "/Home/GetCurrentTime",
                method: "GET",
                dataType: "JSON",
                success: function (data) {
                    _timeCounter = data;
                }
            });
        };

        this.scroll = function () {
            $(".messages").animate({ scrollTop: $(".messages").prop("scrollHeight") }, 300);
        };

        this.bindMessage=function(data){
            var model = {
                guid:data.Id,
                side: (data.FromUser.UserName === "wesley" ? "right" : "left"),
                message: data.Content,
                sendTime:data.SendTime
            };
            _this.listMessage(model);
        }


        this.removeMessage = function (data) {
            $.ajax({
                url: "/Home/RemoveMessage",
                method: "GET",
                dataType: "JSON",
                data: { guid: data },
                success: function (data) {

                }
            });
        };

        this.start = function () {
            var hub = $.connection.chatHub;
            hub.client.OnMessage = function (data) {
                _this.bindMessage(data);
                _this.scroll();
            };
            $.connection.hub.logging = true;
            $.connection.hub.start();

            _this.getMessage();
            _this.getCurrentTime();

            var timer = setInterval(function () {
                _timeCounter += 1;

                $(".messages").children(".message").each(function () {
                    var guid = $(this).attr("guid");
                    var time = $(this).attr("sendTime");
                    var diff = Number(_timeCounter) - Number(time);
                    if (guid != undefined && time != undefined && time>0) {
                        if (_enableTimer == 1) {
                            if (diff < _totalSecond) {
                                $(this).children(".avatar").html((_totalSecond - diff));
                            } else {
                                _this.removeMessage(guid);
                                $(this).remove();
                            }
                        } else {
                            var pauseDiff = $(this).attr("pauseDiff");
                            $(this).attr("sendTime", (Number(_timeCounter) - Number(pauseDiff)));
                        }
                    }
                });
            }, 1000);

            $(window).focus(function () {
                _enableTimer = 1;
            });

            $(window).blur(function () {
                _enableTimer = 0;
                $(".messages").children(".message").each(function () {
                    var time = $(this).attr("sendTime");
                    var diff = Number(_timeCounter) - Number(time);
                    $(this).attr("pauseDiff", diff);
                });
            });

            $(".message_input").focus();

        };

        this.getMessage = function ()
        {
            $.ajax({
                url: "/Home/GetMessage",
                method: "GET",
                dataType: "JSON",
                success: function (data)
                {
                    $.each(data, function (index){
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
                    url: "/Home/ClearMessage",
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
            if (content.length > 0)
            {
                $.ajax({
                    url: "/Home/SendMessage",
                    method: "POST",
                    data: {
                        Content: content
                    }
                });
            }
            $(".message_input").val("").focus();
            _this.scroll();
        };

        this.listMessage = function (data) {

            var templete = $($(".message_template").clone().html());

            templete.addClass(data.side).find(".text").html(data.message);

            templete.attr('guid', data.guid);

            templete.attr('sendTime', data.sendTime);


            $(".messages").append(templete);

            setTimeout(function ()
            {
                templete.addClass("appeared");
            }, 0);
        };

    };


    $(function () {

        var messager = new Message();

        messager.start();

        $(".send_message").click(function (e)
        {
            messager.sendMessage();
        });

        $(".message_input").keyup(function (e)
        {
            if (e.which === 13)
            {
                messager.sendMessage();
            }
        });

        //$(".close").click(function (e) {
        //    $(".messages").toggle();
        //});

        //$(".minimize").click(function () {
        //    messager.clearMessage();
        //});

        //$(".chat_window").mouseleave(function () {
        //    //$(".messages").hide();
        //});

    });


}.call(this));