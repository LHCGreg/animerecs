class MalInputJson
{
  final String MalName;
  final int GoodCutoff;
  final int OkCutoff;
  final double GoodPercentile;
  final double DislikedPercentile;
  
  MalInputJson(String this.MalName);
  MalInputJson.WithAbsoluteCutoffs(String this.MalName, int this.GoodCutoff, int this.OkCutoff);
  MalInputJson.WithPercentileCutoffs(String this.MalName, double this.GoodPercentile, double this.DislikedPercentile);
  
  String ToJson()
  {
    Map<String, Object> jsonMap = new Map<String, Object>();
    jsonMap["MalName"] = MalName;
    if(GoodCutoff != null)
    {
      jsonMap["GoodCutoff"] = GoodCutoff;
      jsonMap["OkCutoff"] = OkCutoff;
    }
    else if(GoodPercentile != null)
    {
      jsonMap["GoodPercentile"] = GoodPercentile;
      jsonMap["DislikedPercentile"] = DislikedPercentile;
    }
    else
    {
      ; // Leave them unset and let the server choose a default
    }
    
    return JSON.stringify(jsonMap);
  }
}
