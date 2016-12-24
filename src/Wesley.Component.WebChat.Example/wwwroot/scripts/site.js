(function () {
    var Message;
    Message = function (arg) {
        this.text = arg.text, this.message_side = arg.message_side;
        this.draw = function (_this) {
            return function () {
                var $message;
                $message = $($('.message_template').clone().html());
                $message.addClass(_this.message_side).find('.text').html(_this.text);
                $('.messages').append($message);
                return setTimeout(function () {
                    return $message.addClass('appeared');
                }, 0);
            };
        }(this);
        return this;
    };
    $(function () {
        var getMessageText, message_side, sendMessage;
        message_side = 'right';
        sendMessage = function (data) {
            var $messages, message;
            console.log(JSON.stringify(data));
            $('.message_input').val('').focus();
            if (!data.Text) {
                return;
            }
            $messages = $('.messages');
            message_side = data.UserName === 'wesley' ? 'right' : 'left';
            message = new Message({
                text: data.Text,
                message_side: message_side
            });
            message.draw();
            return $messages.animate({ scrollTop: $messages.prop('scrollHeight') }, 300);
        };

        $('.message_input').keyup(function (e) {
            if (e.which === 13) {
                var message = $('.message_input').val();
                if (message.length > 0) {
                    $.ajax({
                        url: '/Home/AddPost',
                        method: 'POST',
                        data: {
                            text: message
                        }
                    });
                }
            }
        });
        $('.close').click(function (e) {
            $(".messages").toggle();
        });

        $(".chat_window").mouseleave(function () {
            $(".messages").hide();
        });


        getMessage = function () {
            $.ajax({
                url: '/Home/GetPosts',
                method: 'GET',
                dataType: 'JSON',
                success: function (posts) {
                    $.each(posts, function (index) {
                        var post = posts[index];
                        sendMessage(post);
                    });
                }
            });
        }

        getMessage();

        $('.send_message').click(function (e) {
            var message = $('.message_input').val();
            if (message.length > 0) {
                $.ajax({
                    url: '/Home/AddPost',
                    method: 'POST',
                    data: {
                        text: message
                    }
                });
            }

        });


        var hub = $.connection.chatHub;
        hub.client.publishPost = sendMessage;
        $.connection.hub.logging = true;
        $.connection.hub.start();









    });
}.call(this));