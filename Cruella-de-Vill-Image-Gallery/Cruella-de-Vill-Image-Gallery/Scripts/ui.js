/// <reference path="bootstrap.js" />
var ui = (function () {

	function buildLoginForm() {
	    var html =
            '<div id="login-form-holder">' +
				'<form>' +
                    '<div class="row">' +
                        '<div class="span8 offset2 text-center">' +
                            '<a href="#" id="btn-show-login" class="button btn-small selected">Login</a>' +
					        '<a href="#" id="btn-show-register" class="button btn-small">Register</a>' +
	                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<fieldset>' +
					        '<div id="login-form" class="span4 offset2">' +
                                '<legend>Login</legend>' +
						        '<input type="text" id="tb-login-username" placeholder="Email"><br />' +
						        '<input type="password" id="tb-login-password" placeholder="Password"><br />' +
						        '<button id="btn-login" class="button btn">Login</button>' +
					        '</div>' +
					        '<div id="register-form" style="display: none" class="span4 offset6">' +
                                '<legend>Register</legend>' +
						        '<input type="text" id="tb-register-username" placeholder="Email"><br />' +
						        '<input type="text" id="tb-register-nickname"  placeholder="Nickname"><br />' +
						        '<input type="password" id="tb-register-password"  placeholder="Password"><br />' +
						        '<button id="btn-register" class="button btn">Register</button>' +
					        '</div>' +
                        '</fieldset>' +
                    '</div>' +
				'</form>' +
                '<div class="row text-center span6 offset3">' +
				    '<div id="error-messages" class="alert alert-error" style="display: none"></div>' +
                '</div>' +
            '</div>';
		return html;
	}

	function buildGalleryUI(nickname, albumsData) {
	    console.log(albumsData);
        var i = 0,
	        albumItems = '';

        for (i = 0; i < albumsData.length; i++) {
            var item =
                '<div class="albumItem" data-albumid="' + albumsData[i].id + '">' +
                    '<a href="#">' +
                        '<img src="../../Content/Images/album96.png">' +
                        '<div class="text-center">' + albumsData[i].title + '</div>' +
                    '</a>' +
                '</div>';
            albumItems += item;
	    }

        var html =
        '<div class="row">' +
            '<div class="span12">' +
                '<p class="pull-right"><button id="btn-logout" class="btn btn-small">Logout</button></p>' +
                '<h4>Hello, ' + nickname + '!</h4>' +
            '</div>' +
        '</div>' +
        '<div class="row">' +
            '<div class="span12">' +
                '<div class="page-header">' +
                    '<p class="pull-right"><a href="#addGalleryForm" id="btn-add-gallery" class="btn btn-small btn-primary" data-toggle="modal">Add</a></p>' +
                    '<h4>Galeries <small>(' + albumsData.length + ')</small></h4>' +
                '</div>' +
                '<div>' +
                    albumItems +
                '</div>' +
            '</div>' +
        '</div>';

		return html;
	}

	function buildAlbumUI(nickname, albumData) {
	    var i = 0,
	        albumItems = '',
            pictureItems = '',
            albums = albumData.subalbums,
            pictures = albumData.pictures;

	    for (i = 0; i < albums.length; i++) {
	        var item =
                '<div class="albumItem" data-albumid="' + albums[i].id + '" data-parentid="' + albums[i].parentId + '">' +
                    '<a href="#">' +
                        '<img src="../../Content/Images/album96.png">' +
                        '<div class="text-center">' + albums[i].title + '</div>' +
                    '</a>' +
                '</div>';
	        albumItems += item;
	    }

	    for (i = 0; i < pictures.length; i++) {
	        var item =
                '<div class="albumItem" data-pictureid="' + pictures[i].id + '">' +
                    '<a href="#">' +
                        '<img src="' + pictures[i].thumbUrl + '" width="120" height="90" class="img-rounded" >' +
                        '<div class="text-center">' + pictures[i].title + '</div>' +
                    '</a>' +
                '</div>';
	        pictureItems += item;
	    }

	    var html =
        // Albums
        '<div class="row">' +
            '<div class="span12">' +
                '<p class="pull-right"><button id="btn-logout" class="btn btn-small">Logout</button></p>' +
                '<h4>Hello, ' + nickname + '!</h4>' +
            '</div>' +
        '</div>' +
        '<div class="row">' +
            '<div class="span12">' +
                '<div class="page-header">' +
                    '<p class="pull-right">' +
                        '<a href="#addAlbumForm" id="btn-add-album" class="btn btn-small btn-primary" data-toggle="modal" data-albumid="' + albumData.id + '">Add</a>' +
                    '</p>' +
                    '<h4>Albums <small>(' + albums.length + ')</small></h4>' +
                '</div>' +
                '<div>' +
                '<div id="btn-album-back" data-parentid="' + albumData.parentId + '">' +
                    '<a href="#">' +
                        '<img src="../../Content/Images/back96.png">' +
                        '<div class="text-center">Back</div>' +
                    '</a>' +
                '</div>' +
                    albumItems +
                '</div>' +
            '</div>' +
        '</div>' +
        // Pictures
        '<div class="row">' +
            '<div class="span12">' +
                '<div class="page-header">' +
                    '<p class="pull-right">' +
                        '<a href="#addGalleryForm" id="btn-add-album" class="btn btn-small btn-primary" data-toggle="modal">Add</a>' +
                    '</p>' +
                    '<h4>Pictures <small>(' + pictures.length + ')</small></h4>' +
                '</div>' +
                '<div>' +
                    pictureItems +
                '</div>' +
            '</div>' +
        '</div>';

	    return html;
	}

	function buildOpenGamesList(games) {
		var list = '<ul class="game-list open-games">';
		for (var i = 0; i < games.length; i++) {
			var game = games[i];
			list +=
				'<li data-game-id="' + game.id + '">' +
					'<a href="#" >' +
						$("<div />").html(game.title).text() +
					'</a>' +
					'<span> by ' +
						game.creatorNickname +
					'</span>' +
				'</li>';
		}
		list += "</ul>";
		return list;
	}

	function buildActiveGamesList(games) {
		var gamesList = Array.prototype.slice.call(games, 0);
		gamesList.sort(function (g1, g2) {
			if (g1.status == g2.status) {
				return g1.title > g2.title;
			}
			else {
				if (g1.status == "in-progress") {
					return -1;
				}
			}
			return 1;
		});

		var list = '<ul class="game-list active-games">';
		for (var i = 0; i < gamesList.length; i++) {
			var game = gamesList[i];
			list +=
				'<li class="game-status-' + game.status + '" data-game-id="' + game.id + '" data-creator="' + game.creatorNickname + '">' +
					'<a href="#" class="btn-active-game">' +
						$("<div />").html(game.title).text() +
					'</a>' +
					'<span> by ' +
						game.creatorNickname +
					'</span>' +
				'</li>';
		}
		list += "</ul>";
		return list;
	}

	function buildGuessTable(guesses) {
		var tableHtml =
			'<table border="1" cellspacing="0" cellpadding="5">' +
				'<tr>' +
					'<th>Number</th>' +
					'<th>Cows</th>' +
					'<th>Bulls</th>' +
				'</tr>';
		for (var i = 0; i < guesses.length; i++) {
			var guess = guesses[i];
			tableHtml +=
				'<tr>' +
					'<td>' +
						guess.number +
					'</td>' +
					'<td>' +
						guess.cows +
					'</td>' +
					'<td>' +
						guess.bulls +
					'</td>' +
				'</tr>';
		}
		tableHtml += '</table>';
		return tableHtml;
	}

	function buildGameState(gameState) {
		var html =
			'<div id="game-state" data-game-id="' + gameState.id + '">' +
				'<h2>' + gameState.title + '</h2>' +
				'<div id="blue-guesses" class="guess-holder">' +
					'<h3>' +
						gameState.blue + '\'s gueesses' +
					'</h3>' +
					buildGuessTable(gameState.blueGuesses) +
				'</div>' +
				'<div id="red-guesses" class="guess-holder">' +
					'<h3>' +
						gameState.red + '\'s gueesses' +
					'</h3>' +
					buildGuessTable(gameState.redGuesses) +
				'</div>' +
		'</div>';
		return html;
	}
	
	function buildMessagesList(messages) {
		var list = '<ul class="messages-list">';
		var msg;
		for (var i = 0; i < messages.length; i += 1) {
			msg = messages[i];
			var item =
				'<li>' +
					'<a href="#" class="message-state-' + msg.state + '">' +
						msg.text +
					'</a>' +
				'</li>';
			list += item;
		}
		list += '</ul>';
		return list;
	}

	return {
	    galleryUI: buildGalleryUI,
        albumUI: buildAlbumUI,
		openGamesList: buildOpenGamesList,
		loginForm: buildLoginForm,
		activeGamesList: buildActiveGamesList,
		gameState: buildGameState,
		messagesList: buildMessagesList
	}

}());