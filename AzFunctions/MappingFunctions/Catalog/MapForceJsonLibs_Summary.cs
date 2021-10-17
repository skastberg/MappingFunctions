using Acceptors = Altova.Json;
namespace Catalog.MapForceJsonLibs
{
    public class Summary 
    {
        public static Acceptors.ValueAcceptor[] Schemas = 
        {
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Welcome2/@64", 
                null, 
                null,
                null,
                null,
                null,
                new Acceptors.ObjectAcceptor(null, null, new Altova.Json.PropertyGroup[]{
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("summary", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Welcome2//properties//summary/@32"), null, new Acceptors.Reference("##fail")),
                   } ),
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("", Acceptors.NameMatchKind.All, new Acceptors.Reference("##fail"), null, null),
                   } ),
                }),
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Welcome2//properties//summary/@32", 
                null, 
                null,
                null,
                null,
                new Acceptors.ArrayAcceptor(null, null, false, new Acceptors.Reference[]{new Acceptors.Reference("file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary/@64"), }),
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary/@64", 
                null, 
                null,
                null,
                null,
                null,
                new Acceptors.ObjectAcceptor(null, null, new Altova.Json.PropertyGroup[]{
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("genre", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//genre/@16"), null, new Acceptors.Reference("##fail")),
                       new Acceptors.PropertyRule("count", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//count/@8"), null, new Acceptors.Reference("##fail")),
                       new Acceptors.PropertyRule("shelf", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//shelf/@16"), null, new Acceptors.Reference("##fail")),
                   } ),
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("", Acceptors.NameMatchKind.All, new Acceptors.Reference("##fail"), null, null),
                   } ),
                }),
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//genre/@16", 
                new Acceptors.StringAcceptor(null, null, null, null), 
                null,
                null,
                null,
                null,
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//count/@8", 
                null, 
                new Acceptors.NumberAcceptor(null, null, null, null, null),
                null,
                null,
                null,
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Catalog.Summary.Schema.json#//definitions//Summary//properties//shelf/@16", 
                new Acceptors.StringAcceptor(null, null, null, null), 
                null,
                null,
                null,
                null,
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),

        };
    }    
}
