class RecommendorMatchJson
{
  RecommendorJson Recommendor;
  double CompatibilityRating;
  
  RecommendorMatchJson.FromJsonObject(Map<String, Object> json)
  {
    _SetProperties(json);
  }
  
  void _SetProperties(Map<String, Object> jsonObject)
  {
    print("In RecommendorMatchJson._SetProperties");
    
    CompatibilityRating = jsonObject["CompatibilityRating"];
    Recommendor = new RecommendorJson.FromJsonObject(jsonObject["Recommendor"]);
    
    print("End of RecommendorMatchJson._SetProperties");
  }
}
