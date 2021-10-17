using Acceptors = Altova.Json;
namespace Catalog.MapForceJsonLibs
{
    public class Shelfs 
    {
        public static Acceptors.ValueAcceptor[] Schemas = 
        {
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Shelfs.schema.json#/@32", 
                null, 
                null,
                null,
                null,
                new Acceptors.ArrayAcceptor(null, null, false, new Acceptors.Reference[]{new Acceptors.Reference("file:///C:/TransformMapforce/Shelfs.schema.json#//items/@64"), }),
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Shelfs.schema.json#//items/@64", 
                null, 
                null,
                null,
                null,
                null,
                new Acceptors.ObjectAcceptor(null, null, new Altova.Json.PropertyGroup[]{
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("category", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Shelfs.schema.json#//items//properties//category/@16"), null, new Acceptors.Reference("##fail")),
                       new Acceptors.PropertyRule("shelf", Acceptors.NameMatchKind.Exact, new Acceptors.Reference("file:///C:/TransformMapforce/Shelfs.schema.json#//items//properties//shelf/@16"), null, new Acceptors.Reference("##fail")),
                   } ),
                   new Acceptors.PropertyGroup(Acceptors.PropertyGroupBehavior.Succeed, Acceptors.PropertyGroupBehavior.Fail, Acceptors.PropertyGroupBehavior.ContinueWithNext, new Acceptors.PropertyRule[]{
                       new Acceptors.PropertyRule("", Acceptors.NameMatchKind.All, new Acceptors.Reference("##fail"), null, null),
                   } ),
                }),
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Shelfs.schema.json#//items//properties//category/@16", 
                new Acceptors.StringAcceptor(null, null, null, null), 
                null,
                null,
                null,
                null,
                null,
                new Acceptors.AlsoAcceptor[]{}
            ),
            new Acceptors.ValueAcceptor(
                "file:///C:/TransformMapforce/Shelfs.schema.json#//items//properties//shelf/@16", 
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
