namespace Durable.Demo.Fanout;

using Durable.Demo.Fanout.Data;
public static class GetData
{
    [FunctionName("Fanout_GetCustomerData")]
    public static List<Customer> Run([ActivityTrigger] Experiment experiment)
    {
        var customers = new List<Customer>();
        var objRandom = new Random(Guid.NewGuid().GetHashCode());
        var maxOrdersPerCustomer = experiment.MaxOrderCount;

        for (var ndx = 0; ndx < experiment.CustomerCount; ndx++)
        {
            var customer = new Customer() {
                CustomerId = ndx,
                OrderCount = objRandom.Next(maxOrdersPerCustomer)
            };
            customers.Add(customer);
        }
        return customers;
    }
}
