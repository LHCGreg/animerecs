(function ($) {

    // Settings
    var apiendpoint = "/GetRecs";
    var timeout = 7000;

    // Default values -- should match up with HTML
    var reccomendedDefault = 8;

    // Activate jQuery plugins
    $('input[placeholder], textarea[placeholder]').placeholder();

    // Cache elements
    var main = $("#main");
    var field_username = $(".datainput .username");

    var field_good = $(".datainput .good");
    if (field_good.length == 0) {
        field_good = null;
    }
    var scores = $(".scores");
    if (scores.length == 0) {
        scores = null;
    }
    var autoscoretoggle = $("input#autoscores");
    if (autoscoretoggle.length == 0) {
        autoscoretoggle = null;
    }

    var gobutton = $(".datainput .go");
    var results = $("#results");

    var field_algorithm = $("#recSourceName");

    // Interactivity
    $("div.autoscores").click(function () {
        var autoscores = autoscoretoggle.attr("checked");
        scores.css({ display: (autoscores ? "none" : "block") });
    });


    // Bind event handlers
    $("#header .title").click(function () {
        app.displaySplash();
    });

    $(".datainput .usernamesubmit .go").click(function () {
        handleDataSubmit();
    });

    $(".datainput .textinput").keydown(function (e) {
        if (e.keyCode == 13) // Enter
            handleDataSubmit();
    })

    function handleDataSubmit(datainputdiv) {
        var username = field_username.val();

        var good = undefined;
        if (field_good != null) {
            good = parseInt(field_good.val());
            if (isNaN(good))
                good = reccomendedDefault;
        }

        var algorithm = field_algorithm.val();

        if (username) {
            if (field_good != null && autoscoretoggle.attr("checked"))
                app.displayUserdata(username, algorithm, undefined);
            else
                app.displayUserdata(username, algorithm, good);
        }
    }





    // Page handling / app logic
    var app = new (function () {

        var splashDisplayed = true;
        var waitingForResponse = false;
        var cancelLoad = function () { };

        this.displaySplash = function () {
            splashDisplayed = true;
            cancelLoad();
            main.removeClass("userinfo").addClass("splash");
            results.html("");
        }

        this.displayUserdata = function (username, algorithm, rec) {
            if (waitingForResponse)
                return;

            waitingForResponse = true;
            gobutton.addClass("loading");
            results.html("");

            var cancelGetData = getData(username, rec, algorithm, function (status, result) {
                waitingForResponse = false;
                gobutton.removeClass("loading");

                if (status == "success") {
                    if (splashDisplayed) {
                        splashDisplayed = false;
                        main.removeClass("splash").addClass("userinfo");
                    }

                    results.html(result.Html);
                } else if (status == "error") {
                    var message = "Sorry, something went wrong.";
                    if (result) {
                        message = result.Message;
                    }
                    alert(message);
                }

            });

            cancelLoad = function () {
                if (!waitingForResponse)
                    return;

                cancelGetData();
                waitingForResponse = false;
                gobutton.removeClass("loading");
            }
        };


        function getData(username, rec, algorithm, callback) {

            var data;

            if (rec != undefined) { // Specified scores
                data = { "MalName": username, "GoodCutoff": rec, "RecSourceName": algorithm };
            } else { // figure out scores or rec source does not use a target score
                data = { "MalName": username, "RecSourceName": algorithm };
            }

            var rq = $.ajax({
                // send
                url: apiendpoint,
                type: "POST",
                data: data,

                // recieve
                dataType: "json",
                timeout: timeout,

                success: function (data, textStatus, jqXHR) {
                    callback("success", data);
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    if (textStatus == "abort") { // request was aborted by rq.abort()
                        // we do nothing here, as it was aborted from client side code
                    } else { // error
                        var errorData = null;
                        if (jqXHR.getResponseHeader("Content-Type") && jqXHR.getResponseHeader("Content-Type").indexOf("application/json") !== -1) {
                            errorData = $.parseJSON(jqXHR.responseText);
                        }
                        callback("error", errorData);
                    }
                }
            });

            return function () {
                rq.abort();
            };
        };

    })();

})(jQuery)