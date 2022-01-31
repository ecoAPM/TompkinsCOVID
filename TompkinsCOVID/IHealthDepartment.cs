namespace TompkinsCOVID;

public interface IHealthDepartment
{
	Task<IDictionary<DateTime, Record>> GetRecords(string url);
}