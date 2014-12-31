import 'dart:html';
import 'package:exportable/exportable.dart';

const String apiEndpoint = "/GetRecs";
const int timeoutInMs = 7000;
HtmlElements elements = new HtmlElements();
bool splashDisplayed = true;
bool waitingForResponse = false;
HttpRequest requestInProgress = null;

// Use this class to disable the stupid inner html sanitization
class NullTreeSanitizer implements NodeTreeSanitizer {
  const NullTreeSanitizer();
  void sanitizeTree(Node node) {}
}

final trustedHtml = const NullTreeSanitizer();

void main() {
  Element autoScoresDiv = querySelector("div.autoscores");
  if(autoScoresDiv != null) {
    autoScoresDiv.onClick.listen(onAutoScoreCheckboxClick);
  }
  
  querySelector("#header .title").onClick.listen((event) => displaySplash());
  
  querySelector(".datainput .usernamesubmit .go").onClick.listen((event) => handleSubmit());
    
  querySelector(".datainput .textinput").onKeyDown.listen( (KeyboardEvent e){
    if (e.keyCode == 13) // Enter
      handleSubmit();
  });
}

void onAutoScoreCheckboxClick(MouseEvent event) {
  bool checkedBeforeClick = elements.minScoreElements.automaticallyDetermineGoodScoreCheckbox.checked;
  if(checkedBeforeClick) {
    elements.minScoreElements.scoresDiv.style.display = "none";
  } else {
    elements.minScoreElements.scoresDiv.style.display = "block";
  }
}

void displaySplash() {
  splashDisplayed = true;
  cancelRecRequest();
  elements.mainDiv.classes
    ..remove("userinfo")
    ..add("splash");
  elements.resultsDiv.innerHtml = "";
}

void handleSubmit() {
  AnimeRecsInputJson inputJson = createInputJsonFromFormFields();
  if(inputJson.MalName != "") {
    submitRecRequest(inputJson);
  }
}

AnimeRecsInputJson createInputJsonFromFormFields() {
  String malName = elements.usernameInput.value;
  int goodCutoff = null;
  int goodPercentile = null;
  if(elements.minScoreElements != null && !elements.minScoreElements.automaticallyDetermineGoodScoreCheckbox.checked) {
    try {
      goodCutoff = int.parse(elements.minScoreElements.minGoodScoreInput.value);
    } on FormatException {
      ; // leave GoodCutoff null if not properly formatted
    }
  }
  String recSourceName = elements.recSourceNameInput.value;
  bool displayDetailedResults = elements.displayDetailedResultsHiddenInput.value == "True";
  
  List<int> animeIdsToWithhold = null;
  num percentOfAnimeToWithhold = null;
  
  if(elements.debugElements != null) {
    String animeIdsToWithholdString = elements.debugElements.animeIdsToWithholdInput.value;    
    List<String> animeIdsToWithholdStrings = animeIdsToWithholdString.split(",");
    List<String> animeIdsWithholdStringsNoEmpties = animeIdsToWithholdStrings.where((s) => s.trim() != "").toList();
    animeIdsToWithhold = animeIdsWithholdStringsNoEmpties.map((s) => int.parse(s)).toList();
    
    try {
      percentOfAnimeToWithhold = double.parse(elements.debugElements.percentToWithholdInput.value);
    } on FormatException {
      ; // leave it null if not properly formatted
    }
  }
  
  return new AnimeRecsInputJson(malName, goodCutoff, goodPercentile, recSourceName, displayDetailedResults,
      animeIdsToWithhold, percentOfAnimeToWithhold);
}

void submitRecRequest(AnimeRecsInputJson input) {
  if(waitingForResponse) {
    return;
  }
  
  waitingForResponse = true;
  elements.goButton.classes.add("loading");
  elements.resultsDiv.innerHtml = "";
  
  String json = input.toJson();
  
  HttpRequest request = new HttpRequest();
  request
    ..open("POST", apiEndpoint)
    ..timeout = timeoutInMs
    ..setRequestHeader("Accept", "application/json, text/javascript, */*; q=0.01")
    ..setRequestHeader("Content-Type", "application/json")
    ..onError.listen((progressEvent) => onRequestError(request))
    ..onLoad.listen((progressEvent) => onRequestComplete(request))
    ..onTimeout.listen((progressEvent) => onRequestTimeout(request))
    ..send(json);
  
  requestInProgress = request;
}

void cancelRecRequest() {
  if(requestInProgress == null) {
    return;
  }
  
  waitingForResponse = false;
  requestInProgress.abort();
  requestInProgress = null;
  elements.goButton.classes.remove("loading");
}

