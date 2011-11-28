class RecommendorJson
{
  String Name;
  List<RecommendedAnimeJson> Recommendations;
  
  RecommendorJson.FromJsonObject(Map<String, Object> json)
  {
    _SetProperties(json);
  }
  
  void _SetProperties(Map<String, Object> jsonObject)
  {
    print("In RecommendorJson._SetProperties");
    Name = jsonObject["Name"];
    
    List<Map<String, Object>> recommendedAnimeJsonList = jsonObject["Recommendations"];
    Recommendations = new List<RecommendedAnimeJson>();
    for(Map<String, Object> recommendedAnimeJsonObject in recommendedAnimeJsonList)
    {
      Recommendations.add(new RecommendedAnimeJson.FromJsonObject(recommendedAnimeJsonObject));
    }
    
    print("End of RecommendorJson._SetProperties");
  }
}
