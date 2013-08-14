/// <reference path="jquery-2.0.3.intellisense.js" />
/// <reference path="class.js" />

var httpRequester = (function () {
    function getJSON(url, success, error) {
        $.ajax({
            url: url,
            type: "GET",
            timeout: 5000,
            contentType: "application/json",
            success: success,
            error: error
        });
    }
    function postJSON(url, data, success, error) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            timeout: 5000,
            data: JSON.stringify(data),
            success: success,
            error: error
        });
    }
    return {
        getJSON: getJSON,
        postJSON: postJSON
    };
}());

//var user = {
//    "email": "gogo@gmail.com",
//    "nickname": "gogo",
//    "authCode": "b44d986c7443bf8656a58318537ebab10f255c06"
//}

//httpRequester.postJSON("http://localhost:53384/api/user/register",
//    user, function () {
//        alert("Success");
//    }, function () {
//        alert("Error");
//    });


