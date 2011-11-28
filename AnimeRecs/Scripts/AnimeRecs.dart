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

String GetMalInputJson()
{
    MalInputJson jsonObject = new MalInputJson(GetMalUsername());
    return jsonObject.ToJson();
}

void OnMalClicked()
{
  print("OnMalClicked() begin");
  ButtonElement malButton = GetMalButton();
  malButton.disabled = true;
  
  try
  {
      XMLHttpRequest ajax = new XMLHttpRequest();
      String matchUrl = "FindBestMatch";
      String inputJson = GetMalInputJson();
      // TODO: Make async
      ajax.open("POST", matchUrl, false);
      ajax.setRequestHeader("Content-Type", "application/json");
      ajax.send(inputJson);
      // TODO: Handle errors
      
      // TODO: Handle malformed json
      print("Got response.");
      String responseString = ajax.responseText;
      RecommendationResultsJson results = new RecommendationResultsJson.FromJsonString(responseString);
      print("Parsed response. # liked = ${results.Liked.length}");
      
      DisplayResults(results);
  }
  finally
  {
    malButton.disabled = false;
    print("Re-enabled button.");
  }
}

void DisplayResults(RecommendationResultsJson results)
{
  Element resultsDiv = document.query("#results");
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
