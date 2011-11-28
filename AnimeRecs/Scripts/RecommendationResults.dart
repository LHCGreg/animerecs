class RecommendationResultsJson
{
  double RecommendedCutoff; // nullable
  double OkCutoff; // nullable
  List<RecommendedAnimeJson> Liked;
  List<RecommendedAnimeJson> Ok;
  List<RecommendedAnimeJson> Disliked;
  List<RecommendorMatchJson> BestMatches;
  
  RecommendationResultsJson.FromJsonString(String json)
  {
    print("In RecommendationResultsJson.FromJsonString(String json)");
    _SetProperties(JSON.parse(json));
  }
  
  RecommendationResultsJson.FromJsonObject(Map<String, Object> json)
  {
    _SetProperties(json);
  }
  
  void _SetProperties(Map<String, Object> jsonObject)
  {
    print("In RecommendationResults._SetProperties");
    if(jsonObject.containsKey("RecommendedCutoff"))
    {
      RecommendedCutoff = jsonObject["RecommendedCutoff"];
    }
    if(jsonObject.containsKey("OkCutoff"))
    {
      OkCutoff = jsonObject["OkCutoff"];
    }
    
    print("In middle of RecommendationResults._SetProperties. RecommendedCutoff = $RecommendedCutoff, OkCutoff = $OkCutoff");
    
    List<Map<String, Object>> likedJsonList = jsonObject["Liked"];
    Liked = new List<RecommendedAnimeJson>();
    for(Map<String, Object> likedJsonObject in likedJsonList)
    {
      Liked.add(new RecommendedAnimeJson.FromJsonObject(likedJsonObject));
    }
    
    List<Map<String, Object>> okJsonList = jsonObject["Ok"];
    Ok = new List<RecommendedAnimeJson>();
    for(Map<String, Object> okJsonObject in okJsonList)
    {
      Ok.add(new RecommendedAnimeJson.FromJsonObject(okJsonObject));
    }
    
    List<Map<String, Object>> dislikedJsonList = jsonObject["Disliked"];
    Disliked = new List<RecommendedAnimeJson>();
    for(Map<String, Object> dislikedJsonObject in dislikedJsonList)
    {
      Disliked.add(new RecommendedAnimeJson.FromJsonObject(dislikedJsonObject));
    }
    
    List<Map<String, Object>> bestMatchesJsonList = jsonObject["BestMatches"];
    BestMatches = new List<RecommendorMatchJson>();
    for(Map<String, Object> bestMatchJsonObject in bestMatchesJsonList)
    {
      BestMatches.add(new RecommendorMatchJson.FromJsonObject(bestMatchJsonObject));
    }
    
    print("End of RecommendationResults._SetProperties");
  }
}
