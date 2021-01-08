using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaberdasheryModel.Data;
using HaberdasheryModel.Models;

public class DbInitializer
{
    public static void Initialize(HaberdasheryContext context)
    {
        context.Database.EnsureCreated();
        if (context.Fabrics.Any())
        {
            return; // BD a fost creata anterior
        }
        var fabrics = new Fabric[]
        {
             new Fabric{Type="Wool",Color="white",Price=Decimal.Parse("21")},
             new Fabric{Type="Wool",Color="red",Price=Decimal.Parse("25")},
             new Fabric{Type="Silk",Color="purple",Price=Decimal.Parse("40")},
             new Fabric{Type="Silk",Color="gold",Price=Decimal.Parse("45")},
             new Fabric{Type="Cotton",Color="green",Price=Decimal.Parse("15")},
             new Fabric{Type="Cotton",Color="yeloow",Price=Decimal.Parse("13")},
        };
        foreach (Fabric f in fabrics)
        {
            context.Fabrics.Add(f);
        }
        context.SaveChanges();
        var customers = new Customer[]
        {
             new Customer{CustomerID=1050,Name="Ionescu Mirela",BirthDate=DateTime.Parse("1979-09-01")},
             new Customer{CustomerID=1045,Name="Pricope Stefan",BirthDate=DateTime.Parse("1998-01-02")},
        };
        foreach (Customer c in customers)
        {
            context.Customers.Add(c);
        }
        context.SaveChanges();
        var orders = new Order[]
        {
             new Order{FabricID=1,CustomerID=1050,OrderDate=DateTime.Parse("02-10-2020")},
             new Order{FabricID=3,CustomerID=1045,OrderDate=DateTime.Parse("09-05-2020")},
             new Order{FabricID=1,CustomerID=1045,OrderDate=DateTime.Parse("10-03-2020")},
             new Order{FabricID=2,CustomerID=1050,OrderDate=DateTime.Parse("09-12-2020")},
             new Order{FabricID=4,CustomerID=1050,OrderDate=DateTime.Parse("09-06-2020")},
             new Order{FabricID=6,CustomerID=1050,OrderDate=DateTime.Parse("10-09-2020")},
 };
        foreach (Order o in orders)
        {
            context.Orders.Add(o);
        }
        context.SaveChanges();
        var weavers = new Weaver[]
        {
             new Weaver{WeaverName="Anghelescu Raluca",Specialty="Wool"},
             new Weaver{WeaverName="Ionescu Voileta",Specialty="Silk"},
             new Weaver{WeaverName="Marin Rica",Specialty="Cotton"},
        };
        foreach (Weaver w in weavers)
        {
            context.Weavers.Add(w);
        }
        context.SaveChanges();
        var fabricsweaved = new FabricWeaved[]
        {
             new FabricWeaved {FabricID = fabrics.Single(c => c.Type == "Wool" ).ID,WeaverID = weavers.Single(i => i.WeaverName == "Anghelescu Raluca").ID},
             new FabricWeaved {FabricID = fabrics.Single(c => c.Type == "Silk" ).ID,WeaverID = weavers.Single(i => i.WeaverName == "Ionescu Voileta").ID},
             new FabricWeaved {FabricID = fabrics.Single(c => c.Type == "Cotton" ).ID,WeaverID = weavers.Single(i => i.WeaverName == "Marin Rica").ID},
        };
        foreach (FabricWeaved fw in fabricsweaved)
        {
            context.FabricWeaved.Add(fw);
        }
        context.SaveChanges();
    }
}
