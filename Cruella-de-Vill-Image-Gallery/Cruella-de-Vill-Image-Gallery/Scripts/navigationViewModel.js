function NavigationViewModel(serverUrl) {
	var self = this,
		$loginMail = $('#loginMail'),
		$loginPass = $('#loginPass'),
		$loginBtn = $('#loginBtn'),
		$logoutBtn = $('#logoutBtn'),
		$pageContainer = $('#pagesContainer');

	self.serverUrl = serverUrl || 'neshto si';
	self.pages = ['gallery'];
	self.currentPage = ko.observable(false);
	self.currentUserID = ko.observable(false);

	self.sessionKey = ko.observable(false);
	
	self.login = function () {
		return $.ajax({
			type: 'POST',
			url: 'api/users/login',
			data: {
				email : $loginMail.val(),
				authCode : $loginPass.val()
			}
		}).done(function (response) {
		    $loginMail.val('');
		    $loginPass.val('');
		    self.sessionKey(response.sessionKey);
		    self.gotoPage('gallery');
		    self.renderPage('gallery');
		}).error(function (response) {
		    $loginBtn.popover('show');
		});
	};

	self.logout = function () {
		return $.ajax({
			type: 'GET',
			url: 'api/users/logout/' + self.sessionKey
		}).done(function (response) {
			self.currentUserID(false);
			self.removeHash();
			self.renderPage('main');
		});
	};

	self.attachPopover = function () {
		$loginBtn.popover({
			animation : true,
			placement : 'bottom',
			trigger : 'manual',
			content : 'Invalid Email or Password..'
		});
	};

	self.hidePopover = function () {
		$loginBtn.popover('hide');
	};

	self.gotoPage = function (page) {
		location.hash = page;
	};

	self.removeHash = function () { 
    	history.pushState(
    		"",
    		document.title,
    		window.location.pathname + window.location.search);
	};

	self.renderPage = function (page, id) {
		if (!page) {
			return;
		}

		if (id) {
			$pageContainer.load(page + 'IDPage.html');
		} else {
			$pageContainer.load(page + 'Page.html');
		}

		$pageContainer.hide().fadeIn(300);
	};

	self.initNavigation = function () {
		Sammy(function () {
			this.get('#:page', function() {
				self.currentPage(this.params.page);
				self.renderPage(this.params.page);
			});

			this.get('#:page/:pageID', function() {
				self.currentPage(false);
				self.renderPage(
					this.params.page,
					this.params.pageID);
			});

			this.get('', function () {
				if (self.sessionKey()) {
					this.app.runRoute('get', '#overview');
				} else {
					this.app.runRoute('get', '#main');
				}
			});
		}).run();
	};

	self.attachPopover();
	self.initNavigation();
};