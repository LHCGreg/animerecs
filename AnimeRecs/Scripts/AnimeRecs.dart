#import('dart:html');
#import("dart:json");
#source('MalInputJson.dart');

void main()
{
  ButtonElement malButton = document.query("#malButton");
  malButton.on.click.add( (event) => OnMalClicked() );
  
  // If the button was disabled when the user left the page last time for some reason,
  // The browser might leave it disabled. Make sure it's enabled.
  malButton.disabled = false;
  
  InputElement malTextbox = document.query("#malUserId");
  malTextbox.select();
  malTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
  
  InputElement goodTextbox = document.query("#goodCutoff");
  goodTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
  
  InputElement okTextbox = document.query("#okCutoff");
  okTextbox.on.keyDown.add( (event) => OnTextboxKeyDown(event) );
}

void OnTextboxKeyDown(var event)
{
  if(event.keyCode == 13)
  {
    OnMalClicked();
  }
}

String GetMalUsername()
{
  InputElement malTextbox = document.query("#malUserId");
  String text = malTextbox.value;
  return text;
}

ButtonElement GetMalButton()
{
  return document.query("#malButton");  
}

int GetGoodCutoff()
{
  InputElement textbox = document.query("#goodCutoff");
  String text = textbox.value;
  return Math.parseInt(text);
}

int GetOkCutoff()
{
  InputElement textbox = document.query("#okCutoff");
  String text = textbox.value;
  return Math.parseInt(text);
}

String GetMalInputJson()
{  
  MalInputJson jsonObject = new MalInputJson.WithAbsoluteCutoffs(GetMalUsername(), GetGoodCutoff(), GetOkCutoff());
  return jsonObject.ToJson();
}

void OnMalClicked()
{
  ButtonElement malButton = GetMalButton();
  malButton.disabled = true;
  
  try
  {
      XMLHttpRequest ajax = new XMLHttpRequest();
      String matchUrl = "FindBestMatch";
      String inputJson = GetMalInputJson();
      
      ajax.open("POST", matchUrl, true);
      ajax.on.load.add( (event) => OnMalAjaxComplete(ajax));
      ajax.on.error.add( (event) => OnMalAjaxError(ajax));
      ajax.setRequestHeader("Content-Type", "application/json");
      ajax.send(inputJson);
  }
  catch(var ex)
  {
    malButton.disabled = false;
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
  
  ButtonElement malButton = GetMalButton();
  malButton.disabled = false;
  HideLoadingImage();
  
  String resultsHtml = ajax.responseText;
  DisplayResultsHtml(resultsHtml);
}

void OnMalAjaxError(XMLHttpRequest ajax)
{
  ButtonElement malButton = GetMalButton();
  malButton.disabled = false;
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

void DisplayResultsHtml(String resultsHtml)
{
  Element resultsDiv = GetResultsDiv();
  resultsDiv.nodes.clear();
  resultsDiv.innerHTML = resultsHtml;
}
