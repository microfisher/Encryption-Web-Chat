(function ($) { var vis = function () { var stateKey, eventKey, keys = { hidden: "visibilitychange", webkitHidden: "webkitvisibilitychange", mozHidden: "mozvisibilitychange", msHidden: "msvisibilitychange" }; for (stateKey in keys) if (stateKey in document) { eventKey = keys[stateKey]; break } return function (c) { if (c) document.addEventListener(eventKey, c); return !document[stateKey] } }(); $.fn.TabWindowVisibilityManager = function (options) { var defaults = { onFocusCallback: function () { }, onBlurCallback: function () { } }; var o = $.extend(defaults, options); var notIE = document.documentMode === undefined, isChromium = window.chrome; this.each(function () { vis(function () { if (vis()) setTimeout(function () { o.onFocusCallback() }, 300); else o.onBlurCallback() }); if (notIE && !isChromium) $(window).on("focusin", function () { setTimeout(function () { o.onFocusCallback() }, 300) }).on("focusout", function () { o.onBlurCallback() }); else if (window.addEventListener) { window.addEventListener("focus", function (event) { setTimeout(function () { o.onFocusCallback() }, 300) }, false); window.addEventListener("blur", function (event) { o.onBlurCallback() }, false) } else { window.attachEvent("focus", function (event) { setTimeout(function () { o.onFocusCallback() }, 300) }); window.attachEvent("blur", function (event) { o.onBlurCallback() }) } }); return this } })(jQuery);

(function () {

    var Message = function (options) {

        var _this = this;
        var _timer = null;
        var _countDown = 10;
        var _enableTimer = 0;

        this.scroll = function () {
            $(".messages").animate({ scrollTop: $(".messages").prop("scrollHeight") }, 300);
        };

        this.bindMessage=function(data){
            var model = {
                guid: data.Id,
                side: (data.FromUser.UserName === "wesley" ? "right" : "left"),
                message: data.MessageContent,
                createdTime: data.CreatedTime,
                readStatus: data.ReadStatus,
                sequence: data.Sequence
            };
            _this.listMessage(model);
        }


        this.count = function () {
            $(".messages").children(".message").each(function () {
                var id = $(this).attr("id");
                var countDown = parseInt($(this).attr("countDown"));
                var readStatus = parseInt($(this).attr("readStatus"));
                if (countDown > 0)
                {
                    $(this).attr("countDown", (countDown - 1))
                    $(this).children(".avatar").html((countDown - 1));
                } else if (readStatus == 0) {
                    $.ajax({
                        url: "/Home/SetStatus",
                        method: "GET",
                        dataType: "JSON",
                        data: { guid: id },
                        success: function (data)
                        {

                        }
                    });
                    $(this).attr("readStatus", 1);
                    $(this).children(".text_wrapper").children().hide();
                    if ($(this).hasClass("left")) {
                        $(this).children(".avatar").html("^_^");
                    } else {
                        $(this).children(".avatar").html("*_*");
                    }
                    $(this).children(".avatar").on("mouseover", function ()
                    {
                        $(this).parent().children(".text_wrapper").children().show();
                    }).on("mouseout", function ()
                    {
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
                templete.children(".avatar").on("mouseover", function ()
                {
                    $(this).parent().children(".text_wrapper").children().show();
                }).on("mouseout", function ()
                {
                    $(this).parent().children(".text_wrapper").children().hide();
                });
            }
            templete.attr("readStatus", data.readStatus);

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

    });


}.call(this));