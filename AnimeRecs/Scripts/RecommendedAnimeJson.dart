class RecommendedAnimeJson
{
  int MalId;
  String MalUrl;
  String Name;
  double Rating; // nullable
  
  RecommendedAnimeJson.FromJsonObject(Map<String, Object> json)
  {
    _SetProperties(json);
  }
  
  void _SetProperties(Map<String, Object> jsonObject)
  {
    print("In RecommendedAnimeJson._SetProperties");
    MalId = jsonObject["MalId"];
    MalUrl = jsonObject["MalUrl"];
    Name = jsonObject["Name"];
    
    if(jsonObject.containsKey("Rating"))
    {
      Rating = jsonObject["Rating"];
    }
    
    print("End of RecommendedAnimeJson._SetProperties");
  }
}
