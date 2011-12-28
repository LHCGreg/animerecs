#import('dart:html');
#import("dart:json");
#source('MalInputJson.dart');
#source('RecommendationResultsWithHtmlJson.dart');

void main()
{
  ButtonElement malButton = GetMalButton();
  malButton.on.click.add( (event) => OnMalClicked() );
  
  ButtonElement malPercentileButton = GetMalPercentileButton();
  malPercentileButton.on.click.add( (event) => OnMalPercentileClicked() );
  
  // If the button was disabled when the user left the page last time for some reason,
  // The browser might leave it disabled. Make sure it's enabled.
  malButton.disabled = false;
  malPercentileButton.disabled = false;
  
  InputElement malTextbox = GetMalUserTextbox();
  malTextbox.select();
  malTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
  
  InputElement goodTextbox = GetGoodTextbox();
  goodTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
  
  InputElement okTextbox = GetOkTextbox();
  okTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
}

void OnTextboxKeyDown(var event)
{
  ButtonElement malButton = GetMalButton();
  if(event.keyCode == 13 && !malButton.disabled)
  {
    OnMalClicked();
  }
}

String GetMalUsername()
{
  InputElement malTextbox = GetMalUserTextbox();
  String text = malTextbox.value;
  return text;
}

ButtonElement GetMalButton()
{
  return document.query("#malButton");  
}

ButtonElement GetMalPercentileButton()
{
  return document.query("#malPercentileButton");  
}

InputElement GetMalUserTextbox()
{
  return document.query("#malUserId");
}

InputElement GetGoodTextbox()
{
  return document.query("#goodCutoff");
}

InputElement GetOkTextbox()
{
  return document.query("#okCutoff");  
}

int GetGoodCutoff()
{
  InputElement textbox = GetGoodTextbox();
  String text = textbox.value;
  return Math.parseInt(text);
}

int GetOkCutoff()
{
  InputElement textbox = GetOkTextbox();
  String text = textbox.value;
  return Math.parseInt(text);
}

void SetCutoffs(double goodCutoff, double okCutoff)
{
  InputElement goodTextbox = GetGoodTextbox();
  goodTextbox.value = goodCutoff.toString();
  
  InputElement okTextbox = GetOkTextbox();
  okTextbox.value = okCutoff.toString();
}

String GetMalInputJsonWithAbsoluteCutoffs()
{  
  MalInputJson jsonObject = new MalInputJson.WithAbsoluteCutoffs(GetMalUsername(), GetGoodCutoff(), GetOkCutoff());
  return jsonObject.ToJson();
}

String GetMalInputJsonWithPercentileCutoffs()
{
  MalInputJson jsonObject = new MalInputJson(GetMalUsername());
  return jsonObject.ToJson();
}

void SetButtonState(bool enabled)
{
  ButtonElement malButton = GetMalButton();
  malButton.disabled = !enabled;
  
  ButtonElement malPercentileButton = GetMalPercentileButton();
  malPercentileButton.disabled = !enabled;
}

void DisableButtons()
{
  SetButtonState(false);  
}

void EnableButtons()
{
  SetButtonState(true);  
}

void OnMalClicked()
{
  OnButtonClicked(true);
}

void OnMalPercentileClicked()
{
  OnButtonClicked(false);
}

void OnButtonClicked(bool absoluteCutoffs)
{
  DisableButtons();
  
  try
  {
      XMLHttpRequest ajax = new XMLHttpRequest();
      String matchUrl = "FindBestMatch";
      String inputJson;
      if(absoluteCutoffs)
      {
        inputJson = GetMalInputJsonWithAbsoluteCutoffs();
      }
      else
      {
        inputJson = GetMalInputJsonWithPercentileCutoffs();
      }
      
      ajax.open("POST", matchUrl, true);
      ajax.on.load.add( (event) => OnMalAjaxComplete(ajax));
      ajax.on.error.add( (event) => OnMalAjaxError(ajax));
      ajax.setRequestHeader("Content-Type", "application/json");
      ajax.send(inputJson);
  }
  catch(var ex)
  {
    EnableButtons();
    throw;
  }
  
  ShowLoadingImage();
}

void OnMalAjaxComplete(XMLHttpRequest ajax)
{
  if(ajax.status < 200 || ajax.status >= 300)
  {
    OnMalAjaxError(ajax);
    return;
  }
  
  EnableButtons();
  HideLoadingImage();
  
  try
  {
    String resultsWithHtmlJsonString = ajax.responseText;
    RecommendationResultsWithHtmlJson resultsObject = new RecommendationResultsWithHtmlJson.FromJsonString(resultsWithHtmlJsonString);
    DisplayResults(resultsObject);
  }
  catch(var ex)
  {
    DisplayError();
  }
}

void OnMalAjaxError(XMLHttpRequest ajax)
{
  EnableButtons();
  HideLoadingImage();
  DisplayError();
}

void ShowLoadingImage()
{
  Element loadingImageDiv = GetLoadingImageDiv();
  Element loadingImg = new Element.tag("img");
  loadingImg.attributes["src"] = "Content/img/ajax-loader.gif";
  loadingImg.attributes["alt"] = "Loading...";
  loadingImageDiv.elements.add(loadingImg);
}

void HideLoadingImage()
{
  Element loadingImageDiv = GetLoadingImageDiv();
  loadingImageDiv.elements.clear();
}

Element GetLoadingImageDiv()
{
  return document.query("#loadingImageDiv"); 
}

Element GetResultsDiv()
{
  return document.query("#results");  
}

void DisplayError()
{
  Element resultsDiv = GetResultsDiv();
  resultsDiv.nodes.clear();
  Element errorTextSpan = new Element.tag("span");
  errorTextSpan.classes.add("Error");
  errorTextSpan.text = "Sorry, something went wrong.";
  resultsDiv.elements.add(errorTextSpan);
}

void DisplayResults(RecommendationResultsWithHtmlJson results)
{
  Element resultsDiv = GetResultsDiv();
  resultsDiv.nodes.clear();
  resultsDiv.innerHTML = results.Html;
  
  SetCutoffs(results.RecommendedCutoff, results.OkCutoff);
}
