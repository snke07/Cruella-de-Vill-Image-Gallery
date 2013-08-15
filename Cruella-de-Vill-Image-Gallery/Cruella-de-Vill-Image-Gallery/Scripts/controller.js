/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    var updateTimer = null;
    var rootUrl = "http://cruelladevilimagegallery.apphb.com/api/";
	//var rootUrl = "http://localhost:53384/api/";

	var Controller = Class.create({
		init: function () {
			this.persister = persisters.get(rootUrl);
		},
		loadUI: function (selector) {
			if (this.persister.isUserLoggedIn()) {
				this.loadGalleryUI(selector);
			}
			else {
				this.loadLoginFormUI(selector);
			}

			this.attachUIEventHandlers(selector);
		},
		loadLoginFormUI: function (selector) {
			var loginFormHtml = ui.loginForm()
			$(selector).html(loginFormHtml);
		},
		loadGalleryUI: function (selector) {
		    var self = this,
                nickname = this.persister.nickname();

		    $.when (
                self.persister.album.all()
            ).done(function (albumsData, picturesData) {
                var galleryUIHtml =
                    ui.galleryUI(nickname, albumsData);

                $(selector).html(galleryUIHtml);
            });
		},
		loadAlbum: function (selector, albumId) {
		    var self = this,
                    nickname = this.persister.nickname();
		    $.when(
                self.persister.album.load(albumId)
            ).done(function (albumData) {
                var albumUIHtml =
                    ui.albumUI(nickname, albumData);

                $(selector).html(albumUIHtml);
            });
		},
		attachUIEventHandlers: function (selector) {
		    var wrapper = $(selector);
		    var modal = $('#modalContainer');
			var self = this;

			wrapper.on("click", "#btn-show-login", function () {
				wrapper.find(".button.selected").removeClass("selected");
				$(this).addClass("selected");
				wrapper.find("#login-form").show();
				wrapper.find("#register-form").hide();
				wrapper.find("#error-messages").hide();
			});

			wrapper.on("click", "#btn-show-register", function () {
				wrapper.find(".button.selected").removeClass("selected");
				$(this).addClass("selected");
				wrapper.find("#register-form").show();
				wrapper.find("#login-form").hide();
				wrapper.find("#error-messages").hide();
			});

			wrapper.on("click", "#btn-login", function () {
				var user = {
					email: $(selector + " #tb-login-username").val(),
					password: $(selector + " #tb-login-password").val()
				}

				wrapper.find("#error-messages").hide();
				self.persister.user.login(user, function () {
					self.loadGalleryUI(selector);
				}, function (err) {
				    wrapper.find("#error-messages").text(JSON.parse(err.responseText).Message).show(300);
				});
				return false;
			});

			wrapper.on("click", "#btn-register", function () {
				var user = {
					email: $(selector).find("#tb-register-username").val(),
					nickname: $(selector).find("#tb-register-nickname").val(),
					password: $(selector + " #tb-register-password").val()
				}
				self.persister.user.register(user, function () {
					self.loadGalleryUI(selector);
				}, function (err) {
				    wrapper.find("#error-messages").text(JSON.parse(err.responseText).Message).show(300);
				});
				return false;
			});

			wrapper.on("click", "#btn-logout", function () {
				self.persister.user.logout(function () {
					self.loadLoginFormUI(selector);
					clearInterval(updateTimer);
				}, function (err) {
				});
			});

			modal.on("click", "#addGalerySubmit", function () {
			    var name = $('#galleryName');
			    var alert = $('#addGalleryAlert').hide();
			    var msg = $('#addGalleryErrorMessage');
			    var btn = $('#addGalerySubmit');

			    var newGalleryData = {
			        title: name.val(),
			        parentId: null
			    };

			    self.persister.album.add(newGalleryData)
                .done(function (data) {
                    alert.removeClass('alert-error');
                    alert.addClass('alert-success');
                    msg.text('Gallery added!');
                    alert.fadeIn(300);
                    btn.hide();
                }).error(function (err) {
                    msg.text(JSON.parse(err.responseText).Message);
                    alert.addClass('alert-error');
                    alert.fadeIn(300);
                    name.focus();
                });
			});

			modal.on("click", "#addAlbumSubmit", function () {
			    var add = $('#btn-add-album');
			    var name = $('#albumName');
			    var alert = $('#addAlbumAlert').hide();
			    var msg = $('#addAlbumErrorMessage');
			    var btn = $('#addAlbumSubmit');
			    var parentId = add.data('albumid');


			    var newAlbumData = {
			        title: name.val(),
			        parentId: parentId
			    };

			    self.persister.album.add(newAlbumData)
                .done(function (data) {
                    alert.removeClass('alert-error');
                    alert.addClass('alert-success');
                    msg.text('Allbum added!');
                    alert.fadeIn(300);
                    btn.hide();
                }).error(function (err) {
                    msg.text(JSON.parse(err.responseText).Message);
                    alert.addClass('alert-error');
                    alert.fadeIn(300);
                    name.focus();
                });
			});

			wrapper.on("click", ".albumItem", function (ev) {
			    var albumid = $(this).data('albumid');
			    self.loadAlbum(selector, albumid);
			    return false;
			});

			wrapper.on("click", "#btn-album-back", function (ev) {
			    var parent = $(this).data('parentid');

			    if (parent) {
			        self.loadAlbum(selector, parent);
			    } else {
			        self.loadGalleryUI(selector);
			    }

			    return false;
			});

			$('#addAlbumForm').on('hidden', function () {
			    var add = $('#btn-add-album');

			    if (add) {
			        var id = add.data('albumid');
			        self.loadAlbum(selector, id);
			    }
			})

			$('#addGalleryForm').on('hidden', function () {
			    self.loadGalleryUI(selector);
			})

		},
		updateUI: function (selector) {
			this.persister.game.open(function (games) {
				var list = ui.openGamesList(games);
				$(selector + " #open-games")
					.html(list);
			});
			this.persister.game.myActive(function (games) {
				var list = ui.activeGamesList(games);
				$(selector + " #active-games")
					.html(list);
			});
			this.persister.message.all(function (msg) {
				var msgList = ui.messagesList(msg);
				$(selector + " #messages-holder").html(msgList);
			});
		}
	});
	return {
		get: function () {
			return new Controller();
		}
	}
}());

$(function () {
	var controller = controllers.get();
	controller.loadUI("#content");
});