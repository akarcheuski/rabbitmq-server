$(function () {
    $('#btnStart').unbind('click');
    $('#btnStart').on('click', function () {
        $('#quant').html('');
        $('#sendingTime').html('');
        $('#consumingTime').html('');
        $('#messagesQuantity1').html('');
        $('#sendingTime1').html('');
        $('#consumingTime1').html('');
        if ($("#amountOfTime").val().length > 0 || $("#messagesQuantity").val().length > 0) {
            var result = ($("#amountOfTime").val().length > 0) ? $("#amountOfTime").val() : $("#messagesQuantity").val();
            var isTime = ($("#amountOfTime").val().length > 0) ? true : false;
            $('#error').empty();
            var clientName1 = $("#lstClient1 option:selected").text();
            var clientName2 = $("#lstClient2 option:selected").text();
            $('#error').append("Executing...").css('color', 'green');
            $.ajax({
                url: "/default/sendmessages",
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify({ client1: clientName1, client2: clientName2, filter: result, isTime: isTime }),
                async: true,
                success: function (data) {
                    $('#error').empty();
                    var quantity = $("<span>").html(data.quantity)
                        .css('color', 'green');
                    var time = $("<br /><span>")
                        .html(data.time).css('color', 'green');
                    var consumedTime = $("<br /><span>")
                   .html(data.consumedTime).css('color', 'green');
                    $('#quant').append("Messages quantity: ");
                    $('#sendingTime').append("Sending time: ");
                    $('#consumingTime').append("Consuming time: ");
                    $('#messagesQuantity1').append(quantity);
                    $('#sendingTime1').append(time);
                    $('#consumingTime1').append(consumedTime);
                },
            });
        }
        else
        {
            $('#error').append("Enter the value").css('color', 'red');
        }
    });
});

$(document).ready(function () {  
    $(":text").keyup(function (e) {
        this.value = this.value.replace(/[^0-9\.]/g, '');
        if ($(this).val() != '') {
            $(":text").not(this).attr('disabled', 'disabled');
        } else {
            $(":text").removeAttr('disabled');
        }
    });
});