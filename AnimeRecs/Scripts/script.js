(function($){
	
	// Settings
	var apiendpoint = "/FindBestMatch";
	var timeout = 7000;
	
	// Default values -- should match up with HTML
	var reccomendedDefault = 8;
	
	// Activate jQuery plugins
	$('input[placeholder], textarea[placeholder]').placeholder();
	
	// Cache elements
	var main = $("#main");
	var field_username = $(".datainput .username");
	var field_good = $(".datainput .good");
	var gobutton = $(".datainput .go");
	var results = $("#results");
	var scores = $(".scores");
	var autoscoretoggle = $("input#autoscores");
	
	// Interactivity
	$("div.autoscores").click(function(){
		var autoscores = autoscoretoggle.attr("checked");
		scores.css({ display: (autoscores ? "none" : "block") });
	});
	
	
	// Bind event handlers
	$("#header .title").click(function(){
		app.displaySplash();
	});
	
	$(".datainput .usernamesubmit .go").click(function(){
		handleDataSubmit();
	});
	
	$(".datainput .textinput").keydown(function(e){
		if(e.keyCode == 13) // Enter
			handleDataSubmit();
	})
	
	function handleDataSubmit(datainputdiv){
		var username = field_username.val();
		
		var good = parseInt(field_good.val());
		if(isNaN(good))
			good = reccomendedDefault;
		
		if(username) {
			if(autoscoretoggle.attr("checked"))
				app.displayUserdata(username, undefined);
			else
				app.displayUserdata(username, good);
		}
	}
	
	
	
	
	
	// Page handling / app logic
	var app = new (function(){
	
		var splashDisplayed = true;
		var waitingForResponse = false;
		var cancelLoad = function(){};
		
		this.displaySplash = function(){
			splashDisplayed = true;
			cancelLoad();
			main.removeClass("userinfo").addClass("splash");
			results.html("");
		}
		
		this.displayUserdata = function(username, rec){
			if(waitingForResponse)
				return;
			
			waitingForResponse = true;
			gobutton.addClass("loading");
			results.html("");
			
			var cancelGetData = getData(username, rec, function(status, result){
				waitingForResponse = false;
				gobutton.removeClass("loading");
				
				if(status == "success"){
					if(splashDisplayed){
						splashDisplayed = false;
						main.removeClass("splash").addClass("userinfo");
					}
					
					if(result.RecommendedCutoff != null)
						field_good.val(result.RecommendedCutoff);
					
					
					results.html(result.Html);
				} else if(status == "error") {
					alert("Sorry, something went wrong.");
				}
				
			});
			
			cancelLoad = function(){
				if(!waitingForResponse)
					return;
				
				cancelGetData();
				waitingForResponse = false;
				gobutton.removeClass("loading");
			}
		};
		
		
		function getData(username, rec, callback){
			
			var data;
			
			if (rec != undefined) { // Specified scores
				data = { "MalName" : username, "GoodCutoff": rec };
			} else { // figure out scores
				data = { "MalName" : username };
			}
			
			var rq = $.ajax({
				// send
				url: apiendpoint,
				type: "POST",
				data: data,
				
				// recieve
				dataType: "json",
				timeout: timeout,
				
				success: function(data, textStatus, jqXHR){
					callback("success", data);
				},
				
				error: function(jqXHR, textStatus, errorThrown){
					if (textStatus == "abort") { // request was aborted by rq.abort()
						// we do nothing here, as it was aborted from client side code
					} else { // error
						callback("error");
					}
				}
			});
			
			//
			
			return function(){
				rq.abort();
			};
		};
	
	})();
	
})(jQuery)