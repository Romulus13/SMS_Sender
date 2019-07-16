$(document).ready(function () {
    resetAlertDiv();
   
    $('#btnSendSMS').prop('disabled', true);
    $('#btnAddRecipient').prop('disabled', true);
    $("#lblCharNum").val('');


    $.when(getRecipients()).then(allowBtns);
 


});

function resetAlertDiv() {
    $("#alertDiv").hide();
    $("#alertDiv").attr("class", "container alert");
}
function addRecipientTotable(recipient) {

    var recipientRow = jQuery.validator.format('<tr id={0}><td><input type="checkbox" id="chkSendSMS" ></td><td>{1}</td><td>{2}</td></tr>', recipient.Id, recipient.FullName, recipient.CellPhone);
    $('#tblReceiversBody').append(recipientRow);

}

function fetchReceiverData() {
    var recipient = new Object();
    recipient.FullName = $('#txtFullName').val();
    recipient.CellPhone = $('#txtCellPhone').val();
    recipient.SendSMS = null;
    recipient.Id = 0;

    var receiverId = saveReceiver(recipient);

}

function allowBtns() {
    $('#btnSendSMS').prop('disabled', false);
    $('#btnAddRecipient').prop('disabled', false);

}

function getRecipients() {
    return $.ajax({
        url: '/Home/GetRecipients/',
        type: 'GET',
        dataType: 'json',
        success: function (e) {
            if ((typeof (e) !== 'undefined') && (e !== null)) { 
                $.each(e, function (index, value) {
                    addRecipientTotable(value);
                });
            }
            else {
                showInfo("Neuspješan dohvat primatelja!", "DANGER");
            }
        }
    });
}


function saveReceiver(recipient) {
    return $.ajax({
        url: '/Home/AddRecipient/',
        type: 'POST',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(recipient),
        dataType: "json",
        success: function (data) {
            if ((typeof (data) !== 'undefined') && (data !== null)) {
                addRecipientTotable(data);
            }
            else {
                showInfo("Neuspješno spremanje primatelja!", "DANGER");

            }
        },

    });
}
function showInfo(message, status) {
    var classToAdd;
    resetAlertDiv();
    switch (status) {
        case "SUCCESS":
            classToAdd ="alert-success";
            break;
        case "WARNING":
            classToAdd = 'alert-warning';
            break;
        case "DANGER":
            classToAdd = 'alert-danger';
            break;
        default:
            classToAdd = 'alert-info';
    }
    $("#alertDiv").addClass(classToAdd);
    $("#alertMsg").html(message);
    $("#alertDiv").show();
}

function CalculateRemainingChars() {
    var charNum = $("#txtSMS").val().length;
    if (charNum < 160) {
        $("#lblCharNum").html(jQuery.validator.format('{0}/160 znakova!', charNum));
    }
    else {
        $("#lblCharNum").html('Poruka je veća od 160 znakova!').addClass('alert-warning');
    }
    


}

function unCheckCheckboxes() {
    
    $('#tblReceiversBody input:checked').each(function () {
        $(this).prop('checked', false);
    });
}

function getMessages() {
    //firstget table body
    var tbody = $('#tblReceiversBody');
    var messages = [];
    //get all rows, eachrow is a recipient
    var tableRows = tbody[0].getElementsByTagName('tr');
    for (var trIndx = 0; trIndx < tableRows.length; trIndx++) {
        //get all td elements in a row
        var columns = tableRows[trIndx].getElementsByTagName('td');
        ///get checkbox input element
        var sendMsgChk = $(columns[0]).find('input');
        if (sendMsgChk.prop('checked') === true) {
            var message = new Object();
            var recipient = new Object();
            recipient.Id = tableRows[trIndx].id;
            recipient.FullName = columns[1].innerText;
            recipient.CellPhone = columns[2].innerText;
            message.Recipient = recipient;
            message.Content = $('#txtSMS').val().substring(0,161);
            messages.push(message);
        }
        
    }

    return $.ajax({
        url: '/Home/SendMessages/',
        type: 'POST',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(messages),
        dataType: "json",
        success: function (data) {
            unCheckCheckboxes();
            if (data.Status === 200) {
                showInfo(data.Message, "SUCCESS");
            }
            else if (data.Status === 202) {
                showInfo(data.Message, "WARNING");
            }
            else {
                showInfo(data.Message, "DANGER");

            }
        }   
  
    });
}

