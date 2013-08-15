﻿/// <reference path="http-requester.js" />
/// <reference path="class.js" />
/// <reference path="http://crypto-js.googlecode.com/svn/tags/3.1.2/build/rollups/sha1.js" />

var persisters = (function () {

    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");

    function saveUserData(userData) {
        localStorage.setItem("nickname", userData.nickname);
        localStorage.setItem("sessionKey", userData.sessionKey);
        nickname = userData.nickname;
        sessionKey = userData.sessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.album = new AlbumPersister(this.rootUrl);
            this.comment = new CommentPersister(this.rootUrl);
            this.picture = new PicturePersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isLoggedIn = nickname != null && sessionKey != null;
            return isLoggedIn;
        },
        nickname: function () {
            return nickname;
        }
    });

    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users/";
        },
        register: function (user, success, error) {
            var url = this.rootUrl + "register";
            var userData = {
                email: user.email,
                nickname: user.nickname,
                authCode: CryptoJS.SHA1(user.email + user.password).toString()
            };
            httpRequester.postJSON(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },
        login: function (user, success, error) {
            var url = this.rootUrl + "login";
            var userData = {
                email: user.email,
                authCode: CryptoJS.SHA1(user.email + user.password).toString()
            };

            httpRequester.postJSON(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },
        logout: function (success, error) {
            var url = this.rootUrl + "logout/" + sessionKey;
            httpRequester.getJSON(url, function (data) {
                clearUserData();
                success(data);
            }, error)
        }
    });

    var AlbumPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "albums/";
        },
        all: function (success, error) {
            var url = this.rootUrl + "all/" + sessionKey;
            return httpRequester.getJSON(url);
        },
        add: function (albmData, success, error) {
            var url = this.rootUrl + "create/" + sessionKey;
            return httpRequester.postJSON(url, albmData);
        },
        load: function (albumId, success, error) {
            var url = this.rootUrl + albumId + "/load/" + sessionKey;
            return httpRequester.getJSON(url);
        }
    });

    var CommentPersister = Class.create({
        init: function () {

        },
        make: function () {

        }
    });


    var PicturePersister = Class.create({
        init: function () {

        },
        make: function () {

        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());