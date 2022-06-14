            /*
             * testing linq quries
            Entity newInventoryOwner = new Entity("sln_inventoryowner");

            newInventoryOwner["sln_name"] = "test Linq quireis";

            lateBoundContext.AddObject(newInventoryOwner);

            var result = lateBoundContext.SaveChanges();

            Console.WriteLine($"created With result: {result.ToString()}");
            */
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMDemos
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CrmConnection conn = new CrmConnection("CRMConnection");

            OrganizationService service = new OrganizationService(conn);

            WhoAmIRequest request = new WhoAmIRequest();

            var response = service.Execute<WhoAmIResponse>(request);

            //for the early bound we need while generation ---> give the Result Classes Context to get classes Implements OrganizationServiceContext
            
            OrganizationServiceContext lateBoundContext = new OrganizationServiceContext(service);


            //var lateQuery = from owner in lateBoundContext.CreateQuery("sln_inventoryowner")
            //                where owner["sln_name"] == "IPM CO"
            //                select owner;

            //var lateQuery = lateBoundContext.CreateQuery("sln_inventoryowner").Where((obj) => obj["sln_name"] == "IPM CO");

            var ipmCompany = lateBoundContext.CreateQuery("sln_inventoryowner")
                .Where((o) => o["sln_name"]=="IPM CO")
                .Select((o) => new { Name = o["sln_name"], Id = o["sln_inventoryownerid"] })
                .SingleOrDefault();

            Entity ipmCoTest = new Entity("sln_inventory");

            ipmCoTest["sln_name"] = "IPM CO Inventory";
            
            ipmCoTest["sln_owner"] = new EntityReference("sln_inventoryowner",new Guid(ipmCompany.Id.ToString()));

            lateBoundContext.AddObject(ipmCoTest);
            
            lateBoundContext.SaveChanges();

            //foreach (var record in lateQuery)
            //{
            //    Console.WriteLine($"record Name : {record.Name} with id {record.Id}");
            //}


            Console.WriteLine($"response successeded with client id : {response.UserId}");
            

        }
    }
}
