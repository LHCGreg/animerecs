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
    var field_detailedResults = $("#displayDetailedResults");

    var field_withholdIds = $("#withholdIds");
    if (field_withholdIds.length == 0) {
        field_withholdIds = null;
    }

    var field_withholdPercent = $("#withholdPercent");
    if (field_withholdPercent.length == 0) {
        field_withholdPercent = null;
    }

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
        var inputJson = new Object();
        inputJson.MalName = field_username.val();

        if (field_good != null && !autoscoretoggle.attr("checked")) {
            var goodCutoff = parseInt(field_good.val());
            if (!isNaN(goodCutoff)) {
                inputJson.GoodCutoff = goodCutoff;
            }
        }

        inputJson.RecSourceName = field_algorithm.val();
        inputJson.DisplayDetailedResults = field_detailedResults.val() === "True";

        if (field_withholdIds != null) {
            var animeIdsToWithholdString = field_withholdIds.val();
            var animeIdsToWithholdStrings = animeIdsToWithholdString.split(",");
            var animeIdsWithholdStringsNoEmpties = animeIdsToWithholdStrings.filter(function (str) { return !/^\s*$/.test(str); });
            var animeIdsToWithhold = animeIdsWithholdStringsNoEmpties.map(function (str) { return parseInt(str, 10); });
            inputJson.AnimeIdsToWithhold = animeIdsToWithhold;
        }

        if (field_withholdPercent != null) {
            var withholdPercent = parseFloat(field_withholdPercent.val());
            if (!isNaN(withholdPercent)) {
                inputJson.PercentOfAnimeToWithhold = withholdPercent;
            }
        }

        if (inputJson.MalName) {
            app.displayUserdata(inputJson);
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

        this.displayUserdata = function (inputJson) {
            if (waitingForResponse)
                return;

            waitingForResponse = true;
            gobutton.addClass("loading");
            results.html("");

            var cancelGetData = getData(inputJson, function (status, result) {
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


        function getData(inputJson, callback) {
            var rq = $.ajax({
                // send
                url: apiendpoint,
                type: "POST",
                data: JSON.stringify(inputJson),
                contentType: "application/json",

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