(function () {

    var Message = function (options) {

        var _this = this;

        this.start = function () {
            var hub = $.connection.chatHub;
            hub.client.OnMessage = function (data) {
                console.log(JSON.stringify(data));
                var model = {
                    side: (data.FromUser.UserName === 'wesley' ? 'right' : 'left'),
                    message: data.Content
                };
                _this.listMessage(model);
            };
            $.connection.hub.logging = true;
            $.connection.hub.start();

            _this.getMessage();
        };

        this.getMessage = function ()
        {
            $.ajax({
                url: '/Home/GetMessage',
                method: 'GET',
                dataType: 'JSON',
                success: function (data)
                {
                    $.each(data, function (index){
                        var single = data[index];
                        var model = {
                            side: (single.FromUser.UserName === 'wesley' ? 'right' : 'left'),
                            message: single.Content
                        };
                        _this.listMessage(model);
                    });
                }
            });
        };

        this.clearMessage = function () {
            if (confirm("确认要清除聊天记录吗？")) {
                $.ajax({
                    url: '/Home/ClearMessage',
                    method: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        $('.messages').empty();
                    }
                });
            }
        };

        this.sendMessage = function () {
            var content = $('.message_input').val();
            if (content.length > 0)
            {
                $.ajax({
                    url: '/Home/SendMessage',
                    method: 'POST',
                    data: {
                        Content: content
                    }
                });
            }
            $(".message_input").val("").focus();
        };

        this.listMessage = function (data) {

            var templete = $($('.message_template').clone().html());

            templete.addClass(data.side).find('.text').html(data.message);

            $('.messages').append(templete);

            $('.messages').animate({ scrollTop: $('.messages').prop('scrollHeight') }, 300);

            $(".message_input").val("").focus();

            setTimeout(function ()
            {
                templete.addClass('appeared');
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

        $(".close").click(function (e) {
            $(".messages").toggle();
        });

        $(".minimize").click(function () {
            messager.clearMessage();
        });

        $(".chat_window").mouseleave(function () {
            $(".messages").hide();
        });

    });


}.call(this));