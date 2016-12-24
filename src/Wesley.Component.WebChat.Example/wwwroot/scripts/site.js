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

        this.sendMessage = function () {
            var content = $('.message_input').val();
            if (content.length > 0)
            {
                $.ajax({
                    url: '/Home/SendMessage',
                    method: 'POST',
                    data: {
                        Content: content
                    },
                    success: function () {
                        $('.messages').animate({ scrollTop: $('.messages').prop('scrollHeight') }, 300);
                    },
                    complete: function () {
                        $(".message_input").val("");
                    }
                });
            }
        };

        this.listMessage = function (data) {

            console.log(JSON.stringify(data));

            var templete = $($('.message_template').clone().html());

            templete.addClass(data.side).find('.text').html(data.message);

            $('.messages').append(templete);

            console.log(templete);

            setTimeout(function ()
            {
                templete.addClass('appeared');
            }, 0);
        };

    };


    $(function () {

        var message = new Message();

        message.start();

        $(".send_message").click(function (e) {
            message.sendMessage();
        });

        $(".message_input").keyup(function (e) {
            if (e.which === 13) {
                message.sendMessage();
            }
        });

        $(".close").click(function (e) {
            $(".messages").toggle();
        });

        $(".chat_window").mouseleave(function () {
            $(".messages").hide();
        });

    });


}.call(this));