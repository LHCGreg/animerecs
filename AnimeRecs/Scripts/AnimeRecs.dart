#import('dart:html');
#import("dart:json");
#source('MalInputJson.dart');
#source('RecommendationResults.dart');
#source('RecommendedAnimeJson.dart');
#source('RecommendorMatchJson.dart');
#source('RecommendorJson.dart');

void main()
{
  print("main");
  ButtonElement malButton = document.query("#malButton");
  malButton.on.click.add( (event) => OnMalClicked() );
  malButton.disabled = false;
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
  catch(var ex) // XXX: Is this correct?
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
  
  // TODO: Handle malformed json
  print("Got response.");
  String responseString = ajax.responseText;
  RecommendationResultsJson results = new RecommendationResultsJson.FromJsonString(responseString);
  print("Parsed response. # liked = ${results.Liked.length}");
  
  DisplayResults(results);
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

void DisplayResults(RecommendationResultsJson results)
{
  Element resultsDiv = GetResultsDiv();
  resultsDiv.nodes.clear();
  resultsDiv.text = "Your best matches are";
  resultsDiv.elements.add(GetMatchListOl(results));
}

Element GetMatchListOl(RecommendationResultsJson results)
{
  Element matchListOl = new Element.tag("ol");
  matchListOl.classes.add("matchList");
  
  // TODO: Sort by compatibility rating
  
  for(RecommendorMatchJson recommendor in results.BestMatches)
  {
    matchListOl.elements.add(GetRecommendorLi(recommendor, results));
  }
  
  return matchListOl;
}

Element GetRecommendorLi(RecommendorMatchJson recommendor, RecommendationResultsJson results)
{
  Element li = new Element.tag("li");
  Element recommendorLink = new Element.tag("a");
  // TODO: sanitize
  recommendorLink.attributes["href"] = "http://myanimelist.net/animelist/" + recommendor.Recommendor.Name;
  recommendorLink.classes.add("recommendor");
  recommendorLink.text = recommendor.Recommendor.Name;
  
  li.elements.add(recommendorLink);
  
  Element compatibilitySpan = new Element.tag("span");
  compatibilitySpan.classes.add("compatibilityRating");
  compatibilitySpan.text = " (${recommendor.CompatibilityRating}%)";
  li.elements.add(compatibilitySpan);
  
  Element recommendationListOl = new Element.tag("ol");
  recommendationListOl.classes.add("recommendationList");
  // TODO: Sort by rating, then alphabetically
  for(RecommendedAnimeJson recommendation in recommendor.Recommendor.Recommendations)
  {
    // Only show recommendation if it hasn't been seen.
    if(!results.Liked.some( (recommendedAnimeJson) => recommendedAnimeJson.MalId == recommendation.MalId)
        && !results.Ok.some((recommendedAnimeJson) => recommendedAnimeJson.MalId == recommendation.MalId)
        && !results.Disliked.some( (recommendedAnimeJson) => recommendedAnimeJson.MalId == recommendation.MalId))
    {
      recommendationListOl.elements.add(GetRecommendationLi(recommendation));
    }
  }
  li.elements.add(recommendationListOl);
  
  return li;
}

Element GetRecommendationLi(RecommendedAnimeJson anime)
{
  Element li = new Element.tag("li");
  
  Element animeLink = new Element.tag("a");
  animeLink.attributes["href"] = anime.MalUrl;
  animeLink.classes.add("recommendation");
  animeLink.text = anime.Name;
  li.elements.add(animeLink);
  
  Element ratingSpan = new Element.tag("span");
  ratingSpan.classes.add("recomendationRating");
  String ratingText;
  if(anime.Rating != null)
  {
    ratingText = anime.Rating.toString();
  }
  else
  {
    ratingText = "?";
  }
  
  ratingSpan.text = " ($ratingText)";
  li.elements.add(ratingSpan);
  
  return li;
}
