class RecommendationResultsWithHtmlJson
{
  double RecommendedCutoff; // nullable
  double OkCutoff; // nullable
  String Html;
  
  RecommendationResultsWithHtmlJson.FromJsonString(String json)
  {
    Map<String, Object> jsonMap = JSON.parse(json);
    
    RecommendedCutoff = jsonMap["RecommendedCutoff"];
    OkCutoff = jsonMap["OkCutoff"];
    Html = jsonMap["Html"];
  }
}
