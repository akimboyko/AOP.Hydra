<Query Kind="Program">
  <Connection>
    <ID>2a6fd500-f351-45ff-9741-1618576dd8e4</ID>
    <Persist>true</Persist>
    <Driver>AstoriaAuto</Driver>
    <Server>http://data.stackexchange.com/stackoverflow/atom</Server>
  </Connection>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
   //var targetTags = new [] { "decorator", "interceptor", "structuremap", "unity", "castle-windsor", "aop", "aspects", "linfu", "postsharp", "inversion-of-control", "ninject", "dependency-injection", "castle-dynamicproxy", "mono.cecil" };
   var targetTags = new [] { "decorator", "interceptor", "aop", "aspects", "linfu", "postsharp", "castle-dynamicproxy", "mono.cecil" };
    
    var dataMinning = 
        targetTags
//            .Take(1)
            .Select(targetTag => 
                new
                {
                    targetTag,
                    taggedPosts = 
                        Posts
                            .Where(post => !post.Tags.Contains("java"))
                            .Where(post => !post.Tags.Contains("python"))
                            .Where(post => post.Tags.Contains(string.Format("<{0}>", targetTag)))
                            .Count(),
                    topPosts = 
                        Posts
                            .Where(post => !post.Tags.Contains("java"))
                            .Where(post => !post.Tags.Contains("python"))
                            .Where(post => post.Tags.Contains(string.Format("<{0}>", targetTag)))
                            .OrderByDescending(post => post.Score)
                            .ToList()
                            .Select(post => new { post.Title, post.Score, post.AnswerCount, post.Tags })
                })
                .OrderByDescending(targetTag => targetTag.taggedPosts)
                .ToList();
    
    //var titles = dataMinning.SelectMany(data => data.topPosts).Select(post => post.Title);
    //var tags = dataMinning.SelectMany(data => data.topPosts).SelectMany(post => ExtractTags(post.Tags));
    
    dataMinning.Select(data => new { data.targetTag, data.taggedPosts }).Dump();
}

static readonly Regex tagsRegex = new Regex("(<([a-z\\.0-9\\-\\#]+)>)+", RegexOptions.Compiled);

IEnumerable<string> ExtractTags(string postTags)
{
    var match = tagsRegex.Match(postTags);
    
    if(match.Success)
    {
        for(int n = 0; n < match.Groups[match.Groups.Count - 1].Captures.Count; n++)
        {
            yield return match.Groups[match.Groups.Count - 1].Captures[n].Value.Replace("-", " ");
        }
    }
}