void onRequestError(HttpRequest request) {
  waitingForResponse = false;
  requestInProgress = null;
  elements.goButton.classes.remove("loading");
  
  AjaxError errorResult = tryGetErrorFromRequest(request);
  if(errorResult != null && errorResult.ErrorCode == "NoSuchMALUser") {
    window.alert(errorResult.Message);
  } else {
    window.alert("Sorry, something went wrong.");
  }
}

AjaxError tryGetErrorFromRequest(HttpRequest request) {
  //if(request.response != null && request.response is Map<String, Object>) {
  if(request.getResponseHeader("Content-Type").contains("application/json")) {
    AjaxError errorResult = new Exportable(AjaxError, request.responseText);
    return errorResult;
  }
  else {
    return null;
  }
}

void onRequestTimeout(HttpRequest request) {
  waitingForResponse = false;
  requestInProgress = null;
  elements.goButton.classes.remove("loading");
  window.alert("The server did not respond.");
}

void onRequestComplete(HttpRequest request) {
  // Send HTTP error responses to the error handler
  if(request.status != null && (request.status < 200 || request.status >= 300)) {
    onRequestError(request);
    return;
  }
  
  waitingForResponse = false;
  requestInProgress = null;
  elements.goButton.classes.remove("loading");
  
  if(splashDisplayed) {
    splashDisplayed = false;
    elements.mainDiv.classes
      ..remove("splash")
      ..add("userinfo");
  }
  
  RecResultsAsHtmlJson results = new Exportable(RecResultsAsHtmlJson, request.responseText);
  elements.resultsDiv.setInnerHtml(results.Html, treeSanitizer: trustedHtml);
}

class HtmlElements {
  Element mainDiv;
  TextInputElement usernameInput;
  Element goButton;
  Element resultsDiv;
  HiddenInputElement displayDetailedResultsHiddenInput;
  InputElement recSourceNameInput; // text if debug mode, otherwise hidden
  
  MinScoreHtmlElements minScoreElements;
  DebugModeHtmlElements debugElements;
  
  HtmlElements() {
    mainDiv = querySelector("#main");
    usernameInput = querySelector("#usernameInput");
    goButton = querySelector(".datainput .go");
    resultsDiv = querySelector("#results");
    displayDetailedResultsHiddenInput = querySelector("#displayDetailedResults");
    recSourceNameInput = querySelector("#recSourceName");
    
    minScoreElements = new MinScoreHtmlElements();
    if(minScoreElements.minGoodScoreInput == null) {
      minScoreElements = null;
    }
    
    debugElements = new DebugModeHtmlElements();
    if(debugElements.animeIdsToWithholdInput == null) {
      debugElements = null;
    }
  }
}

class MinScoreHtmlElements {
  TextInputElement minGoodScoreInput;
  Element scoresDiv;
  CheckboxInputElement automaticallyDetermineGoodScoreCheckbox;
  
  MinScoreHtmlElements() {
    minGoodScoreInput = querySelector("#minGoodScoreInput");
    if(minGoodScoreInput != null) {
      scoresDiv = querySelector(".scores");
      automaticallyDetermineGoodScoreCheckbox = querySelector("#autoscores");
    }
  }
}

class DebugModeHtmlElements {
  TextInputElement animeIdsToWithholdInput;
  TextInputElement percentToWithholdInput;
  
  DebugModeHtmlElements() {
    animeIdsToWithholdInput = querySelector("#withholdIds");
    if(animeIdsToWithholdInput != null) {
      percentToWithholdInput = querySelector("#withholdPercent");
    }
  }
}

@export
class AnimeRecsInputJson extends Object with Exportable {
  @export String MalName; // required
  @export List<int> AnimeIdsToWithhold; // nullable
  @export num PercentOfAnimeToWithhold; // cannot be null
  @export num GoodCutoff; // nullable
  @export num GoodPercentile; // nullable
  @export bool DisplayDetailedResults; // only applicable for AnimeRecs algorithm
  @export String RecSourceName;
  
  AnimeRecsInputJson(this.MalName, this.GoodCutoff, this.GoodPercentile, this.RecSourceName,
                     this.DisplayDetailedResults, this.AnimeIdsToWithhold, this.PercentOfAnimeToWithhold) {
    ;
  }
}

@export
class RecResultsAsHtmlJson extends Object with Exportable {
  @export String Html;
  
//  RecResultsAsHtmlJson.FromJson(Map<String, Object> json) {
//    Html = json["Html"];
//  }
}

@export
class AjaxError extends Object with Exportable {
  @export String ErrorCode;
  @export String Message;
  
//  AjaxError.FromJson(Map<String, Object> json) {
//    ErrorCode = json["ErrorCode"];
//    Message = json["Message"];
//  }
}