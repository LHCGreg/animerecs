import 'dart:html';
import 'package:exportable/exportable.dart';

const String apiEndpoint = "/GetRecs";
const int timeoutInMs = 7000;
HtmlElements elements = new HtmlElements();
bool inResultsMode = false; // When not in results mode, input is centered
HttpRequest requestInProgress = null;

// Use this class to disable the stupid inner html sanitization
class NullTreeSanitizer implements NodeTreeSanitizer {
  const NullTreeSanitizer();
  void sanitizeTree(Node node) {}
}

final trustedHtml = const NullTreeSanitizer();

void main() {
  // Set up event handlers
  //TODO: Update this
  Element autoScoresDiv = querySelector("div.autoscores");
  if(autoScoresDiv != null) {
    autoScoresDiv.onClick.listen(onAutoScoreCheckboxClick);
  }
    
  elements.goButton.onClick.listen((event) => handleSubmit());
    
  elements.usernameInput.onKeyDown.listen( (KeyboardEvent e){
    if (e.keyCode == 13) // Enter
      handleSubmit();
  });
}

void onAutoScoreCheckboxClick(MouseEvent event) {
  // TODO: update this
  bool checkedBeforeClick = elements.minScoreElements.automaticallyDetermineGoodScoreCheckbox.checked;
  if(checkedBeforeClick) {
    elements.minScoreElements.scoresDiv.style.display = "none";
  } else {
    elements.minScoreElements.scoresDiv.style.display = "block";
  }
}

void handleSubmit() {
  if(requestInProgress != null) {
    return;
  }
  
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
  enterLoadingMode();
  
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

void enterLoadingMode() {
  elements.goButton.disabled = true;
  elements.resultsDiv.innerHtml = "";
}

void exitLoadingMode() {
  elements.goButton.disabled = false;
}

void enterResultsMode() {
  if(inResultsMode) {
    return;
  }
  
  // remove viewport-center class from main-content-outer
  elements.mainContentOuterDiv.classes.remove("viewport-center");
  // remove div-center class from main-content-inner
  elements.mainContentInnerDiv.classes.remove("div-center");
  // add hidden to div with title class
  elements.titleDiv.classes.add("hidden");
  // add col-xs-6 class to anything with input-col-xs-6 class
  ElementList col6Elements = querySelectorAll(".input-col-xs-6");
  col6Elements.classes.add("col-xs-6");
  elements.footer.classes.remove("footer-no-results");
  elements.footer.classes.add("footer-results");
  inResultsMode = true;
}

void displayResults(RecResultsAsHtmlJson results) {
  enterResultsMode();
  elements.resultsDiv.setInnerHtml(results.Html, treeSanitizer: trustedHtml);
}

void onRequestError(HttpRequest request) {
  requestInProgress = null;
  exitLoadingMode();
  
  AjaxError errorResult = tryGetErrorFromRequest(request);
  if(errorResult != null && errorResult.ErrorCode == "NoSuchMALUser") {
    window.alert(errorResult.Message);
  } else {
    window.alert("Sorry, something went wrong.");
  }
}

AjaxError tryGetErrorFromRequest(HttpRequest request) {
  if(request.getResponseHeader("Content-Type").contains("application/json")) {
    AjaxError errorResult = new Exportable(AjaxError, request.responseText);
    return errorResult;
  }
  else {
    return null;
  }
}

void onRequestTimeout(HttpRequest request) {
  requestInProgress = null;
  exitLoadingMode();
  window.alert("The server did not respond.");
}

void onRequestComplete(HttpRequest request) {
  // Send HTTP error responses to the error handler
  if(request.status != null && (request.status < 200 || request.status >= 300)) {
    onRequestError(request);
    return;
  }
  
  requestInProgress = null;
  exitLoadingMode();
  
  RecResultsAsHtmlJson results = new Exportable(RecResultsAsHtmlJson, request.responseText);
  displayResults(results);
  //elements.resultsDiv.setInnerHtml(results.Html, treeSanitizer: trustedHtml);
}

class HtmlElements {
  Element mainContentOuterDiv;
  Element mainContentInnerDiv;
  Element titleDiv;
  TextInputElement usernameInput;
  ButtonElement goButton;
  Element resultsDiv;
  HiddenInputElement displayDetailedResultsHiddenInput;
  InputElement recSourceNameInput; // text if debug mode, otherwise hidden
  Element footer;
  
  MinScoreHtmlElements minScoreElements;
  DebugModeHtmlElements debugElements;
  
  HtmlElements() {
    mainContentOuterDiv = querySelector("#main-content-outer");
    mainContentInnerDiv = querySelector("#main-content-inner");
    titleDiv = querySelector(".title");
    usernameInput = querySelector("#usernameInput");
    goButton = querySelector("#goButton");
    resultsDiv = querySelector("#results");
    displayDetailedResultsHiddenInput = querySelector("#displayDetailedResults");
    recSourceNameInput = querySelector("#recSourceName");
    footer = querySelector("#footer");
    
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