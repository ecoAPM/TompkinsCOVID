using System.Collections.Generic;
using System.Threading.Tasks;

namespace TompkinsCOVID
{
    public interface IHealthDepartment
    {
        Task<IList<Record>> GetLatestRecords();
    }
}