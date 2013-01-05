<Query Kind="Statements">
  <Connection>
    <ID>2a6fd500-f351-45ff-9741-1618576dd8e4</ID>
    <Persist>true</Persist>
    <Driver>AstoriaAuto</Driver>
    <Server>http://data.stackexchange.com/stackoverflow/atom</Server>
  </Connection>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

var statistics =
    Tags
        .Where(tag => 
            false
            || tag.TagName.Contains("decorator")
            || tag.TagName.Contains("interceptor")
            || tag.TagName.Contains("aop")
            || tag.TagName.Contains("aspect")
            || tag.TagName.Contains("linfu")
            || tag.TagName.Contains("postsharp")
            || tag.TagName.Contains("dynamicproxy")
            || tag.TagName.Contains("mono.cecil")
            || tag.TagName.Contains("reflection.emit")
            || tag.TagName.Contains("mono.cecil")
            //|| tag.TagName.Contains("structuremap")
            //|| tag.TagName.Contains("unity")
            //|| tag.TagName.Contains("castle-windsor")
            //|| tag.TagName.Contains("ninject")
            || false)
        .Where(tag => !tag.TagName.Contains("aspectj"))
        .Where(tag => !tag.TagName.Contains("spring-aop"))
        .Where(tag => !tag.TagName.Contains("zend"))
        .Where(tag => !tag.TagName.Contains("unity3d"))
        .Where(tag => !tag.TagName.Contains("adornerdecorator"))
        .Where(tag => !tag.TagName.Contains("aopalliance"))
        .Where(tag => !tag.TagName.Contains("aspect-fit"))
        .ToList()
        .Select(tag => new
                        {
                            tag.TagName,
                            TotalPosts = Posts
                                        .Where(post => post.Tags.Contains(tag.TagName))
                                        .Where(post => !post.Tags.Contains("java"))
                                        .Where(post => !post.Tags.Contains("php"))
                                        .Where(post => !post.Tags.Contains("python"))
                                        .Count(),
                            VotedPosts = Posts
                                        .Where(post => post.Tags.Contains(tag.TagName))
                                        .Where(post => !post.Tags.Contains("java"))
                                        .Where(post => !post.Tags.Contains("php"))
                                        .Where(post => !post.Tags.Contains("python"))
                                        .OrderByDescending(post => post.Score)
                                        .ToList()
                        })
        .OrderByDescending(tagInfo => tagInfo.TotalPosts)
        .ToList();


var text = 
        statistics
            .AsParallel()
            .Select(tag => string.Format("{0} {1}",
                                string.Join(" ", Enumerable
                                                    .Range(0, tag.TotalPosts)
                                                    .Select(n => tag.TagName)),
                                string.Join(" ", tag.VotedPosts
                                                    .Select(post => 
                                                        string.Format("{0} {1}", post.Title, post.Body)))));
                                
text.Dump();